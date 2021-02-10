using Autofac;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Cryptowallet.Application
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<AppShellViewModel>();

            Routing.RegisterRoute("AddTransactionViewModel", typeof(Modules.AddTransaction.AddTransactionView));
        }
    }
}
