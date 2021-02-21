using Autofac;
using Cryptowallet.Application;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptowallet.Modules.Assets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssetsView : ContentPage
    {
        public AssetsView()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<AssetsViewModel>();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await(BindingContext as AssetsViewModel).InitializeAsync(null);
        }
    }
}