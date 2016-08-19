using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Ticker.VM;

namespace TickerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IViewModel vm = DataContext as IViewModel;
            if(vm != null)
            {
                vm.OnViewLoaded();
            }
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            IViewModel vm = DataContext as IViewModel;
            if (vm != null)
            {
                vm.OnViewUnloaded();
            }
        }

        private void watchlist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PriceHistoryWindow phw = new PriceHistoryWindow();
            phw.DataContext = watchlist.SelectedItem;
            phw.Show();
        }
    }
}
