using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers;
using Chicken4WP8.Entities;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Models.Tombstoning;
using Chicken4WP8.Services.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Chicken4WP8.Services.Implementation
{
    public class StorageService : IStorageService
    {
        #region properties
        private static JsonSerializer serializer;
        private static IsolatedStorageFile fileSystem = IsolatedStorageFile.GetUserStoreForApplication();
        private static object locker = new object();
        private ChickenDataContext context;
        private const string EMOTIONS_FILE_NAME = "emotions.json";
        private const string IMAGE_PATH = "images";

        static StorageService()
        {
            serializer = JsonSerializer.Create(Const.JsonSettings);

            var ctx = new ChickenDataContext();
            if (!ctx.DatabaseExists())
            {
                ctx.CreateDatabase();
            }
        }

        public StorageService()
        {
            this.context = new ChickenDataContext();
        }
        #endregion

        #region settings
        public UserSetting GetCurrentUserSetting()
        {
            var entity = context.Settings.FirstOrDefault(s => s.Category == SettingCategory.CurrentUserSetting && s.IsCurrentlyInUsed);
            if (entity == null || entity.Data == null)
                return null;
            return DeserializeObject<UserSetting>(entity.Data);
        }

        public void UpdateCurrentUserSetting(UserSetting setting)
        {
            var entity = context.Settings.FirstOrDefault(s => s.Category == SettingCategory.CurrentUserSetting && s.IsCurrentlyInUsed);
            if (entity == null) //add new
            {
                entity = new Setting
                {
                    Category = SettingCategory.CurrentUserSetting,
                    Name = setting.Name,
                };
                context.Settings.InsertOnSubmit(entity);
            }
            entity.IsCurrentlyInUsed = true;
            entity.Data = SerializeObject(setting);
            context.SubmitChanges();
        }

        public string GetCurrentLanguage()
        {
            var setting = context.Settings.FirstOrDefault(s => s.Category == SettingCategory.LanguageSetting && s.IsCurrentlyInUsed);
            if (setting != null)
                return setting.Name;
            return string.Empty;
        }

        public void UpdateLanguage(string name)
        {
            var setting = context.Settings.FirstOrDefault(s => s.Category == SettingCategory.LanguageSetting && s.IsCurrentlyInUsed);
            if (setting == null)
            {
                setting = new Setting
                {
                    Category = SettingCategory.LanguageSetting,
                };
                context.Settings.InsertOnSubmit(setting);
            }
            setting.IsCurrentlyInUsed = true;
            setting.Name = name;
            context.SubmitChanges();
        }
        #endregion

        #region temp data
        public ITweetModel GetTempTweet()
        {
            var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.TweetDetail);
            var tweetId = Encoding.Unicode.GetString(entity.Data, 0, entity.Data.Length);
            return GetCachedTweet(tweetId);
        }

        public void UpdateTempTweetId(long tweetId)
        {
            lock (locker)
            {
                var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.TweetDetail);
                if (entity == null)
                {
                    entity = new TempData { Type = TempType.TweetDetail };
                    context.TempDatas.InsertOnSubmit(entity);
                }
                entity.Data = Encoding.Unicode.GetBytes(tweetId.ToString());
                context.SubmitChanges();
            }
        }

        public string GetTempUserName()
        {
            var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.UserProfile);
            var name = Encoding.Unicode.GetString(entity.Data, 0, entity.Data.Length);
            return name;
        }

        public void UpdateTempUserName(string name)
        {
            lock (locker)
            {
                var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.UserProfile);
                if (entity == null)
                {
                    entity = new TempData { Type = TempType.UserProfile };
                    context.TempDatas.InsertOnSubmit(entity);
                }
                entity.Data = Encoding.Unicode.GetBytes(name);
                context.SubmitChanges();
            }
        }

        public string GetTempDirectMessageUserName()
        {
            var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.DirectMessage);
            return Encoding.Unicode.GetString(entity.Data, 0, entity.Data.Length);
        }

        public void UpdateTempDirectMessageUserName(string name)
        {
            lock (locker)
            {
                var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.DirectMessage);
                if (entity == null)
                {
                    entity = new TempData { Type = TempType.DirectMessage };
                    context.TempDatas.InsertOnSubmit(entity);
                }
                entity.Data = Encoding.Unicode.GetBytes(name);
                context.SubmitChanges();
            }
        }

        public NewTweetModel GetTempNewTweet()
        {
            var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.NewStatus);
            if (entity == null || entity.Data == null)
                return null;
            return DeserializeObject<NewTweetModel>(entity.Data);
        }

        public void UpdateTempNewTweet(NewTweetModel status)
        {
            lock (locker)
            {
                var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.NewStatus);
                if (entity == null)
                {
                    entity = new TempData { Type = TempType.NewStatus };
                    context.TempDatas.InsertOnSubmit(entity);
                }
                entity.Data = SerializeObject(status);
                context.SubmitChanges();
            }
        }
        #endregion

        #region cached data
        public ITweetModel GetCachedTweet(string tweetId)
        {
            long id = long.Parse(tweetId);
            var entity = context.CachedTweets.FirstOrDefault(u => u.Id == id);
            if (entity == null || entity.Data == null)
                return null;
            return DeserializeObject<ITweetModel>(entity.Data);
        }

        public void AddCachedTweets(IEnumerable<ITweetModel> tweets)
        {
            lock (locker)
            {
                foreach (var tweet in tweets)
                {
                    #region cache tweets
                    var entity = context.CachedTweets.FirstOrDefault(t => t.Id == tweet.Id);
                    if (entity == null)
                    {
                        entity = new CachedTweet { Id = tweet.Id };
                        context.CachedTweets.InsertOnSubmit(entity);
                    }
                    entity.Data = SerializeObject(tweet);
                    #endregion

                    #region cache users
                    var user = context.CachedUsers.FirstOrDefault(u => u.Id == tweet.User.ScreenName);
                    if (user == null)
                    {
                        user = new CachedUser { Id = tweet.User.ScreenName };
                        context.CachedUsers.InsertOnSubmit(user);
                    }
                    user.Data = SerializeObject(tweet.User);

                    #region retweet users
                    if (tweet.RetweetedStatus != null && tweet.RetweetedStatus.User != null)
                    {
                        var origin = context.CachedUsers.FirstOrDefault(u => u.Id == tweet.RetweetedStatus.User.ScreenName);
                        if (origin == null)
                        {
                            origin = new CachedUser { Id = tweet.RetweetedStatus.User.ScreenName };
                            context.CachedUsers.InsertOnSubmit(origin);
                        }
                        origin.Data = SerializeObject(tweet.RetweetedStatus.User);
                    }
                    #endregion
                    #endregion
                }
                context.SubmitChanges();
            }
        }

        public IUserModel GetCachedUser(string name)
        {
            var entity = context.CachedUsers.FirstOrDefault(u => u.Id == name);
            if (entity == null || entity.Data == null)
                return null;
            return DeserializeObject<IUserModel>(entity.Data);
        }

        public void AddOrUpdateCachedUser(IUserModel user)
        {
            lock (locker)
            {
                var entity = context.CachedUsers.FirstOrDefault(u => u.Id == user.ScreenName);
                if (entity == null)
                {
                    entity = new CachedUser { Id = user.ScreenName };
                    context.CachedUsers.InsertOnSubmit(entity);
                }
                entity.Data = SerializeObject(user);
                context.SubmitChanges();
            }
        }

        public void AddCachedUsers(IEnumerable<IUserModel> users)
        {
            lock (locker)
            {
                foreach (var user in users)
                {
                    var entity = context.CachedUsers.FirstOrDefault(u => u.Id == user.ScreenName);
                    if (entity == null)
                    {
                        entity = new CachedUser { Id = user.ScreenName };
                        context.CachedUsers.InsertOnSubmit(entity);
                    }
                    entity.Data = SerializeObject(user);
                }
                context.SubmitChanges();
            }
        }

        public IFriendshipModel GetCachedFriendship(string name)
        {
            var entity = context.CachedFriendships.FirstOrDefault(u => u.Id == name);
            if (entity == null || entity.Data == null)
                return null;
            return DeserializeObject<IFriendshipModel>(entity.Data);
        }

        public void AddOrUpdateCachedFriendship(IFriendshipModel friendship)
        {
            lock (locker)
            {
                var entity = context.CachedFriendships.FirstOrDefault(u => u.Id == friendship.ScreenName);
                if (entity == null)
                {
                    entity = new CachedFriendship { Id = friendship.ScreenName };
                    context.CachedFriendships.InsertOnSubmit(entity);
                }
                entity.Data = SerializeObject(friendship);
                context.SubmitChanges();
            }
        }

        public byte[] GetCachedImage(string id)
        {
            try
            {
                byte[] bytes = null;
                var md5 = MD5.GetMd5String(id);
                string filepath = Path.Combine(IMAGE_PATH, md5);
                if (!fileSystem.FileExists(filepath))
                    return null;
                using (var fileStream = fileSystem.OpenFile(filepath, FileMode.Open))
                {
                    using (var memStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memStream);
                        bytes = memStream.ToArray();
                    }
                }
                return bytes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public byte[] AddOrUpdateImageCache(string id, byte[] data)
        {
            try
            {
                if (!fileSystem.DirectoryExists(IMAGE_PATH))
                    fileSystem.CreateDirectory(IMAGE_PATH);
                var md5 = MD5.GetMd5String(id);
                string filepath = Path.Combine(IMAGE_PATH, md5);
                if (fileSystem.FileExists(filepath))
                    fileSystem.DeleteFile(filepath);
                using (var fileStream = fileSystem.OpenFile(filepath, FileMode.Create))
                {
                    fileStream.Write(data, 0, data.Length);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return data;
        }

        public long? GetSendDirectMessageSinceId()
        {
            if (context.CachedDirectMessages.Count() == 0)
                return null;
            return context.CachedDirectMessages.Where(m => m.IsSentByMe).Max(m => m.Id);
        }

        public long? GetSendDirectMessageMaxId()
        {
            if (context.CachedDirectMessages.Count() == 0)
                return null;
            return context.CachedDirectMessages.Where(m => m.IsSentByMe).Min(m => m.Id);
        }

        public long? GetReceivedDirectMessageMaxId()
        {
            if (context.CachedDirectMessages.Count() == 0)
                return null;
            return context.CachedDirectMessages.Where(m => !m.IsSentByMe).Max(m => m.Id);
        }

        public long? GetReceivedDirectMessageSinceId()
        {
            if (context.CachedDirectMessages.Count() == 0)
                return null;
            return context.CachedDirectMessages.Where(m => !m.IsSentByMe).Max(m => m.Id);
        }

        public void AddCachedDirectMessages(IEnumerable<IDirectMessageModel> messages)
        {
            lock (locker)
            {
                foreach (var message in messages)
                {
                    var entity = new CachedDirectMessage
                    {
                        Id = message.Id,
                        UserId = message.User.Id.Value,
                        IsSentByMe = message.IsSentByMe,
                        Data = SerializeObject(message)
                    };
                    context.CachedDirectMessages.InsertOnSubmit(entity);
                }
                context.SubmitChanges();
            }
        }

        public List<IDirectMessageModel> GetGroupedDirectMessages()
        {
            var list = new List<IDirectMessageModel>();
            var entities = context.CachedDirectMessages
                .OrderByDescending(m => m.Id)
                .GroupBy(m => m.UserId)
                .Select(g => g.First())
                .ToList();
            foreach (var entity in entities)
            {
                list.Add(DeserializeObject<IDirectMessageModel>(entity.Data));
            }
            return list;
        }
        #endregion

        #region tombstoning
        public T GetTombstoningData<T>(TombstoningType type, string id)
    where T : TombstoningDataBase
        {
            var entity = context.TombstoningDatas.FirstOrDefault(t => t.Type == type && t.Id == id);
            if (entity == null || entity.Data == null)
                return default(T);
            return DeserializeObject<T>(entity.Data);
        }

        public void AddOrUpdateTombstoningData<T>(TombstoningType type, string id, T data)
            where T : TombstoningDataBase
        {
            lock (locker)
            {
                var entity = context.TombstoningDatas.FirstOrDefault(t => t.Type == type && t.Id == id);
                if (entity == null)
                {
                    entity = new TombstoningData { Type = type, Id = id };
                    context.TombstoningDatas.InsertOnSubmit(entity);
                }
                entity.Data = SerializeObject(data);
                context.SubmitChanges();
            }
        }
        #endregion

        public List<string> GetEmotions()
        {
            List<string> result = new List<string>();
            string filepath = Path.Combine(EMOTIONS_FILE_NAME);
            using (var fileStream = fileSystem.OpenFile(filepath, FileMode.OpenOrCreate))
            {
                if (fileStream == null || fileStream.Length == 0)
                {
                    var resource = Application.GetResourceStream(new Uri(Path.Combine("Data", filepath), UriKind.Relative));
                    resource.Stream.CopyTo(fileStream);
                }
                using (var streamReader = new StreamReader(fileStream))
                {
                    fileStream.Position = 0;
                    while (!streamReader.EndOfStream)
                    {
                        string s = streamReader.ReadLine();
                        if (!string.IsNullOrEmpty(s))
                        {
                            result.Add(s.Trim());
                        }
                    }
                }
            }
            return result;
        }

        #region private
        private byte[] SerializeObject(object value)
        {
            using (var memoryStream = new MemoryStream())
            {
                BsonWriter writer = new BsonWriter(memoryStream);
                serializer.Serialize(writer, value);
                return memoryStream.ToArray();
            }
        }

        private T DeserializeObject<T>(byte[] data)
        {
            var result = default(T);
            using (var memoryStream = new MemoryStream(data))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                var reader = new BsonReader(memoryStream);
                var stream = new StreamReader(memoryStream);
                result = serializer.Deserialize<T>(reader);
            }
            return result;
        }
        #endregion
    }
}
