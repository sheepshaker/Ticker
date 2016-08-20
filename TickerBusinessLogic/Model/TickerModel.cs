using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Ticker.Model
{
    public class TickerModel
    {
        public Price CurrentPrice { get; set; }
        public Price AveragePrice { get; set; }
    }

    public enum PriceChange
    {
        Constant,
        Increasing,
        Decreasing
    }

    public class Price
    {
        public decimal Value { get; private set; }
        public PriceChange Change { get; private set; }

        public Price(decimal newPrice, Price currentPrice)
        {
            if(currentPrice != null)
            {
                if(newPrice > currentPrice.Value)
                {
                    Change = PriceChange.Increasing;
                }
                else
                {
                    Change = PriceChange.Decreasing;
                }
            }

            Value = newPrice;
        }
    }
}
