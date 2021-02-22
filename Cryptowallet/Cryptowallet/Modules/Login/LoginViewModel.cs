using Cryptowallet.Application;
using Cryptowallet.Common.Base;
using Cryptowallet.Common.Database;
using Cryptowallet.Common.Dialog;
using Cryptowallet.Common.Models;
using Cryptowallet.Common.Navigation;
using Cryptowallet.Common.Password;
using Cryptowallet.Common.Validations;
using Cryptowallet.Modules.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Cryptowallet.Modules.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IRepository<User> _userRepository;
        private IDialogMessage _dialogMessage;

        public LoginViewModel(INavigationService navigationService, IRepository<User> userRepository, IDialogMessage dialogMessage)
        {
            _navigationService = navigationService;
            _userRepository = userRepository;
            _dialogMessage = dialogMessage;
            AddValidations();
        }

        private ValidatableObject<string> _email;
        public ValidatableObject<string> Email
        {
            get => _email;
            set { SetProperty(ref _email, value); }
        }
        private ValidatableObject<string> _password;
        public ValidatableObject<string> Password
        {
            get => _password;
            set { SetProperty(ref _password, value); }
        }

        public ICommand RegisterCommand { get => new Command(async () => await GoToRegister()); }
        public ICommand LoginCommand { get => new Command(async () => await LoginUser(), () => IsNotBusy); }

        
        private async Task GoToRegister()
        {
            await _navigationService.InsertAsRoot<RegisterViewModel>();
        }
        private async Task LoginUser()
        {
            if (!EntriesCorrectlyPopulated())
            {
                return;
            }
            IsBusy = true;
            var user = (await _userRepository.GetAllAsync()).FirstOrDefault(x => x.Email == Email.Value);
            if (user == null)
            {
                await DisplayCredentialsError();
                IsBusy = false;
                return;
            }
            if (!SecurePasswordHasher.Verify(Password.Value, user.HashedPassword))
            {
                await DisplayCredentialsError();
                IsBusy = false;
                return;
            }
            Preferences.Set(Constants.IS_USER_LOGGED_IN, true);
            _navigationService.GoToMainFlow();
            IsBusy = false;
        }

        private async Task DisplayCredentialsError()
        {
            await _dialogMessage.DisplayAlert("Error", "Credentials are wrong.", "Ok");
            Password.Value = "";
        }
        private void AddValidations()
        {
            _email = new ValidatableObject<string>();            
            _password = new ValidatableObject<string>();

            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email is empty." });
            _email.Validations.Add(new EmailRule<string> { ValidationMessage = "Email is not in a correct format." });

            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password is empty." });
        }
        private bool EntriesCorrectlyPopulated()
        {
            _email.Validate();
            _password.Validate();

            return _email.IsValid && _password.IsValid;
        }
    }
}
