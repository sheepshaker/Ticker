using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ticker.VM;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using Ticker.Model;
using Ticker.PriceProvider;

namespace Test
{
    [TestClass]
    public class TickerViewModelTest
    {
        TickerViewModel _vm;
        TestPriceProvider _priceProvider;

        public TickerViewModelTest()
        {
            _priceProvider = new TestPriceProvider();
            _vm = new TickerViewModel(_priceProvider);
        }

        [TestMethod]
        public void TestPropertyChangedOnPricePush()
        {
            AssertPropertyChanged(_vm, (x) => { _priceProvider.Push("symbol1", 1); }, "Watchlists");
            AssertPropertyChanged(_vm.WatchlistMap["symbol1"], (x) => { _priceProvider.Push("symbol1", 1); }, new[] { "CurrentPrice", "AveragePrice", "HistoricalPrices" });
            
            AssertPropertyChanged(_vm, (x) => { _priceProvider.Push("symbol2", 1); }, "Watchlists");
            AssertPropertyChanged(_vm.WatchlistMap["symbol2"], (x) => { _priceProvider.Push("symbol2", 1); }, new[] { "CurrentPrice", "AveragePrice", "HistoricalPrices" });
            
            AssertPropertyNotChanged(_vm, (x) => { _priceProvider.Push("symbol2", 2); }, "Watchlists");
            AssertPropertyChanged(_vm.WatchlistMap["symbol2"], (x) => { _priceProvider.Push("symbol2", 2); }, new[] { "CurrentPrice", "AveragePrice", "HistoricalPrices" });
        }

        [TestMethod]
        public void TestPriceValuesConstant()
        {
            _priceProvider.Push("symbol2", 1);
            Assert.AreEqual(_vm.WatchlistMap["symbol2"].CurrentPrice.Value, 1);
            Assert.AreEqual(_vm.WatchlistMap["symbol2"].CurrentPrice.Change, PriceChange.Constant);

            _priceProvider.Push("symbol2", 1);      
            Assert.AreEqual(_vm.WatchlistMap["symbol2"].CurrentPrice.Value, 1);
            Assert.AreEqual(_vm.WatchlistMap["symbol2"].CurrentPrice.Change, PriceChange.Constant);
        }

        [TestMethod]
        public void TestPriceValuesIncrease()
        {
            _priceProvider.Push("symbol3", 1);
            Assert.AreEqual(_vm.WatchlistMap["symbol3"].CurrentPrice.Value, 1);
            Assert.AreEqual(_vm.WatchlistMap["symbol3"].CurrentPrice.Change, PriceChange.Constant);

            _priceProvider.Push("symbol3", 2);
            Assert.AreEqual(_vm.WatchlistMap["symbol3"].CurrentPrice.Value, 2);
            Assert.AreEqual(_vm.WatchlistMap["symbol3"].CurrentPrice.Change, PriceChange.Increasing);
        }

        [TestMethod]
        public void TestPriceValuesDecrease()
        {
            _priceProvider.Push("symbol4", 2);
            Assert.AreEqual(_vm.WatchlistMap["symbol4"].CurrentPrice.Value, 2);
            Assert.AreEqual(_vm.WatchlistMap["symbol4"].CurrentPrice.Change, PriceChange.Constant);

            _priceProvider.Push("symbol4", 1);
            Assert.AreEqual(_vm.WatchlistMap["symbol4"].CurrentPrice.Value, 1);
            Assert.AreEqual(_vm.WatchlistMap["symbol4"].CurrentPrice.Change, PriceChange.Decreasing);
        }

        [TestMethod]
        public void TestPriceAverage()
        {
            _priceProvider.Push("symbol5", 1);
            Assert.AreEqual(_vm.WatchlistMap["symbol5"].AveragePrice.Value, 1);

            _priceProvider.Push("symbol5", 2);
            Assert.AreEqual(_vm.WatchlistMap["symbol5"].AveragePrice.Value, 3 /2m);

            _priceProvider.Push("symbol5", 3);
            Assert.AreEqual(_vm.WatchlistMap["symbol5"].AveragePrice.Value, 6 / 3m);

            _priceProvider.Push("symbol5", 4);
            Assert.AreEqual(_vm.WatchlistMap["symbol5"].AveragePrice.Value, 10 / 4m);

            _priceProvider.Push("symbol5", 5);
            Assert.AreEqual(_vm.WatchlistMap["symbol5"].AveragePrice.Value, 15 / 5m);

            _priceProvider.Push("symbol5", 6);
            Assert.AreEqual(_vm.WatchlistMap["symbol5"].AveragePrice.Value, 20 / 5m);

            _priceProvider.Push("symbol5", 7);
            Assert.AreEqual(_vm.WatchlistMap["symbol5"].AveragePrice.Value, 25 / 5m);
        }

