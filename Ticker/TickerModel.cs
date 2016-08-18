using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ticker
{
    class TickerModel
    {
        public string Symbol { get; }
        public List<decimal> Prices { get; }
    }
}
