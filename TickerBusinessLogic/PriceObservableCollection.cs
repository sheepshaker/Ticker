using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Ticker
{
    public class PriceObservableCollection : NotificationObject
    {
        int _limit;
        LinkedList<TickerModel> _prices = new LinkedList<TickerModel>();

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

            TickerModel model = new TickerModel()
            {
                CurrentPrice = newCurrentPrice,
                AveragePrice = newAveragePrice
            };

            _prices.AddFirst(model);
            RaisePropertyChanged("CurrentPrice");
            RaisePropertyChanged("AveragePrice");
            RaisePropertyChanged("HistoricalPrices");
        }

        private Price GetCurrentPrice()
        {
            Price currentPrice = null;

            if (_prices.Any())
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

        public IEnumerable<Price> HistoricalPrices
        {
            get
            {
                return _prices.Select(p => p.CurrentPrice);
            }
        }
    }
}
