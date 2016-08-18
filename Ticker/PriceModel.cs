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

        public PriceModel(decimal currentPrice) : base(currentPrice, MaxPrices)
        {

        }

        public override void Push(decimal value)
        {
            base.Push(value);

            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("CurrentPrice"));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("AveragePrice"));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("AllPrices"));
        }
    }
}
