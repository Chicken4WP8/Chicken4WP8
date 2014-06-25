using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Chicken4WP8.Services.Implemention;
using Chicken4WP8.Services.Interface;

namespace Chicken4WP8.AutofacModules
{
    public class LanguageModule : Autofac.Module
    {
        private static Type @interface = typeof(ILanguageHelper);

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            //register language helper
            builder.RegisterType<LanguageHelper>()
                .As<ILanguageHelper>()
                .SingleInstance();
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            var type = registration.Activator.LimitType;
            var propertyInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p => p.PropertyType == @interface);
            if (propertyInfo == null)
                return;
            registration.Activated += (o, e) =>
            {
                var helper = e.Context.Resolve<ILanguageHelper>();
                propertyInfo.SetValue(e.Instance, helper);
            };
        }
    }
}