        [TestMethod]
        public void TestPriceHistory()
        {
            _priceProvider.Push("symbol6", 1);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.Count(), 1);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.FirstOrDefault().Value, 1);

            _priceProvider.Push("symbol6", 2);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.Count(), 2);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.FirstOrDefault().Value, 2);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.LastOrDefault().Value, 1);

            _priceProvider.Push("symbol6", 3);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.Count(), 3);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.FirstOrDefault().Value, 3);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.LastOrDefault().Value, 1);

            _priceProvider.Push("symbol6", 4);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.Count(), 4);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.FirstOrDefault().Value, 4);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.LastOrDefault().Value, 1);

            _priceProvider.Push("symbol6", 5);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.Count(), 5);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.FirstOrDefault().Value, 5);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.LastOrDefault().Value, 1);

            _priceProvider.Push("symbol6", 6);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.Count(), 6);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.FirstOrDefault().Value, 6);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.LastOrDefault().Value, 1);

            _priceProvider.Push("symbol6", 7);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.Count(), 7);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.FirstOrDefault().Value, 7);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.LastOrDefault().Value, 1);

            _priceProvider.Push("symbol6", 8);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.Count(), 8);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.FirstOrDefault().Value, 8);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.LastOrDefault().Value, 1);

            _priceProvider.Push("symbol6", 9);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.Count(), 9);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.FirstOrDefault().Value, 9);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.LastOrDefault().Value, 1);

            _priceProvider.Push("symbol6", 10);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.Count(), 10);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.FirstOrDefault().Value, 10);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.LastOrDefault().Value, 1);

            _priceProvider.Push("symbol6", 11);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.Count(), 10);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.FirstOrDefault().Value, 11);
            Assert.AreEqual(_vm.WatchlistMap["symbol6"].HistoricalPrices.LastOrDefault().Value, 2);
        }

        [TestMethod]
        public void TestVmCreationException()
        {
            AssertException.Throws<ArgumentNullException>(() => { IViewModel vm = new TickerViewModel(null); });
        }

        [TestMethod]
        public void TestFileProviderCreationException()
        {
            AssertException.Throws<ArgumentNullException>(() => { IPriceProvider provider = new FilePriceProvider(null); });
            AssertException.Throws<ArgumentNullException>(() => { IPriceProvider provider = new FilePriceProvider(string.Empty); });
            AssertException.Throws<Exception>(() => { IPriceProvider provider = new FilePriceProvider("?"); });
        }

        private void AssertPropertyChanged<T>(T instance, Action<T> actionPropertySetter, string expectedPropertyName) where T : INotifyPropertyChanged
        {
            string actual = null;
            instance.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                actual = e.PropertyName;
            };
            actionPropertySetter.Invoke(instance);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedPropertyName, actual);
        }

        private void AssertPropertyNotChanged<T>(T instance, Action<T> actionPropertySetter, string expectedPropertyName) where T : INotifyPropertyChanged
        {
            string actual = null;
            instance.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                actual = e.PropertyName;
            };
            actionPropertySetter.Invoke(instance);
            Assert.IsNull(actual);
        }

        private void AssertPropertyChanged<T>(T instance, Action<T> actionPropertySetter, string[] expectedPropertyNames) where T : INotifyPropertyChanged
        {
            var actualNames = new List<string>();
            instance.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                actualNames.Add(e.PropertyName);
            };
            actionPropertySetter.Invoke(instance);
            CollectionAssert.AreEqual(actualNames, expectedPropertyNames);
        }

        public static class AssertException
        {
            public static void Throws<T>(Action func) where T : Exception
            {
                var exceptionThrown = false;
                try
                {
                    func.Invoke();
                }
                catch (T)
                {
                    exceptionThrown = true;
                }

                if (!exceptionThrown)
                {
                    throw new AssertFailedException(
                        String.Format("An exception of type {0} was expected, but not thrown", typeof(T))
                        );
                }
            }
        }
    }
}
