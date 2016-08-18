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
        public ObservableConcurrentDictionary<string, PriceModel> Model { get; set; }

        private TaskFactory uiFactory; //dispatching
        private FileStream _fs;
        private StreamReader _sr;

        public TickerViewModel()
        {
            Model = new ObservableConcurrentDictionary<string, PriceModel>();
            uiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            
            _timer = new Timer(s => 
            {
                for(int i=0; i<5; i++)
                {
                    try
                    {
                        var str = ReadOneLine();
                        var dto = new TickerModelDTO(str);
                        uiFactory.StartNew(() =>
                        {
                            if (Model.ContainsKey(dto.Symbol) == false)
                            {
                                Price newPrice = new Price { Value = dto.Price, Change = PriceChange.Constant };
                                Model.Add(dto.Symbol, new PriceModel(newPrice));
                            }
                            else
                            {
                                Price currentPrice;
                                PriceChange change = PriceChange.Constant;

                                var previousPrice = Model[dto.Symbol].Peek();
                                if (previousPrice.Value > dto.Price)
                                {
                                    change = PriceChange.Decreasing;
                                    
                                }

                                if (previousPrice.Value < dto.Price)
                                {
                                    change = PriceChange.Increasing;
                                }

                                currentPrice = new Price { Value = dto.Price, Change = change };
                                Model[dto.Symbol].Push(currentPrice);
                            }
                        });
                    }
                    catch(Exception ex)
                    {
                        //tbd log
                    }
                }
            });

            _timer.Change(0, 1000);

            _fs = new FileStream("Sample Data.txt", FileMode.Open);
            _sr = new StreamReader(_fs, Encoding.Default);
        }

        ~TickerViewModel()
        {
            Dispose(false);
        }

        Timer _timer;

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
