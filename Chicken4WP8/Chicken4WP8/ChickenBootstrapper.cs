using System.Linq;
using Autofac;
using Caliburn.Micro;
using Chicken4WP8.Services.Implemention;
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

            var assembiles = AssemblySource.Instance.ToArray();
            //register services
            builder.RegisterAssemblyTypes(assembiles)
                // must be a type which name ends with service
                .Where(type => type.Name.EndsWith("Service"))
                // namespace ends with services implemention
                    .Where(type => type.Namespace.EndsWith("Implemention"))
                // registered as interface
                    .AsImplementedInterfaces()
                //create new one
                    .InstancePerDependency()
                //auto inject property
                    .PropertiesAutowired();

            //register language helper
            builder.RegisterType<LanguageHelper>()
                .As<ILanguageHelper>()
                .SingleInstance();

            //register progress service
            builder.RegisterInstance(new ProgressService(RootFrame))
                .As<IProgressService>()
                .PropertiesAutowired()
                .SingleInstance();
        }
    }
}
