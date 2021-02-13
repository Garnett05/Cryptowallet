using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptowallet.Common.Validations
{
    public class NonNegativeRule : IValidationRule<decimal>
    {
        public string ValidationMessage { get; set; }

        public bool Check(decimal value)
        {
            return value > 0;
        }
    }
}
