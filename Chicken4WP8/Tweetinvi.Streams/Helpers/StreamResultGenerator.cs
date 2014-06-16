using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Exceptions;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Streams.Properties;

namespace Tweetinvi.Streams.Helpers
{
    /// <summary>
    /// Extract objects from any kind of stream
    /// </summary>
    public class StreamResultGenerator : IStreamResultGenerator
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly IWebHelper _webHelper;
        private const int STREAM_RESUME_DELAY = 1000;

        public event EventHandler StreamStarted;
        public event EventHandler StreamResumed;
        public event EventHandler StreamPaused;
        public event EventHandler<StreamExceptionEventArgs> StreamStopped;

        private WebRequest _currentWebRequest;
        private StreamReader _currentReader;
        private Exception _lastException;

        private bool IsRunning
        {
            get { return _streamState == StreamState.Resume || _streamState == StreamState.Pause; }
        }

        private StreamState _streamState;
        public StreamState StreamState
        {
            get { return _streamState; }
            set
            {
                if (_streamState != value)
                {
                    _streamState = value;

                    switch (_streamState)
                    {
                        case StreamState.Resume:
                            this.Raise(StreamResumed);
                            break;
                        case StreamState.Pause:
                            this.Raise(StreamPaused);
                            break;
                        case StreamState.Stop:
                            var streamExceptionEventArgs = new StreamExceptionEventArgs(_lastException);
                            this.Raise(StreamStopped, streamExceptionEventArgs);
                            break;
                    }
                }
            }
        }

        public StreamResultGenerator(IExceptionHandler exceptionHandler, IWebHelper webHelper)
        {
            _exceptionHandler = exceptionHandler;
            _webHelper = webHelper;
        }

        public async Task StartStreamAsync(Action<string> processObject, Func<HttpWebRequest> generateWebRequest)
        {
            Func<string, bool> processValidObject = json =>
            {
                processObject(json);
                return true;
            };

            await StartStreamAsync(processValidObject, generateWebRequest);
        }

        public async Task StartStreamAsync(Func<string, bool> processObject, Func<HttpWebRequest> generateWebRequest)
        {
            if (IsRunning)
            {
                throw new OperationCanceledException(Resources.Stream_IllegalMultipleStreams);
            }

            if (processObject == null)
            {
                throw new NullReferenceException(Resources.Stream_ObjectDelegateIsNull);
            }

            _lastException = null;
            _streamState = StreamState.Resume;
            this.Raise(StreamStarted);

            _currentWebRequest = generateWebRequest();
            _currentReader = await InitWebRequest(_currentWebRequest);

            if (_lastException != null)
            {
                _streamState = StreamState.Stop;
            }

            int errorOccured = 0;
            while (StreamState != StreamState.Stop)
            {
                if (StreamState == StreamState.Pause)
                {
                    using (EventWaitHandle tmpEvent = new ManualResetEvent(false))
                    {
                        tmpEvent.WaitOne(TimeSpan.FromSeconds(STREAM_RESUME_DELAY));
                    }
                    continue;
                }

                try
                {
                    string jsonResponse = await _currentReader.ReadLineAsync();

                    #region Error Checking

                    if (jsonResponse == null)
                    {
                        if (errorOccured == 0)
                        {
                            ++errorOccured;
                            continue;
                        }
                        
                        if (errorOccured == 1)
                        {
                            ++errorOccured;
                            _currentWebRequest.Abort();
                            _currentReader = await InitWebRequest(_currentWebRequest);
                            continue;
                        }
                        
                        if (errorOccured == 2)
                        {
                            ++errorOccured;
                            _currentWebRequest.Abort();
                            _currentWebRequest = generateWebRequest();
                            _currentReader = await InitWebRequest(_currentWebRequest);
                            continue;
                        }
                        
                        break;
                    }
                    
                    errorOccured = 0;

                    #endregion

                    if (jsonResponse == String.Empty)
                    {
                        continue;
                    }

                    if (StreamState == StreamState.Resume && !processObject(jsonResponse))
                    {
                        StreamState = StreamState.Stop;
                        break;
                    }
                }
                catch (WebException wex)
                {
                    _exceptionHandler.AddWebException(wex, String.Empty);
                    
                    if (!_exceptionHandler.SwallowWebExceptions)
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                    if (ex is IOException && StreamState == StreamState.Stop)
                    {
                        _lastException = null;
                    }

                    break;
                }
            }

            if (_currentWebRequest != null)
            {
                _currentWebRequest.Abort();
            }

            if (_currentReader != null)
            {
                _currentReader.Dispose();
            }

            StreamState = StreamState.Stop;
        }

        private async Task<StreamReader> InitWebRequest(WebRequest webRequest)
        {
            StreamReader reader = null;

            try
            {
                var responseStream = await _webHelper.GetResponseStreamAsync(webRequest);
                if (responseStream != null)
                {
                    reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                }
            }
            catch (WebException wex)
            {
                _exceptionHandler.AddWebException(wex, webRequest.RequestUri.AbsoluteUri);

                if (!_exceptionHandler.SwallowWebExceptions)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    if (ex.Message == "Stream was not readable.")
                    {
                        webRequest.Abort();
                    }
                }

                _lastException = ex;
                StreamState = StreamState.Stop;
            }

            return reader;
        }

        public void ResumeStream()
        {
            StreamState = StreamState.Resume;
        }

        public void PauseStream()
        {
            StreamState = StreamState.Pause;
        }

        public void StopStream()
        {
            StreamState = StreamState.Stop;

            if (_currentWebRequest != null)
            {
                _currentWebRequest.Abort();
            }
        }

        public void StopStream(Exception exception, IDisconnectMessage disconnectMessage = null)
        {
            _lastException = exception;
            _streamState = StreamState.Stop;
            var streamExceptionEventArgs = new StreamExceptionEventArgs(exception, disconnectMessage);
            this.Raise(StreamStopped, streamExceptionEventArgs);
        }
    }
}