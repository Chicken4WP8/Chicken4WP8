using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.ViewModels.Base
{
    public abstract class TweetPivotItemViewModelBase : PivotItemViewModelBase<ITweetModel>
    {
        #region properties
        private List<ITweetModel> fetchedItemsCache = new List<ITweetModel>();
        private List<ITweetModel> missedItemsCache = new List<ITweetModel>();
        private List<ITweetModel> loadedItemsCache = new List<ITweetModel>();

        public TweetPivotItemViewModelBase(
            IEventAggregator eventAggregator,
            ILanguageHelper languageHelper)
            : base(eventAggregator, languageHelper)
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
        #endregion

        #region init load data from web
        private async Task InitLoadDataFromWeb()
        {
            var options = Const.GetDictionary();
            options.Add(Const.COUNT, Const.DEFAULTCOUNT);
            var fetchedList = await LoadDataFromWeb(options);
            if (fetchedList != null && fetchedList.Count != 0)
            {
                int count = fetchedList.Count;
                Debug.WriteLine("init loaded data count is: {0}", count);
                if (count > 0)
                {
                    foreach (var item in fetchedList)
                        Items.Add(item);
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
            Debug.WriteLine("fetchedItemsCache count is : {0}", count);
            #region add items from cache, with 10 items per action
            if (count > 0)
            {
                if (count > ITEMSPERPAGE)
                {
                    for (int i = 0; i < ITEMSPERPAGE; i++)
                        Items.Insert(0, fetchedItemsCache[count - 1 - i]);
                    fetchedItemsCache.RemoveRange(count - ITEMSPERPAGE, ITEMSPERPAGE);
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
                //step 1: fetch data with since_id:
                Debug.WriteLine("fetch data from internet");
                var options = Const.GetDictionary();
                options.Add(Const.COUNT, Const.DEFAULTCOUNT);
                long? sinceId = null;
                if (Items.Count != 0)
                {
                    sinceId = Items.First().Id;
                    Debug.WriteLine("sinceId is : {0}", sinceId);
                    options.Add(Const.SINCE_ID, sinceId);
                }
                var fetchedList = await LoadDataFromWeb(options);
                #region add to list
                if (fetchedList != null && fetchedList.Count > 0)
                {
                    Debug.WriteLine("fetced data list count is :{0}", fetchedList.Count);
                    //step 2: load data using fetchedList's last item as missed_since_id
                    options.Clear();
                    var missedMaxId = fetchedList.Last().Id;
                    Debug.WriteLine("the last fetched tweet is : {0}", missedMaxId);
                    options.Add(Const.MAX_ID, missedMaxId - 1);
                    options.Add(Const.SINCE_ID, sinceId);
                    options.Add(Const.COUNT, Const.DEFAULTCOUNT);
                    var missedList = await LoadDataFromWeb(options);
                    //step 3: no tweets means no gap,
                    //otherwise, show load more tweets button:
                    if (missedList != null && missedList.Count != 0)
                    {
                        #region remove oldest tweets
                        if (missedItemsCache.Count != 0)
                        {
                            var oldestTweet = Items.Single(t => t.IsLoadMoreTweetButtonVisible);
                            oldestTweet.IsLoadMoreTweetButtonVisible = false;
                            oldestTweet.IsBottomBoundsVisible = false;
                            int index = Items.IndexOf(oldestTweet) + 1;
                            foreach (var item in missedItemsCache)
                            {
                                Items.Insert(index, item);
                                index++;
                            }
                            missedItemsCache.Clear();
                            while (Items.Count > index)
                                Items.RemoveAt(Items.Count - 1);
                        }
                        #endregion
                        #region cache the missed tweets:
                        missedItemsCache.AddRange(missedList);
                        var showedItem = fetchedList.Last();
                        Debug.WriteLine("show load more tweet button at tweet id : {0}", showedItem.Id);
                        showedItem.IsLoadMoreTweetButtonVisible = true;
                        #endregion
                    }
                    fetchedItemsCache.AddRange(fetchedList);
                    Items.First().IsTopBoundsVisible = true;
                    fetchedList.Last().IsBottomBoundsVisible = true;
                    await FetchMoreDataFromWeb();
                }
                #endregion
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
                if (Items.Count == 0)
                {
                    Debug.WriteLine("no items yet, should fetch data first.");
                    await FetchMoreDataFromWeb();
                    return;
                }
                Debug.WriteLine("load data from internet");
                var options = Const.GetDictionary();
                options.Add(Const.COUNT, Const.DEFAULTCOUNT);
                options.Add(Const.MAX_ID, Items.Last().Id - 1);
                var loadedList = await LoadDataFromWeb(options);
                Debug.WriteLine("loaded data count is : {0}", loadedList.Count);
                if (loadedList != null && loadedList.Count != 0)
                {
                    loadedItemsCache.AddRange(loadedList);
                    loadedList.First().IsTopBoundsVisible = true;
                    Items.Last().IsBottomBoundsVisible = true;
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

        #region load more tweets button click
        public async Task LoadMoreTweetsButtonClick(object sender, EventArgs e)
        {
            if (IsLoading)
                return;
            var currentShowedItem = sender as ITweetModel;
            currentShowedItem.IsLoadMoreTweetButtonVisible = false;
            missedItemsCache.First().IsTopBoundsVisible = true;
            missedItemsCache.Last().IsBottomBoundsVisible = true;
            var index = Items.IndexOf(currentShowedItem) + 1;
            foreach (var item in missedItemsCache)
            {
                Items.Insert(index, item);
                index++;
            }
            missedItemsCache.Clear();
            var showedItem = Items[index - 1];
            var missedMaxId = showedItem.Id;
            var sinceId = Items[index].Id;
            var options = Const.GetDictionary();
            options.Add(Const.COUNT, Const.DEFAULTCOUNT);
            options.Add(Const.MAX_ID, missedMaxId - 1);
            options.Add(Const.SINCE_ID, sinceId);
            var missedList = await LoadDataFromWeb(options);
            if (missedList != null && missedList.Count != 0)
            {
                //cache the missed tweets:
                Debug.WriteLine("show load more tweet button at tweet id : {0}", showedItem.Id);
                showedItem.IsLoadMoreTweetButtonVisible = true;
                missedItemsCache.AddRange(missedList);
            }
        }
        #endregion

        protected abstract Task<IList<ITweetModel>> LoadDataFromWeb(IDictionary<string, object> options);
    }
}
