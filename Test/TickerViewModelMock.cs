using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ticker.VM;

namespace Test
{
    class TickerViewModelMock : TickerViewModel
    {
        Action _testMethod;

        public TickerViewModelMock(Action testMethod)
        {
            _testMethod = testMethod;
            TimerCallback(null);
        }

        protected override void TimerCallback(object status)
        {
            _testMethod();
        }
    }
}
