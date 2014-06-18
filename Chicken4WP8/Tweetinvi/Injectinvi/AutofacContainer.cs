﻿using System;
using System.Collections.Generic;
using Autofac;
using Tweetinvi.Controllers;
using Tweetinvi.Core;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Credentials;
using Tweetinvi.Factories;
using Tweetinvi.Logic;
using Tweetinvi.Resource;
using Tweetinvi.Streams;
using Tweetinvi.WebLogic;

namespace Tweetinvi.Injectinvi
{
    internal interface IAutofacContainer : ITweetinviContainer
    {
    }

    public class AutofacContainer : IAutofacContainer
    {
        private static IContainer _container;
        private ContainerBuilder _containerBuilder;
        private List<ITweetinviModule> _moduleCatalog;

        [ThreadStatic]
        private static ITweetinviContainer _threadContainer;
        private static ITweetinviContainer ThreadContainer
        {
            get
            {
                if (_threadContainer == null)
                {
                    _threadContainer = GetThreadContainer();
                }

                return _threadContainer;
            }
        }

        public AutofacContainer()
        {
            Initialize();
        }

        public void Initialize()
        {
            _containerBuilder = new ContainerBuilder();
            _moduleCatalog = new List<ITweetinviModule>();

            RegisterModules();
            InitializeModules();
            _container = _containerBuilder.Build();
        }

        private void RegisterModules()
        {
            _moduleCatalog.Add(new TweetinviModule(this));
            _moduleCatalog.Add(new TweetinviResourcesModule(this));
            _moduleCatalog.Add(new TweetinviControllersModule(this));
            _moduleCatalog.Add(new TweetinviCoreModule(this));
            _moduleCatalog.Add(new TweetinviCredentialsModule(this));
            _moduleCatalog.Add(new TweetinviFactoriesModule(this));
            _moduleCatalog.Add(new TweetinviLogicModule(this));
            _moduleCatalog.Add(new TweetinviWebLogicModule(this));

            _moduleCatalog.Add(new StreaminviModule(this));
        }

        private void InitializeModules()
        {
            foreach (var module in _moduleCatalog)
            {
                module.Initialize();
            }
        }

        private static ITweetinviContainer GetThreadContainer()
        {
            return new AutofacThreadContainer(_container);
        }

        public void RegisterType<T, U>(RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve) where U : T
        {
            switch (registrationLifetime)
            {
                case RegistrationLifetime.InstancePerResolve:
                    _containerBuilder.RegisterType<U>().As<T>();
                    return;
                case RegistrationLifetime.InstancePerThread:
                    _containerBuilder.RegisterType<U>().As<T>().InstancePerLifetimeScope();
                    return;
                case RegistrationLifetime.InstancePerApplication:
                    _containerBuilder.RegisterType<U>().As<T>().SingleInstance();
                    return;
            }
        }

        public void RegisterGeneric(Type sourceType, Type targetType, RegistrationLifetime registrationLifetime = RegistrationLifetime.InstancePerResolve)
        {
            switch (registrationLifetime)
            {
                case RegistrationLifetime.InstancePerResolve:
                    _containerBuilder.RegisterGeneric(targetType).As(sourceType);
                    return;
                case RegistrationLifetime.InstancePerThread:
                    _containerBuilder.RegisterGeneric(targetType).As(sourceType).InstancePerLifetimeScope();
                    return;
                case RegistrationLifetime.InstancePerApplication:
                    _containerBuilder.RegisterGeneric(targetType).As(sourceType).SingleInstance();
                    return;
            }
        }

        public void RegisterInstance(Type targetType, object value)
        {
            _containerBuilder.RegisterInstance(value).As(targetType).ExternallyOwned();
        }

        public T Resolve<T>(params IConstructorNamedParameter[] parameters)
        {
            return ThreadContainer.Resolve<T>(parameters);
        }
    }
}