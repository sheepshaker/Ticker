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

namespace Ticker.VM
{
    public class TickerViewModel : BaseViewModel, IDisposable
    {
        public WatchlistModel WatchlistModel { get; private set; }

        public TickerViewModel()
        {
            WatchlistModel = new WatchlistModel();
        }

        public TickerViewModel(WatchlistModel watchlistModel)
        {
            if (watchlistModel == null)
            {
                throw new ArgumentNullException("watchlistModel");
            }

            WatchlistModel = watchlistModel;
        }

        public override void OnViewLoaded()
        {
            WatchlistModel.Subscribe();
            RaisePropertyChanged("WatchlistModel");

            base.OnViewLoaded();
        }

        public override void OnViewUnloaded()
        {
            WatchlistModel.Unsubscribe();

            base.OnViewUnloaded();
        }

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
                return;

            // get rid of managed resources
            if (WatchlistModel != null)
            {
                WatchlistModel.Dispose();
            }

            _disposed = true;
        }
    }
}
