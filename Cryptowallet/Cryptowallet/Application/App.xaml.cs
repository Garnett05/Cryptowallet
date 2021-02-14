using Autofac;
using Cryptowallet.Common.Database;
using Cryptowallet.Common.Models;
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
