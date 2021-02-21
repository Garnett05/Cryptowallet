﻿using Cryptowallet.Application;
using Cryptowallet.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cryptowallet.Common.Navigation
{
    public interface INavigationService
    {
        Task PushAsync<TViewModel>(string parameters = null) where TViewModel : BaseViewModel;
        Task PopAsync();
        Task GoBackAsync();
        Task InsertAsRoot<TViewModel>(string parameters = null) where TViewModel : BaseViewModel;
        void GoToMainFlow();
        void GoToLoginFlow();
    }
    public class ShellRoutingService : INavigationService
    {
        public Task PopAsync()
        {
            return Shell.Current.Navigation.PopAsync();
        }
        public Task GoBackAsync()
        {
            return Shell.Current.GoToAsync("..");
        }
        public Task InsertAsRoot<TViewModel>(string parameters = null) where TViewModel : BaseViewModel
        {
            return GoToAsync<TViewModel>("//", parameters);
        }
        public Task PushAsync<TViewModel>(string parameters = null) where TViewModel : BaseViewModel
        {
            return GoToAsync<TViewModel>("", parameters);
        }

        private Task GoToAsync<TViewModel>(string routePrefix, string parameters) where TViewModel : BaseViewModel
        {
            var route = routePrefix + typeof(TViewModel).Name;
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                route += $"?{parameters}";
            }
            return Shell.Current.GoToAsync(route);
        }

        public void GoToMainFlow()
        {            
            App.Current.MainPage = new AppShell();
        }

        public void GoToLoginFlow()
        {            
            App.Current.MainPage = new LoginShell();
        }
    }
}
