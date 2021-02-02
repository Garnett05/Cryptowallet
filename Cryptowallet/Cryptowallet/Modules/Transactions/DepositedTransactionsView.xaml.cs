using Autofac;
using Cryptowallet.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptowallet.Modules.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DepositedTransactionsView : ContentPage
    {
        public DepositedTransactionsView()
        {
            InitializeComponent();
            BindingContext = Application.App.Container.Resolve<TransactionsViewModel>();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as TransactionsViewModel).InitializeAsync(Constants.TRANSACTION_DEPOSITED);
        }
    }
}