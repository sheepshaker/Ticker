using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Ticker
{
    public class TickerViewModel : IDisposable
    {
        public Dictionary<string, PriceObservableCollection> Model { get; set; }

        private TaskFactory uiFactory; //dispatching
        private FileStream _fs;
        private StreamReader _sr;
        private Timer _timer;

        public TickerViewModel()
        {
            Model = new Dictionary<string, PriceObservableCollection>();
            uiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            
            _timer = new Timer(TimerCallback);

            _timer.Change(0, 1000);

            _fs = new FileStream("Sample Data.txt", FileMode.Open);
            _sr = new StreamReader(_fs, Encoding.Default);
        }

        ~TickerViewModel()
        {
            Dispose(false);
        }

        private void TimerCallback(object status)
        {
            lock (_timer)//one loop at a time
            {
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        var str = ReadOneLine();
                        var dto = new TickerModelDTO(str);

                        //dispatch to UI thread
                        uiFactory.StartNew(() =>
                        {
                            if (Model.ContainsKey(dto.Symbol) == false)
                            {
                                Model.Add(dto.Symbol, new PriceObservableCollection(10));
                            }

                            Model[dto.Symbol].Push(dto.Price);
                        });
                    }
                    catch (Exception ex)
                    {
                        //tbd log
                    }
                }
            }
        }

        private string ReadOneLine()
        {
            if(_sr.EndOfStream)
            {
                _sr.BaseStream.Seek(0, SeekOrigin.Begin);
            }

            return _sr.ReadLine();
        }

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // get rid of managed resources
                _timer.Dispose();
            }

            // get rid of unmanaged resources
            _sr.Dispose();
            _fs.Dispose();
        }
    }
}
