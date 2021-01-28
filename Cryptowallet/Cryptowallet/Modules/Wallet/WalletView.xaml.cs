using Autofac;
using Cryptowallet.Application;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptowallet.Modules.Wallet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WalletView : ContentPage
    {
        public WalletView()
        {
            InitializeComponent();
                        
            BindingContext = App.Container.Resolve<WalletViewModel>();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as WalletViewModel).InitializeAsync(false);
        }
    }
}