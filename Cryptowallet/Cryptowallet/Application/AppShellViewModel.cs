using Cryptowallet.Common.Navigation;
using Cryptowallet.Modules.Login;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Cryptowallet.Application
{
    public class AppShellViewModel
    {
        private INavigationService _navigationService;

        public AppShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public ICommand SignOutCommand { get => new Command(async () => await SignOut()); }

        private async Task SignOut()
        {
            Preferences.Remove(Constants.IS_USER_LOGGED_IN);
            _navigationService.GoToLoginFlow();
            await _navigationService.InsertAsRoot<LoginViewModel>();
        }
    }
}
