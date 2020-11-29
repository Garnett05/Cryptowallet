using Cryptowallet.Common.Base;
using Cryptowallet.Common.Controllers;
using Cryptowallet.Common.Models;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptowallet.Modules.Wallet
{
    public class WalletViewModel : BaseViewModel
    {
        private IWalletController _walletController;
        private Chart _portfolioView;
        public Chart PortfolioView
        {
            get => _portfolioView;
            set { SetProperty(ref _portfolioView, value); }
        }
        public override async Task InitializeAsync(object parameter)
        {
            var assets = await _walletController.GetCoins();
            BuildChart(assets);
        }
        public WalletViewModel(IWalletController walletController)
        {
            _walletController = walletController;
        }

        private void BuildChart(List<Coin> assets)
        {
            var whiteColor = SKColor.Parse("#FFFFFF");
            var colors = Coin.GetAvailableAssets();
            List<ChartEntry> entries = new List<ChartEntry>();
            foreach(var i in assets)
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
