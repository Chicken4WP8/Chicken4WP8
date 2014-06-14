using System;
using System.Linq;
using Chicken4WP8.Common;
using Chicken4WP8.Entities;
using Chicken4WP8.Services.Interface;
using LinqToTwitter;
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
                Initialize(ctx);
            }
        }

        private static void Initialize(ChickenDataContext context)
        {
            var credential = new InMemoryCredentialStore
            {
                ConsumerKey = "pPnxpn00RbGx3YJJtvYUsA",
                ConsumerSecret = "PoX3exts23HJ1rlMaPr6RtlX2G5VQdrqbpUWpkMcCo"
            };
            var baseOAuth = new Setting
            {
                Id = 0,
                Category = SettingCategory.OAuthSetting,
                IsCurrentlyInUsed = true,
                Name = Const.OAUTH_MODE_BASE,
                Data = JsonConvert.SerializeObject(credential, Const.JsonSettings)
            };
            context.Settings.InsertOnSubmit(baseOAuth);
            context.SubmitChanges();
        }

        public StorageService()
        {
            this.context = new ChickenDataContext();
        }

        public ICredentialStore GetCurrentOAuthSetting()
        {
            var oauth = context.Settings.FirstOrDefault(s => s.Category == SettingCategory.OAuthSetting && s.IsCurrentlyInUsed);
            if (oauth == null || string.IsNullOrEmpty(oauth.Data))
                return null;
            switch (oauth.Name)
            {
                case Const.OAUTH_MODE_BASE:
                    return JsonConvert.DeserializeObject<InMemoryCredentialStore>(oauth.Data);
                default:
                    return null;
            }
        }

        public void UpdageCurrentSetting(ICredentialStore CredentialStore)
        {
            throw new NotImplementedException();
        }
    }
}
