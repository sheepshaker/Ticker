using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using Ticker.Model;
using Ticker.PriceProvider;

namespace Ticker.VM
{
    public class TickerViewModel : BaseViewModel, IDisposable
    {
        private TaskFactory _uiFactory = new TaskFactory(); //dispatching
        IPriceProvider _priceProvider;

        private Dictionary<string, PriceObservableCollection> _watchlist = new Dictionary<string, PriceObservableCollection>();
        public Dictionary<string, PriceObservableCollection> Watchlist
        {
            get { return _watchlist; }
            private set
            {
                if (_watchlist != value)
                {
                    _watchlist = value;
                    RaisePropertyChanged("Watchlist");
                }
            }
        }

        public TickerViewModel()
        {
            _priceProvider = new FilePriceProvider("Sample Data.txt");
        }

        public TickerViewModel(IPriceProvider priceProvider)
        {
            if (priceProvider == null)
            {
                throw new ArgumentNullException("priceProvider");
            }

            _priceProvider = priceProvider;
        }

        public override void OnViewLoaded()
        {
            _priceProvider.PriceUpdate += _priceProvider_PriceUpdate;
            _priceProvider.Start(1000);

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
            //dispatch to UI thread
            _uiFactory.StartNew(() =>
            {
                lock (Watchlist)
                {
                    if (Watchlist.ContainsKey(e.Symbol) == false)
                    {
                        Watchlist.Add(e.Symbol, new PriceObservableCollection(10));
                    }

                    Watchlist[e.Symbol].Push(e.PriceValue);
                }
            });
        }

        public void Dispose()
        {
            // get rid of managed resources
            if (_priceProvider != null)
            {
                _priceProvider.Dispose();
            }
        }
    }
}
