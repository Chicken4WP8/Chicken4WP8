using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Chicken4WP8.Common;
using Chicken4WP8.Controllers;
using Chicken4WP8.Entities;
using Chicken4WP8.Models.Setting;
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
                BsonReader reader = new BsonReader(memoryStream);
                result = serializer.Deserialize<T>(reader);
            }
            return result;
        }
        #endregion
    }
}
