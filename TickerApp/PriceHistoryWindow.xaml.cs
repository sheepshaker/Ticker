﻿using System;
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
using System.Windows.Shapes;

namespace TickerApp
{
    /// <summary>
    /// Interaction logic for PriceHistory.xaml
    /// </summary>
    public partial class PriceHistoryWindow : Window
    {
        public PriceHistoryWindow()
        {
            InitializeComponent();
            Loaded += PriceHistoryWindow_Loaded;
        }

        private void PriceHistoryWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var x = DataContext;
        }
    }
}
