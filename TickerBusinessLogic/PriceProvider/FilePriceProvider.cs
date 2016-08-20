using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ticker.Model;
using System.Threading;
using System.IO;

namespace Ticker.PriceProvider
{
    public class FilePriceProvider : PriceProvider, IDisposable
    {
        private FileStream _fs;
        private StreamReader _sr;
        private Timer _timer;

        public FilePriceProvider(string fileName)
        {
            _timer = new Timer(TimerCallback);

            _fs = new FileStream(fileName, FileMode.Open);
            _sr = new StreamReader(_fs, Encoding.Default);
        }

        protected virtual void TimerCallback(object status)
        {
            lock (_timer)//one tick at a time
            {
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        var str = ReadOneLine();
                        var dto = new TickerModelDTO(str);
                        RaisePriceUpdate(dto.Symbol, dto.Price);
                    }
                    catch (Exception ex)
                    {
                        //tbd log
                    }
                }
            }
        }

        public override void Start(int delay)
        {
            _timer.Change(0, delay);
            base.Start(delay);
        }


        public override void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            base.Stop();
        }

        private string ReadOneLine()
        {
            if (_sr.EndOfStream)
            {
                _sr.BaseStream.Seek(0, SeekOrigin.Begin);
            }

            return _sr.ReadLine();
        }

        public override void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }

            if (_sr != null)
            {
                _sr.Dispose();
                _sr = null;
            }

            if (_fs != null)
            {
                _fs.Dispose();
                _fs = null;
            }

            base.Dispose();
        }
    }
}
