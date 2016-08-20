using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ticker.Model;

namespace Ticker.PriceProvider
{
    public interface IPriceProvider: IDisposable
    {
        event EventHandler<PriceUpdateEventArgs> PriceUpdate;
        void Start(int delay);
        void Stop();
    }

    public class PriceUpdateEventArgs : EventArgs
    {
        public string Symbol { get; set; }
        public decimal PriceValue { get; set; }
    }

    public class PriceProvider : IPriceProvider
    {
        public event EventHandler<PriceUpdateEventArgs> PriceUpdate;

        protected void RaisePriceUpdate(string symbol, decimal priceValue)
        {
            var temp = PriceUpdate;
            if (temp != null)
            {
                temp(this, new PriceUpdateEventArgs { Symbol = symbol, PriceValue = priceValue });
            }
        }

        public virtual void Start(int delay)
        {

        }

        public virtual void Stop()
        {

        }

        public virtual void Dispose()
        {

        }
    }
}
