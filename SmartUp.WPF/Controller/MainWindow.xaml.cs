using System;
using System.Windows;

namespace SmartUp.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new Uri("./View/Page1.xaml", UriKind.Relative));
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new Uri("./View/Page2.xaml", UriKind.Relative));
        }
    }
}
