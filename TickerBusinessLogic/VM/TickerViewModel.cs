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

namespace Ticker.VM
{
    public class TickerViewModel : BaseViewModel, IDisposable
    {
        public Dictionary<string, PriceObservableCollection> Model { get; set; }

        private TaskFactory _uiFactory; //dispatching
        private FileStream _fs;
        private StreamReader _sr;
        private Timer _timer;

        public TickerViewModel()
        {
        }

        ~TickerViewModel()
        {
            Dispose(false);
        }

        public override void OnViewLoaded()
        {
            Model = new Dictionary<string, PriceObservableCollection>();
            RaisePropertyChanged("Model");

            _uiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            _timer = new Timer(TimerCallback);

            _fs = new FileStream("Sample Data.txt", FileMode.Open);
            _sr = new StreamReader(_fs, Encoding.Default);
            _timer.Change(0, 1000);

            base.OnViewLoaded();
        }

        public override void OnViewUnloaded()
        {
            _timer.Dispose();
            _timer = null;

            _sr.Dispose();
            _sr = null;

            _fs.Dispose();
            _fs = null;
            
            base.OnViewUnloaded();
        }

        protected virtual void TimerCallback(object status)
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
                        _uiFactory.StartNew(() =>
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
                if (_timer != null)
                {
                    _timer.Dispose();
                }
            }

            // get rid of unmanaged resources
            //make sure the file is not left locked
            if (_sr != null)
            {
                _sr.Dispose();
            }

            if (_fs != null)
            {
                _fs.Dispose();
            }
        }
    }
}
