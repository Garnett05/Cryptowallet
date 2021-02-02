using Autofac;
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
    public partial class TransactionsView : ContentPage
    {
        public TransactionsView()
        {
            InitializeComponent();
            BindingContext = Application.App.Container.Resolve<TransactionsViewModel>();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as TransactionsViewModel).InitializeAsync("");
        }
    }
}