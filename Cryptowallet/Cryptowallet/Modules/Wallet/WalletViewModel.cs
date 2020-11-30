﻿using Cryptowallet.Common.Base;
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
        private ObservableCollection<Coin> _assets;
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
        public override async Task InitializeAsync(object parameter)
        {
            var assets = await _walletController.GetCoins();
            Assets = new ObservableCollection<Coin>(assets.Take(3));
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
