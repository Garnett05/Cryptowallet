using Cryptowallet.Common.Base;
using Cryptowallet.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Cryptowallet.Modules.AddTransaction
{
    [QueryProperty("Id", "id")]
    public class AddTransactionViewModel : BaseViewModel
    {
        private bool _isDeposit;
        private string _id;
        private ObservableCollection<Coin> _availableAssets;
        private Coin _selectedCoin;
        private DateTime _transactionDate;

        public AddTransactionViewModel()
        {
            AvailableAssets = new ObservableCollection<Coin>(Coin.GetAvailableAssets());
        }

        public bool IsDeposit
        {
            get => _isDeposit;
            set { SetProperty(ref _isDeposit, value); }
        }
        public string Id
        {
            set
            {
                _id = Uri.UnescapeDataString(value);
            }
        }
        public ObservableCollection<Coin> AvailableAssets
        {
            get => _availableAssets;
            set { SetProperty(ref _availableAssets, value); }
        }
        public Coin SelectedCoin
        {
            get => _selectedCoin;
            set { SetProperty(ref _selectedCoin, value); }
        }
        public DateTime TransactionDate
        {
            get => _transactionDate;
            set { SetProperty(ref _transactionDate, value); }
        }
        public ICommand AddTransactionCommand { get => new Command(async () => await AddTransaction()); }

        private async Task AddTransaction()
        {
            //throw new NotImplementedException();
        }
    }
}
