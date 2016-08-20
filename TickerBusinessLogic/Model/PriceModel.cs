using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Ticker.Model
{
    public class PriceModel
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
            var priceChange = PriceChange.Constant;

            if(currentPrice != null)
            {
                if(newPrice > currentPrice.Value)
                {
                    priceChange = PriceChange.Increasing;
                }
                else if(newPrice < currentPrice.Value)
                {
                    priceChange = PriceChange.Decreasing;
                }
                else
                {
                    priceChange = PriceChange.Constant;
                }
            }

            Change = priceChange;
            Value = newPrice;
        }
    }
}
