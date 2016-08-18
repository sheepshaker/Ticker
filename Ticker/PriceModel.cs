using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ticker
{
    public class PriceModel : ObservableMaxStack<Price>
    {
        private const int AverageCount = 5;
        private const int MaxPrices = 10;

        public Price CurrentPrice
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
                return PeekMultiple(AverageCount).Select(p => p.Value).Average();
            }
        }

        public IEnumerable<Price> PriceHistory
        {
            get
            {
                return PeekMultiple(MaxPrices);
            }
        }

        public PriceModel(Price currentPrice) : base(MaxPrices)
        {
            Push(currentPrice);
        }

        public override void Push(Price value)
        {
            base.Push(value);

            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("CurrentPrice"));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("AveragePrice"));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("PriceHistory"));
        }
    }

    public enum PriceChange
    {
        Constant,
        Increasing,
        Decreasing
    }

    public class Price
    {
        public decimal Value { get; set; }
        public PriceChange Change { get; set; }
    }
}
