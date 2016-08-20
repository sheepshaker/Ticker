using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ticker.VM;
using Ticker.PriceProvider;

namespace Test
{
    class TestPriceProvider : PriceProviderBase
    {
        public void Push(string symbol, decimal priceValue)
        {
            RaisePriceUpdate(symbol, priceValue);
        }
    }
}
