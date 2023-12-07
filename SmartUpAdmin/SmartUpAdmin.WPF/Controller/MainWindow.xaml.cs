using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SmartUp.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonToCourse(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new Uri("./View/ViewCourse.xaml", UriKind.Relative));
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new Uri("./View/ViewSemester.xaml", UriKind.Relative));
        }
    }
}
