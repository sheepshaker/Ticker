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

            var z = watchlist.DataContext;
        }

        private void watchlist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PriceHistoryWindow phw = new PriceHistoryWindow();
            phw.DataContext = watchlist.SelectedItem;
            //phw.DataContext = new MyContext();
            phw.Show();
        }
    }

    public class MyContext
    {
        public MyContext()
        {
            Names = new ObservableCollection<decimal>();
            Names.Add(1);
            Names.Add(2);
            Names.Add(3);
            Names.Add(4);
        }

        public string Name { get { return "Jacek"; } }
        public ObservableCollection<decimal> Names { get; private set; }
    }
}
