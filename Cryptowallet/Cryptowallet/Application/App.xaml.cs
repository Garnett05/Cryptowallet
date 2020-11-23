using Autofac;
using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

            //get container
            Container = builder.Build();
            MainPage = new AppShell();
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
