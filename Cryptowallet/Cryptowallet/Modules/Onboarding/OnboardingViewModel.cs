using Cryptowallet.Common.Base;
using Cryptowallet.Common.Models;
using Cryptowallet.Common.Navigation;
using Cryptowallet.Modules.Login;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cryptowallet.Modules.Onboarding
{
    class OnboardingViewModel : BaseViewModel
    {
        private INavigationService _navigationService;

        public OnboardingViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public ObservableCollection<OnboardingItem> OnboardingSteps { get; set; } = new ObservableCollection<OnboardingItem>
        {
            new OnboardingItem("welcome.png", "Welcome to Cryptowallet", "Manage all your crypto assets! It's simple and easy!"),
            new OnboardingItem("nice.png", "Nice and Tidy Crypto Portfolio", "Keep BTC, ETH, XRP, and many other tokens."),
            new OnboardingItem("security.png", "Your Safety is Our Top Priority", "Our top-notch security features will keep you completely safe.")
        };

        public ICommand SkipCommand { get => new Command(async () => await Skip()); }

        private async Task Skip()
        {
            await _navigationService.InsertAsRoot<LoginViewModel>();
        }
    }
}
