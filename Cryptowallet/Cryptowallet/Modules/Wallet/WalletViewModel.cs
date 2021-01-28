using Cryptowallet.Common.Base;
using Cryptowallet.Common.Controllers;
using Cryptowallet.Common.Models;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptowallet.Modules.Wallet
{
    public class WalletViewModel : BaseViewModel
    {
        private IWalletController _walletController;
        private Chart _portfolioView;
        private int _coinsHeight;
        private int _transactionsHeight;
        private decimal _portfolioValue;
        private bool _isRefreshing;
        private bool _hasTransaction;
        private ObservableCollection<Coin> _assets;
        private ObservableCollection<Transaction> _latestTransaction;
        public ObservableCollection<Transaction> LatestTransaction
        {
            get => _latestTransaction;
            set { SetProperty(ref _latestTransaction, value);
                if (_latestTransaction == null)
                {
                    return;
                }
                HasTransaction = _latestTransaction.Count > 0;
                if (_latestTransaction.Count == 0)
                {
                    TransactionsHeight = 430;
                    return;
                }
                TransactionsHeight = _latestTransaction.Count * 85;
            }

        }
        public Chart PortfolioView
        {
            get => _portfolioView;
            set { SetProperty(ref _portfolioView, value); }
        }
        public int CoinsHeight
        {
            get => _coinsHeight;
            set { SetProperty(ref _coinsHeight, value); }
        }
        public ObservableCollection<Coin> Assets
        {
            get => _assets;
            set
            {
                SetProperty(ref _assets, value);
                if (_assets == null)
                {
                    return;
                }
                CoinsHeight = _assets.Count * 85;
            }
        }
        public int TransactionsHeight
        {
            get => _transactionsHeight;
            set
            {
                SetProperty(ref _transactionsHeight, value);
            }
        }
        public decimal PortfolioValue
        {
            get => _portfolioValue;
            set { SetProperty(ref _portfolioValue, value); }
        }
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { { SetProperty(ref _isRefreshing, value); } }
        }
        public bool HasTransaction
        {
            get => _hasTransaction;
            set { SetProperty(ref _hasTransaction, value); }
        }
        public override async Task InitializeAsync(object parameter)
        {
            bool reload = (bool)parameter;

            var transactions = await _walletController.GetTransactions();
            LatestTransaction = new ObservableCollection<Transaction>(transactions.Take(3));            

            var assets = await _walletController.GetCoins();
            Assets = new ObservableCollection<Coin>(assets.Take(3));
            BuildChart(assets);
            PortfolioValue = assets.Sum(x => x.DollarValue);

            IsRefreshing = false;
            IsBusy = false;
        }
        public WalletViewModel(IWalletController walletController)
        {
            _walletController = walletController;
            Assets = new ObservableCollection<Coin>();
            LatestTransaction = new ObservableCollection<Transaction>();
        }

        private void BuildChart(List<Coin> assets)
        {
            var whiteColor = SKColor.Parse("#FFFFFF");
            var colors = Coin.GetAvailableAssets();
            List<ChartEntry> entries = new List<ChartEntry>();
            foreach (var i in assets)
            {
                entries.Add(new ChartEntry((float)i.DollarValue)
                {
                    TextColor = whiteColor,
                    ValueLabel = i.Name,
                    Color = SKColor.Parse(colors.First(x => x.Symbol == i.Symbol).HexColor)
                });
            }
            var chart = new DonutChart { Entries = entries };
            chart.BackgroundColor = whiteColor;
            PortfolioView = chart;
        }
    }
}
