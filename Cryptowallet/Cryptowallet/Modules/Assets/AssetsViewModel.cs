using Cryptowallet.Common.Base;
using Cryptowallet.Common.Controllers;
using Cryptowallet.Common.Models;
using Cryptowallet.Common.Navigation;
using Cryptowallet.Modules.AddTransaction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cryptowallet.Modules.Assets
{
    public class AssetsViewModel : BaseViewModel
    {
        private IWalletController _walletController;
        private INavigationService _navigationService;

        public AssetsViewModel(IWalletController walletController, INavigationService navigationService)
        {
            _walletController = walletController;
            _navigationService = navigationService;
            Assets = new ObservableCollection<Coin>();
        }

        public async override Task InitializeAsync(object parameter)
        {
            var assets = await _walletController.GetCoins();
            Assets = new ObservableCollection<Coin>(assets);
        }
        private ObservableCollection<Coin> _assets;
        public ObservableCollection<Coin> Assets
        {
            get => _assets;
            set { SetProperty(ref _assets, value); }
        }
        public ICommand AddTransactionCommand { get => new Command(async () => await AddTransaction()); }

        private async Task AddTransaction()
        {
            await _navigationService.PushAsync<AddTransactionViewModel>();
        }
    }
}
