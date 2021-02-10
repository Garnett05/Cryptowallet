using Cryptowallet.Common.Base;
using Cryptowallet.Common.Controllers;
using Cryptowallet.Common.Models;
using Cryptowallet.Common.Navigation;
using Cryptowallet.Modules.AddTransaction;
using Cryptowallet.Modules.Wallet;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cryptowallet.Modules.Transactions
{
    public class TransactionsViewModel : BaseViewModel
    {
        private IWalletController _walletController;
        private INavigationService _navigationService;
        private Transaction _selectedTransaction;
        private ObservableCollection<Transaction> _transactions;
        private bool _isRefreshing;
        private string _filter = string.Empty;
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set { SetProperty(ref _transactions, value); }
        }
        public Transaction SelectedTransaction {
            get => _selectedTransaction;
            set { SetProperty(ref _selectedTransaction, value); } 
        }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { SetProperty(ref _isRefreshing, value); }
        }
        public ICommand RefreshTransactionsCommand { get => new Command(async () => await RefreshTransactions()); }

        private async Task RefreshTransactions()
        {
            await GetTransactions();
        }
        public ICommand TransactionSelectedCommand { get => new Command(async () => await TransactionSelected()); }

        private async Task TransactionSelected()
        {
            await _navigationService.PushAsync<AddTransactionViewModel>("id=1");
        }
        public ICommand TradeCommand { get => new Command(async () => await PerformNavigation()); }

        private async Task PerformNavigation()
        {
            await _navigationService.PushAsync<AddTransactionViewModel>();
        }

        public override async Task InitializeAsync(object parameter)
        {
            _filter = parameter.ToString();
            await GetTransactions();
        }

        private async Task GetTransactions()
        {
            IsRefreshing = true;
            var transactions = await _walletController.GetTransactions();
            if (!string.IsNullOrEmpty(_filter))
            {
                transactions = transactions.Where(x => x.Status == _filter).ToList();
            }
            Transactions = new ObservableCollection<Transaction>(transactions);
            IsRefreshing = false;
        }

        public TransactionsViewModel(IWalletController walletController, INavigationService navigationService)
        {
            _walletController = walletController;
            _navigationService = navigationService;
            Transactions = new ObservableCollection<Transaction>();
        }
    }
}
