using System;
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
        private long? sinceId, maxId, missedSinceId, missedMaxId;
        /// <summary>
        /// index of to be inserted missed item
        /// </summary>
        private int indexOfMissedItem;
        private bool isMissedSinceIdInit;

        private List<ITweetModel> fetchedItemsCache = new List<ITweetModel>();
        private List<ITweetModel> loadedItemsCache = new List<ITweetModel>();

        private bool isLoadMissedItemsButtonVisible;
        public bool IsLoadMissedItemsButtonVisible
        {
            get { return isLoadMissedItemsButtonVisible; }
            set
            {
                isLoadMissedItemsButtonVisible = value;
                NotifyOfPropertyChange(() => IsLoadMissedItemsButtonVisible);
            }
        }
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
            await LoadMoreDataFromDatabase();
            //then fetch data from web
            await FetchMoreDataFromWeb();
            await HideProgressBar();
        }

        public async virtual void LoadMissedItemButtonClick(object sender, EventArgs e)
        {
            if (IsLoading)
                return;
            await ShowProgressBar();
            await LoadMissedDataFromWeb();
            await HideProgressBar();
        }

        #region load data from database
        private async Task LoadMoreDataFromDatabase()
        {
            //load from database
            var list = await LoadDataFromDatabase();
            if (list != null && list.Count() != 0)
            {
                maxId = list.Last().Id;
                //initialize the missedMaxId, only once.
                //the missedMaxId should not substract 1,
                //because we need to know the missed max tweet's exact id
                missedMaxId = list.First().Id;

                foreach (var item in list)
                    Items.Add(item);
            }
        }
        #endregion

        #region load missed data
        protected async Task LoadMissedDataFromWeb()
        {
            Debug.WriteLine("load missed data from web, missedMaxId is : {0}, missedSinceId is : {1}", missedMaxId, missedSinceId);
            var options = TwitterHelper.GetDictionary();
            options.Add(Const.SINCE_ID, missedSinceId);
            options.Add(Const.MAX_ID, missedMaxId);
            var missedList = await LoadDataFromWeb(options);
            Debug.WriteLine("missed data count is :{0}", missedList.Count());
            if (missedList.Count() != 0)
            {
                missedSinceId = missedList.Last().Id;
                Debug.WriteLine("missedSinceId is :{0}, missedMaxId is :{1}", missedSinceId, missedMaxId);

                if (missedSinceId <= missedMaxId)
                    IsLoadMissedItemsButtonVisible = false;

                foreach (var item in missedList)
                {
                    Items.Insert(indexOfMissedItem, item);
                    indexOfMissedItem++;
                }
            }
            else
            { }
        }
        #endregion

        #region fetch data
        protected override async Task FetchMoreDataFromWeb()
        {
            int count = fetchedItemsCache.Count;
            Debug.WriteLine("realizedFetchedItems' count is: {0}", count);
            #region add items from cache, with 10 items per action
            if (count > 0)
            {
                if (count > ITEMSPERPAGE)
                {
                    for (int i = 0; i < ITEMSPERPAGE; i++)
                        Items.Insert(0, fetchedItemsCache[count - 1 - i]);
                    fetchedItemsCache.RemoveRange(count - 1 - ITEMSPERPAGE, ITEMSPERPAGE);
                    indexOfMissedItem += ITEMSPERPAGE;
                }
                else
                {
                    for (int i = count - 1; i >= 0; i--)
                        Items.Insert(0, fetchedItemsCache[i]);
                    indexOfMissedItem += count;
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
                if (maxId != null)
                    options.Add(Const.MAX_ID, maxId);
                var fetchedList = await LoadDataFromWeb(options);
                Debug.WriteLine("fetced data count is :{0}", fetchedList.Count());
                if (fetchedList.Count() != 0)
                {
                    sinceId = fetchedList.First().Id;

                    if (!isMissedSinceIdInit)
                    {
                        isMissedSinceIdInit = true;
                        missedSinceId = fetchedList.Last().Id;
                        if (missedSinceId > missedMaxId)
                            IsLoadMissedItemsButtonVisible = true;
                    }

                    fetchedItemsCache.AddRange(fetchedList);
                    await FetchMoreDataFromWeb();
                }
                else
                { }
            }
            #endregion
        }
        #endregion

        #region load data
        protected override async Task LoadMoreDataFromWeb()
        {
            int count = loadedItemsCache.Count;
            Debug.WriteLine("realized loaded items' count is: {0}", count);
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
                Debug.WriteLine("loaded data count is: {0}", loadedList.Count());
                if (loadedList.Count() != 0)
                {
                    maxId = loadedList.Last().Id - 1;

                    loadedItemsCache.AddRange(loadedList);
                    await LoadMoreDataFromWeb();
                }
                else
                { }
            }
            #endregion
        }
        #endregion

        protected abstract Task<IEnumerable<ITweetModel>> LoadDataFromDatabase();

        protected abstract Task<IEnumerable<ITweetModel>> LoadDataFromWeb(IDictionary<string, object> options);
    }
}
