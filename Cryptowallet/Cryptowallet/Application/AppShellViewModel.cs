using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cryptowallet.Application
{
    public class AppShellViewModel
    {
        public ICommand SignOutCommand { get => new Command(async () => await SignOut()); }

        private async Task SignOut()
        {
            await Shell.Current.DisplayAlert("Todo", "You have been logged out", "Ok");
        }
    }
}
