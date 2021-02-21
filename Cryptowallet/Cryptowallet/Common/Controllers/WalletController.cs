using Cryptowallet.Application;
using Cryptowallet.Common.Database;
using Cryptowallet.Common.Models;
using Cryptowallet.Common.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptowallet.Common.Controllers
{
    public interface IWalletController
    {
        Task<List<Coin>> GetCoins(bool forceReload = false);
        Task<List<Transaction>> GetTransactions(bool forceReload = false);
    }
    class WalletController : IWalletController
    {
        private IRepository<Transaction> _transactionRepository;
        private ICryptoService _cryptoService;
        private List<Coin> _cachedCoins = new List<Coin>();
        private List<Coin> _defaultAssets = new List<Coin>
        {
            new Coin
            {
                Name = "Bitcoin",
                Amount = 0,
                Symbol = "BTC",
                DollarValue = 0
            },
            new Coin
            {
                Name = "Ethereum",
                Amount = 0,
                Symbol = "ETH",
                DollarValue = 0
            },
            new Coin
            {
                Name = "Litecoin",
                Amount = 0,
                Symbol = "LTC",
                DollarValue = 0
            }
        };

        public WalletController(IRepository<Transaction> transactionRepository, CryptoService cryptoService)
        {
            _transactionRepository = transactionRepository;
            _cryptoService = cryptoService;
        }

        public async Task<List<Coin>> GetCoins(bool forceReload = false)
        {
            List<Transaction> transactions = await LoadTransactions(forceReload);
            
            if(transactions.Count == 0 || _cachedCoins.Count == 0)
            {
                return _defaultAssets;
            }
            var groupedTransactions = transactions.GroupBy(x => x.Symbol);
            var result = new List<Coin>();
            foreach (var item in groupedTransactions)
            {
                var amount = item.Where(x => x.Status == Constants.TRANSACTION_DEPOSITED).Sum(x => x.Amount) 
                    - item.Where(x => x.Status == Constants.TRANSACTION_WITHDRAWN).Sum(x => x.Amount);
                var newCoin = new Coin
                {
                    Symbol = item.Key,
                    Amount = amount,
                    DollarValue = amount * (decimal)_cachedCoins.FirstOrDefault(y => y.Symbol == item.Key).Price,
                    Name = Coin.GetAvailableAssets().First(a => a.Symbol == item.Key).Name
                };
                result.Add(newCoin);
            }
            return result.OrderByDescending(x => x.DollarValue).ToList();
        }

        public async Task<List<Transaction>> GetTransactions(bool forceReload = false)
        {
            List<Transaction> transactions = await LoadTransactions(forceReload);
            if (transactions.Count == 0 || _cachedCoins.Count == 0){
                return transactions;
            }
            transactions.ForEach(x =>
            {
                x.StatusImageSource = x.Status == Constants.TRANSACTION_DEPOSITED ?
                                                Constants.TRANSACTION_DEPOSITED_IMAGE :
                                                Constants.TRANSACTION_WITHDRAWN_IMAGE;
                x.DollarValue = x.Amount * (decimal)_cachedCoins.First(z => z.Symbol == x.Symbol).Price;
            });
            return transactions;
        }
        private async Task<List<Transaction>> LoadTransactions(bool forceReload)
        {
            if (_cachedCoins.Count == 0 || forceReload)
            {
                _cachedCoins = await _cryptoService.GetLatestPrices();
            }
            var transactions = await _transactionRepository.GetAllAsync();
            transactions = transactions.OrderByDescending(x => x.TransactionDate).ToList();
            return transactions;
        }
    }
}
