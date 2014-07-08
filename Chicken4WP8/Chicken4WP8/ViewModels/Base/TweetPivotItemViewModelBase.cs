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
            //load data from database first
            await InitLoadDataFromDatabase();
            //then load data from web
            await InitLoadDataFromWeb();
            await HideProgressBar();
        }

        #region init load data from database
        private async Task InitLoadDataFromDatabase()
        {
            //load from database
            var list = await LoadDataFromDatabase();
            if (list != null && list.Count != 0)
            {
                sinceId = list.First().Id - 1;
                maxId = list.Last().Id - 1;

                foreach (var item in list)
                    Items.Add(item);
            }
        }
        #endregion

        #region init load data from web
        private async Task InitLoadDataFromWeb()
        {
            var options = TwitterHelper.GetDictionary();
            options.Add(Const.COUNT, ITEMSPERPAGE + 1);
            if (sinceId != null)
                options.Add(Const.SINCE_ID, sinceId);
            var fetchedList = await LoadDataFromWeb(options);
            int count = fetchedList.Count;
            Debug.WriteLine("init loaded data count is: {0}", count);
            if (count >= 1)
            {
                if (sinceId != null && fetchedList.Last().Id > sinceId.Value)
                {
                    maxId = fetchedList.Last().Id - 1;
                    Items.Clear();
                }
                fetchedList.RemoveAt(count - 1);
                foreach (var item in fetchedList)
                    Items.Add(item);
                sinceId = fetchedList.First().Id - 1;
                if(maxId==null)
                    maxId = fetchedList.Last().Id - 1;
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
                if (fetchedList != null && fetchedList.Count >= 1)
                {
                    Debug.WriteLine("fetced data list count is :{0}", fetchedList.Count);
                    sinceId = fetchedList.First().Id - 1;
                    fetchedList.RemoveAt(fetchedList.Count - 1);
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

        protected abstract Task<IList<ITweetModel>> LoadDataFromDatabase();

        protected abstract Task<IList<ITweetModel>> LoadDataFromWeb(IDictionary<string, object> options);
    }
}
