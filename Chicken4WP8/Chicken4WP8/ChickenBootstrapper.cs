using System.Linq;
using System.Windows;
using Autofac;
using Caliburn.Micro;
using Chicken4WP8.Models.Setting;
using Chicken4WP8.Services.Implementation;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8
{
    public class ChickenBootstrapper : AutofacBootstrapper
    {
        public ChickenBootstrapper()
        {
            base.Initialize();
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            //register language helper
            builder.RegisterType<LanguageHelper>()
                .As<ILanguageHelper>()
                .PropertiesAutowired()
                .SingleInstance();

            //register progress service
            builder.RegisterInstance(new ProgressService(RootFrame))
                .As<IProgressService>()
                .PropertiesAutowired()
                .SingleInstance();
            //register storage service
            builder.RegisterType<StorageService>()
                .As<IStorageService>()
                .PropertiesAutowired()
                .SingleInstance();

            var assembiles = AssemblySource.Instance.ToArray();
            //register services
            builder.RegisterAssemblyTypes(assembiles)
                // must be a type which name ends with service
                .Where(type => type.Name.EndsWith("Service"))
                // namespace ends with services implemention
                    .Where(type => type.Namespace.EndsWith("Implementation"))
                    .Except<ProgressService>()
                    .Except<StorageService>()
                // registered as interface
                    .AsImplementedInterfaces()
                    .InstancePerDependency()
                //create new one
                    .InstancePerDependency()
                //auto inject property
                    .PropertiesAutowired();

            //register controllers
            builder.RegisterAssemblyTypes(assembiles)
                //must be a type which name ends with controller
                .Where(type => type.Name.EndsWith("Controller"))
                //starts with base
                .Where(type => type.Name.StartsWith("Base"))
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired()
                .WithMetadata<OAuthTypeMetadata>(
                meta => meta.For(
                    m => m.OAuthType, OAuthSettingType.BaseOAuth));
            builder.RegisterAssemblyTypes(assembiles)
                //must be a type which name ends with controller
                .Where(type => type.Name.EndsWith("Controller"))
                //starts with Customer
                .Where(type => type.Name.StartsWith("Customer"))
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired()
                .WithMetadata<OAuthTypeMetadata>(
                meta => meta.For(
                    m => m.OAuthType, OAuthSettingType.CustomerOAuth));
            builder.RegisterAssemblyTypes(assembiles)
                //must be a type which name ends with controller
                .Where(type => type.Name.EndsWith("Controller"))
                //starts with base
                .Where(type => type.Name.StartsWith("Twip"))
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired()
                .WithMetadata<OAuthTypeMetadata>(
                meta => meta.For(
                    m => m.OAuthType, OAuthSettingType.TwipOAuth));
        }

        protected override void OnUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            //base.OnUnhandledException(sender, e);
        }
    }
}
