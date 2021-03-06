﻿using Autofac;
using Cryptowallet.Common.Database;
using Cryptowallet.Common.Models;
using Cryptowallet.Modules.Loading;
using System.Reflection;

namespace Cryptowallet.Application
{
    public partial class App : Xamarin.Forms.Application
    {
        public static IContainer Container;
        public App()
        {
            InitializeComponent();
            //class used for registration
            var builder = new ContainerBuilder();
            //scan and register all classes in the assembly
            var dataAccess = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(dataAccess)
                .AsImplementedInterfaces()
                .AsSelf();
            //TODO - Register repositories if you use them
            builder.RegisterType<Repository<Transaction>>().As<IRepository<Transaction>>();
            builder.RegisterType<Repository<User>>().As<IRepository<User>>();
            //get container
            Container = builder.Build();
            //set first page
            MainPage = Container.Resolve<LoadingView>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
