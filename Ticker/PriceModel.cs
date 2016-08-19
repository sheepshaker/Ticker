using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

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

    public class NewPriceModel : NotifyPropertyChangedBase
    {
        private Price _currentPrice;
        private Price _averagePrice;

        public Price CurrentPrice
        {
            get { return _currentPrice; }
            set
            {
                if (_currentPrice != value)
                {
                    _currentPrice = value;
                    RaisePropertyChanged("CurrentPrice");
                }
            }
        }

        public Price AveragePrice
        {
            get { return _averagePrice; }
            set
            {
                if (_averagePrice != value)
                {
                    _averagePrice = value;
                    RaisePropertyChanged("AveragePrice");
                }
            }
        }
    }

    public class PriceObservableCollection : NotifyPropertyChangedBase
    {
        int _limit;
        LinkedList<NewPriceModel> _prices = new LinkedList<NewPriceModel>();

        public PriceObservableCollection(int limit)
        {
            _limit = limit;
        }

        public void Push(decimal newPriceValue)
        {
            if (_prices.Count == _limit)
            {
                _prices.RemoveLast();
            }

            Price currentPrice = GetCurrentPrice();
            Price currentAveragePrice = GetCurrentAveragePrice();

            Price newCurrentPrice = new Price(newPriceValue, currentPrice);

            decimal averagePrice = CalculateNewAveragePrice(newPriceValue, 4);

            Price newAveragePrice = new Price(averagePrice, currentAveragePrice);

            NewPriceModel model = new NewPriceModel()
            {
                CurrentPrice = newCurrentPrice,
                AveragePrice = newAveragePrice
            };

            _prices.AddFirst(model);
            RaisePropertyChanged("CurrentPrice");
            RaisePropertyChanged("AveragePrice");
        }

        private Price GetCurrentPrice()
        {
            Price currentPrice = null;

            if(_prices.Any())
            {
                currentPrice = _prices.First().CurrentPrice;
            }

            return currentPrice;
        }

        private Price GetCurrentAveragePrice()
        {
            Price currentAveragePrice = null;

            if (_prices.Any())
            {
                currentAveragePrice = _prices.First().AveragePrice;
            }

            return currentAveragePrice;
        }

        private decimal CalculateNewAveragePrice(decimal newPrice, int averageCount)
        {
            var prices = _prices.Take(averageCount).Select(p => p.CurrentPrice.Value).ToList();
            prices.Add(newPrice);

            return prices.Average();
        }

        public Price CurrentPrice
        {
            get
            {
                return GetCurrentPrice();
            }
        }

        public Price AveragePrice
        {
            get
            {
                return GetCurrentAveragePrice();
            }
        }

        
    }

    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
