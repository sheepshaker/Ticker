using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ticker
{
    public class PriceModel : ObservableMaxStack<decimal>
    {
        private const int AverageCount = 5;
        private const int MaxPrices = 10;

        public decimal CurrentPrice
        {
            get
            {
                return Top;
            }
        }

        public decimal AveragePrice
        {
            get
            {
                return PeekMultiple(AverageCount).Average();
            }
        }

        public IEnumerable<decimal> AllPrices
        {
            get
            {
                return PeekMultiple(MaxPrices);
            }
        }

        public PriceChange PriceChange
        {
            get
            {
                PriceChange priceChange = PriceChange.Constant;

                if (Count > 1)
                {
                    var prices = PeekMultiple(2).ToArray();
                    var curPrice = prices[0];
                    var prevPrice = prices[1];

                    if (curPrice > prevPrice)
                    {
                        priceChange = PriceChange.Increasing;
                    }
                    else
                    {
                        priceChange = PriceChange.Decreasing;
                    }
                }

                return priceChange;
            }            
        }

        public PriceModel(decimal currentPrice) : base(currentPrice, MaxPrices)
        {

        }

        public override void Push(decimal value)
        {
            base.Push(value);

            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("CurrentPrice"));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("AveragePrice"));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("AllPrices"));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("PriceChange"));
        }
    }

    public enum PriceChange
    {
        Increasing,
        Decreasing,
        Constant
    }
}
