using Cryptowallet.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Cryptowallet.Modules.AddTransaction
{
    [QueryProperty("Id", "id")]
    public class AddTransactionViewModel : BaseViewModel
    {
        private string _id;
        public string Id
        {
            set
            {
                _id = Uri.UnescapeDataString(value);
            }
        }
    }
}
