using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ticker
{
    class PriceHistoryViewModel
    {
        public string Symbol { get; private set; }
        public PriceModel Prices { get; private set; }

        public PriceHistoryViewModel(string symbol, PriceModel prices)
        {
            Symbol = symbol;
            Prices = prices;
        }
    }
}
