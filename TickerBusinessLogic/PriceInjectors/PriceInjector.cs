using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ticker.Model;

namespace Ticker.PriceInjectors
{
    public class PriceInjector
    {
        private IPriceInject _priceInject;

        public PriceInjector(IPriceInject priceInject)
        {
            _priceInject = priceInject;
        }
    }
}
