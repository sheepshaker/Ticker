using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Ticker
{
    public class TickerViewModel
    {
        public ObservableConcurrentDictionary<string, ObservableMaxStack<decimal>> Model { get; set; }
        public string TestN { get { return "jack"; } }
        public decimal TestValue { get { return 12345.12345m; } }
        public ObservableCollection<TestClass> Model2 { get; set; }
        TaskFactory uiFactory;

        public TickerViewModel()
        {
            Model = new ObservableConcurrentDictionary<string, ObservableMaxStack<decimal>>();
            var stack = new ObservableMaxStack<decimal>(10);
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);
            stack.Push(5);
            Model.Add("test", stack);
            Model2 = new ObservableCollection<TestClass> { new TestClass { TestName = "test1" }, new TestClass { TestName = "test2" }, new TestClass { TestName = "test3" } };

            uiFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());


            _timer = new Timer(s => 
            {
                uiFactory.StartNew(() => {
                    var curVal = Model["test"].Peek();
                    Model["test"].Push(curVal + 1);
                });
            });

            _timer.Change(0, 100);
        }

        Timer _timer; 
    }

    public class TestClass
    {
        private string _testName = "testName";
        public string TestName {
            get { return _testName; }
            set { _testName = value; } }
    }
}
