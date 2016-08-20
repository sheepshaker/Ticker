using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Ticker.Model
{
    //public class Watchlist : NotificationObject
    //{
    //    private IEnumerable<Price> _history;
    //    public string Symbol { get; set; }
    //    public Price CurrentPrice { get; set; }
    //    public Price AveragePrice { get; set; }
    //    public IEnumerable<Price> HistoricalPrices
    //    {
    //        get { return _history; }
    //        set
    //        {
    //            _history = value;
    //            RaisePropertyChanged("HistoricalPrices");
    //        }
    //    }

    //    public Watchlist(string symbol, Price currentPrice, Price averagePrice, IEnumerable<Price> historicalPrices)
    //    {
    //        Symbol = symbol;
    //        CurrentPrice = currentPrice;
    //        AveragePrice = averagePrice;
    //        HistoricalPrices = historicalPrices;           
    //    }
    //}

    public class Watchlist
    {
        public string Symbol { get; set; }
        public PriceObservableCollection Prices { get; set; }
    }

    public interface IPriceInject
    {
        void PushPrice(string symbol, decimal price);
    }

    public class WatchlistModel : NotificationObject, IPriceInject
    {
        public Dictionary<string, PriceObservableCollection> TickerMap { get; set; }
        private TaskFactory _uiFactory; //dispatching
        //private FileStream _fs;
        //private StreamReader _sr;
        //private Timer _timer;

        public WatchlistModel()
        {
            //TickerMap = new Dictionary<string, PriceObservableCollection>();
            //var s1 = new PriceObservableCollection(10);
            //s1.Push(1);
            //s1.Push(2);
            //s1.Push(3);
            //TickerMap.Add("S1", s1);

            //var s2 = new PriceObservableCollection(10);
            //s2.Push(1);
            //s2.Push(2);
            //s2.Push(3);
            //TickerMap.Add("S2", s2);
            TickerMap = new Dictionary<string, PriceObservableCollection>();
            _uiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
        }

        //public void Subscribe()
        //{
        //    TickerMap = new Dictionary<string, PriceObservableCollection>();

        //    _uiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
        //    _timer = new Timer(TimerCallback);

        //    //_fs = new FileStream("Sample Data.txt", FileMode.Open);
        //    //_sr = new StreamReader(_fs, Encoding.Default);
        //    _timer.Change(0, 1000);

        //    RaisePropertyChanged("Watchlist");
        //}

        public void PushPrice(string symbol, decimal price)
        {
            _uiFactory.StartNew(() =>
            {
                if (TickerMap.ContainsKey(symbol) == false)
                {
                    TickerMap.Add(symbol, new PriceObservableCollection(10));
                }

                TickerMap[symbol].Push(price);
            });
        }

        public IEnumerable<Watchlist> Watchlist
        {
            get
            {
                if (TickerMap != null)
                    return TickerMap.Select(w => new Watchlist { Symbol = w.Key, Prices = w.Value });
                else
                    return null;
            }
        }

        //protected virtual void TimerCallback(object status)
        //{
        //    lock (_timer)//one tick at a time
        //    {
        //        for (int i = 0; i < 5; i++)
        //        {
        //            try
        //            {
        //                var str = ReadOneLine();
        //                var dto = new TickerModelDTO(str);

        //                //dispatch to UI thread
        //                _uiFactory.StartNew(() =>
        //                {
        //                    if (TickerMap.ContainsKey(dto.Symbol) == false)
        //                    {
        //                        TickerMap.Add(dto.Symbol, new PriceObservableCollection(10));
        //                    }

        //                    TickerMap[dto.Symbol].Push(dto.Price);
        //                });
        //            }
        //            catch (Exception ex)
        //            {
        //                //tbd log
        //            }
        //        }

        //        RaisePropertyChanged("Watchlist");
        //    }
        //}

        //Random r = new Random();

        //private string ReadOneLine()
        //{
        //    //if (_sr.EndOfStream)
        //    //{
        //    //    _sr.BaseStream.Seek(0, SeekOrigin.Begin);
        //    //}

        //    //return _sr.ReadLine();
        //    return "S" + r.Next(1, 6) + ":" + r.Next(1, 100) + "." + r.Next(1, 100);
        //}

        //public void Unsubscribe()
        //{
        //    if (_timer != null)
        //    {
        //        _timer.Dispose();
        //        _timer = null;
        //    }

        //    if (_sr != null)
        //    {
        //        _sr.Dispose();
        //        _sr = null;
        //    }

        //    if (_fs != null)
        //    {
        //        _fs.Dispose();
        //        _fs = null;
        //    }
        //}
    }
}
