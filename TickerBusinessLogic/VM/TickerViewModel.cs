using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;
using System.ComponentModel;
using Ticker.Model;
using Ticker.PriceProvider;

namespace Ticker.VM
{
    public class TickerViewModel : BaseViewModel, IDisposable
    {
        IPriceProvider _priceProvider;

        private Dictionary<string, PriceObservableCollection> _watchlist = new Dictionary<string, PriceObservableCollection>();
        public Dictionary<string, PriceObservableCollection> WatchlistMap
        {
            get { return _watchlist; }
        }

        public TickerViewModel()
        {
            _priceProvider = new FilePriceProvider("Sample Data.txt");
            _priceProvider.PriceUpdate += _priceProvider_PriceUpdate;
        }

        public TickerViewModel(IPriceProvider priceProvider)
        {
            if (priceProvider == null)
            {
                throw new ArgumentNullException("priceProvider");
            }

            _priceProvider = priceProvider;
            _priceProvider.PriceUpdate += _priceProvider_PriceUpdate;
        }

        public override void OnViewLoaded()
        {            
            _priceProvider.Start(100);

            base.OnViewLoaded();
        }

        public override void OnViewUnloaded()
        {
            _priceProvider.Stop();
            _priceProvider.PriceUpdate -= _priceProvider_PriceUpdate;

            base.OnViewUnloaded();
        }

        private void _priceProvider_PriceUpdate(object sender, PriceUpdateEventArgs e)
        {
            lock (WatchlistMap)
            {
                if (WatchlistMap.ContainsKey(e.Symbol) == false)
                {
                    WatchlistMap.Add(e.Symbol, new PriceObservableCollection(10));
                    RaisePropertyChanged("Watchlists");
                }

                WatchlistMap[e.Symbol].Push(e.PriceValue);
            }
        }

        public void Dispose()
        {
            // get rid of managed resources
            if (_priceProvider != null)
            {
                _priceProvider.PriceUpdate -= _priceProvider_PriceUpdate;
                _priceProvider.Dispose();
                _priceProvider = null;
            }
        }
    }
}
