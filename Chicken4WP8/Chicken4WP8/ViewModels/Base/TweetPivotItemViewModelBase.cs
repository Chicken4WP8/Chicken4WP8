using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers;

namespace Chicken4WP8.ViewModels.Base
{
    public abstract class TweetPivotItemViewModelBase : PivotItemViewModelBase<ITweetModel>
    {
        #region properties
        private long? sinceId, maxId;

        private List<ITweetModel> fetchedItemsCache = new List<ITweetModel>();
        private List<ITweetModel> loadedItemsCache = new List<ITweetModel>();
        #endregion

        public TweetPivotItemViewModelBase()
        { }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            if (Items == null)
                Items = new ObservableCollection<ITweetModel>();

            await ShowProgressBar();
            //when initialize a pivot item,
            //load data from web first
            await InitLoadDataFromWeb();
            await HideProgressBar();
        }

        #region init load data from web
        private async Task InitLoadDataFromWeb()
        {
            var options = TwitterHelper.GetDictionary();
            options.Add(Const.COUNT, ITEMSPERPAGE);
            var fetchedList = await LoadDataFromWeb(options);
            int count = fetchedList.Count;
            Debug.WriteLine("init loaded data count is: {0}", count);
            if (count >0)
            {
                sinceId = fetchedList.First().Id;
                maxId = fetchedList.Last().Id - 1;

                foreach (var item in fetchedList)
                    Items.Add(item);
            }
        }
        #endregion

        #region fetch data
        protected override async Task FetchMoreDataFromWeb()
        {
            int count = fetchedItemsCache.Count;
            Debug.WriteLine("fetchedItemsCache count is : {0}", count);
            #region add items from cache, with 10 items per action
            if (count > 0)
            {
                if (count > ITEMSPERPAGE)
                {
                    for (int i = 0; i < ITEMSPERPAGE; i++)
                        Items.Insert(0, fetchedItemsCache[count - 1 - i]);
                    fetchedItemsCache.RemoveRange(count - 1 - ITEMSPERPAGE, ITEMSPERPAGE);
                }
                else
                {
                    for (int i = count - 1; i >= 0; i--)
                        Items.Insert(0, fetchedItemsCache[i]);
                    fetchedItemsCache.Clear();
                }
            }
            #endregion
            #region fetch data from derived class
            else
            {
                Debug.WriteLine("fetch data from internet");
                var options = TwitterHelper.GetDictionary();
                if (sinceId != null)
                    options.Add(Const.SINCE_ID, sinceId);
                var fetchedList = await LoadDataFromWeb(options);
                if (fetchedList != null && fetchedList.Count >= 0)
                {
                    Debug.WriteLine("fetced data list count is :{0}", fetchedList.Count);
                    sinceId = fetchedList.First().Id;
                    fetchedItemsCache.AddRange(fetchedList);
                    await FetchMoreDataFromWeb();
                }
                else
                {
                    //no new tweets yet
                }
            }
            #endregion
        }
        #endregion

        #region load data
        protected override async Task LoadMoreDataFromWeb()
        {
            int count = loadedItemsCache.Count;
            Debug.WriteLine("loadedItemsCache count is : {0}", count);
            #region add items from cache, with 10 items per action
            if (count > 0)
            {
                if (count > ITEMSPERPAGE)
                {
                    for (int i = 0; i < ITEMSPERPAGE; i++)
                        Items.Add(loadedItemsCache[i]);
                    loadedItemsCache.RemoveRange(0, ITEMSPERPAGE);
                }
                else
                {
                    foreach (var item in loadedItemsCache)
                        Items.Add(item);
                    loadedItemsCache.Clear();
                }
            }
            #endregion
            #region load data from derived class
            else
            {
                Debug.WriteLine("load data from internet");
                var options = TwitterHelper.GetDictionary();
                if (maxId != null)
                    options.Add(Const.MAX_ID, maxId);
                var loadedList = await LoadDataFromWeb(options);
                Debug.WriteLine("loaded data count is : {0}", loadedList.Count);
                if (loadedList.Count != 0)
                {
                    maxId = loadedList.Last().Id - 1;

                    loadedItemsCache.AddRange(loadedList);
                    await LoadMoreDataFromWeb();
                }
                else
                {
                    //no more tweets
                }
            }
            #endregion
        }
        #endregion

        protected abstract Task<IList<ITweetModel>> LoadDataFromWeb(IDictionary<string, object> options);
    }
}
