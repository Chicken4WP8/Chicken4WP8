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
        public static AutoResetEvent resetEvent = new AutoResetEvent(true);
        private ChickenDataContext context;
        private const string EMOTIONS_FILE_NAME = "emotions.json";

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

        public ITweetModel GetTempTweet()
        {
            var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.TweetDetail);
            if (entity == null || entity.Data == null)
                return null;
            return DeserializeObject<ITweetModel>(entity.Data);
        }

        public void UpdateTempTweet(ITweetModel tweet)
        {
            var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.TweetDetail);
            if (entity == null)
            {
                entity = new TempData { Type = TempType.TweetDetail };
                context.TempDatas.InsertOnSubmit(entity);
            }
            entity.Data = SerializeObject(tweet);
            context.SubmitChanges();
        }

        public string GetCachedUserName()
        {
            var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.UserProfile);
            return Encoding.Unicode.GetString(entity.Data, 0, entity.Data.Length);
        }

        public void AddOrUpdateUserName(string name)
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

        public IUserModel GetCachedUser(string name)
        {
            var entity = context.CachedUsers.FirstOrDefault(u => u.Id == name);
            if (entity == null || entity.Data == null || DateTime.Now - entity.InsertedTime > Const.EXPERIEDTIME)
                return null;
            return DeserializeObject<IUserModel>(entity.Data);
        }

        public void AddOrUpdateUserCache(IUserModel user)
        {
            try
            {
                resetEvent.WaitOne();
                var entity = context.CachedUsers.FirstOrDefault(u => u.Id == user.ScreenName);
                if (entity == null)
                {
                    entity = new CachedUser { Id = user.ScreenName };
                    context.CachedUsers.InsertOnSubmit(entity);
                }
                entity.Data = SerializeObject(user);
                entity.InsertedTime = DateTime.Now;

                context.SubmitChanges();
            }
            catch (Exception)
            { }
            finally
            {
                resetEvent.Set();
            }
        }

        public byte[] GetCachedImage(string id)
        {
            CachedImage image = null;
            if (string.IsNullOrEmpty(id))
                image = context.CachedImages.FirstOrDefault(c => c.Id == id);
            if (image != null)
                return image.Data;
            return null;
        }

        public byte[] AddOrUpdateImageCache(string id, byte[] data)
        {
            try
            {
                resetEvent.WaitOne();
                var image = context.CachedImages.FirstOrDefault(c => c.Id == id);
                if (image == null)
                {
                    image = new CachedImage { Id = id };
                    context.CachedImages.InsertOnSubmit(image);
                }
                image.Data = data;
                context.SubmitChanges();
            }
            catch (Exception e)
            { }
            finally
            {
                resetEvent.Set();
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

        public string GetDirectMessageUserName()
        {
            var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.DirectMessage);
            return Encoding.Unicode.GetString(entity.Data, 0, entity.Data.Length);
        }

        public void AddOrUpdateDirectMessageUserName(string name)
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

        public NewStatusModel GetTempNewStatus()
        {
            var entity = context.TempDatas.FirstOrDefault(t => t.Type == TempType.NewStatus);
            if (entity == null || entity.Data == null)
                return null;
            return DeserializeObject<NewStatusModel>(entity.Data);
        }

        public void UpdateTempNewStatus(NewStatusModel status)
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

        public T GetTombstoningData<T>(TombstoningType type, string id)
            where T : TombstoningDataBase
        {
            var entity = context.TombstoningDatas.FirstOrDefault(t => t.Type == type && t.Key == id);
            if (entity == null || entity.Data == null)
                return default(T);
            return DeserializeObject<T>(entity.Data);
        }

        public void AddOrUpdateTombstoningData<T>(TombstoningType type, string id, T data)
            where T : TombstoningDataBase
        {
            var entity = context.TombstoningDatas.FirstOrDefault(t => t.Type == type && t.Key == id);
            if (entity == null)
            {
                entity = new TombstoningData { Type = type, Key = id };
                context.TombstoningDatas.InsertOnSubmit(entity);
            }
            entity.Data = SerializeObject(data);
            context.SubmitChanges();
        }

        public List<string> GetEmotions()
        {
            List<string> result = new List<string>();
            string filepath = Path.Combine(EMOTIONS_FILE_NAME);
            IsolatedStorageFile fileSystem = IsolatedStorageFile.GetUserStoreForApplication();
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
