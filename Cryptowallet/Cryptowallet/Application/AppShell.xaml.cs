using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Cryptowallet.Application
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new AppShellViewModel();
        }
    }
}
