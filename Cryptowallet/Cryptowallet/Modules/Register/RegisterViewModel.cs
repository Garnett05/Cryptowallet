using Cryptowallet.Application;
using Cryptowallet.Common.Base;
using Cryptowallet.Common.Database;
using Cryptowallet.Common.Models;
using Cryptowallet.Common.Navigation;
using Cryptowallet.Common.Password;
using Cryptowallet.Common.Validations;
using Cryptowallet.Modules.Login;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Cryptowallet.Modules.Register
{
    public class RegisterViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IRepository<User> _userRepository;

        public RegisterViewModel(INavigationService navigationService, IRepository<User> userRepository)
        {
            _navigationService = navigationService;
            _userRepository = userRepository;
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
        private ValidatableObject<string> _name;
        public ValidatableObject<string> Name
        {
            get => _name;
            set { SetProperty(ref _name, value); }
        }
        public ICommand LoginCommand { get => new Command(async () => await GoLogin()); }
        public ICommand RegisterUserCommand { get => new Command(async () => await RegisterUser(), () => IsNotBusy); }
                
        private async Task GoLogin()
        {
            await _navigationService.InsertAsRoot<LoginViewModel>();
        }
        private async Task RegisterUser()
        {
            if (!EntriesAreCorrectlyPopulated())
            {
                return;
            }
            IsBusy = true;
            var newUser = new User
            {
                Email = Email.Value,
                FirstName = Name.Value,
                HashedPassword = SecurePasswordHasher.Hash(Password.Value)
            };
            await _userRepository.SaveAsync(newUser);
            Preferences.Set(Constants.IS_USER_LOGGED_IN, true);
            _navigationService.GoToMainFlow();
            IsBusy = false;
        }
        private void AddValidations()
        {
            _email = new ValidatableObject<string>();
            _name = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Email is empty." });
            _email.Validations.Add(new EmailRule<string> { ValidationMessage = "Email is not in a correct format." });

            _name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Name is empty." });

            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Password is empty." });
        }
        private bool EntriesAreCorrectlyPopulated()
        {
            _email.Validate();
            _name.Validate();
            _password.Validate();

            return _email.IsValid && _name.IsValid && _password.IsValid;
        }
    }
}
