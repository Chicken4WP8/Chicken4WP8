using System.Linq;
using Chicken4WP8.Common;
using Chicken4WP8.Entities;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Interface;
using Newtonsoft.Json;

namespace Chicken4WP8.Services.Implemention
{
    public class StorageService : IStorageService
    {
        private ChickenDataContext context;

        static StorageService()
        {
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

        public UserSetting GetCurrentUserSetting()
        {
            var entity = context.Settings.FirstOrDefault(s => s.Category == SettingCategory.CurrentUserSetting && s.IsCurrentlyInUsed);
            if (entity == null || entity.Data == null)
                return null;
            return JsonConvert.DeserializeObject<UserSetting>(entity.Data, Const.JsonSettings);
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
            entity.Data = JsonConvert.SerializeObject(setting, Const.JsonSettings);
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

        public byte[] GetCachedImage(string id)
        {
            CachedImage image = null;
            if (string.IsNullOrEmpty(id))
                image = context.CachedImages.FirstOrDefault(c => c.Id == id);
            if (image != null)
                return image.Data;
            return null;
        }

        public void AddOrUpdateImageCache(string id, byte[] data)
        {
            var image = context.CachedImages.FirstOrDefault(c => c.Id == id);
            if (image == null)
            {
                image = new CachedImage { Id = id };
                context.CachedImages.InsertOnSubmit(image);
            }
            image.Data = data;
            context.SubmitChanges();
        }
    }
}
