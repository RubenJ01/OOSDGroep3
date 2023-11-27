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
            ContentArea.Navigate(new Uri("./View/GradeUpdateScreen.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new Uri("./View/GradeStudent.xaml", UriKind.Relative));
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new Uri("./View/SemesterStudent.xaml", UriKind.Relative));
        }
    }
}
