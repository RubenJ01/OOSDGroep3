using SmartUp.DataAccess.SQLServer.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Diagnostics;
using Azure;
using Microsoft.Identity.Client;

namespace SmartUp.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AddButtonsStudent();
        }


        public void SetContentArea(Uri uri)
        {
            ContentArea.Navigate(uri);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new Uri("./View/GradeStudent.xaml", UriKind.Relative));
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new Uri("./View/SemesterStudent.xaml", UriKind.Relative));
        }
        private void ButtonToStudent_Click(object sender, RoutedEventArgs e)
        {
            stackpanelButtons.Children.Clear();
            AddButtonsStudent();
        }
        private void ButtonToTeacher_Click(object sender, RoutedEventArgs e)
        {
            stackpanelButtons.Children.Clear();
            AddButtonsDocent();
        }
        private void ButtonToGradesSb_Student_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new Uri("./View/SbStudent.xaml", UriKind.Relative));
        }
        private void ButtonToGradesTeacher_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Navigate(new Uri("./View/GradeTeacher.xaml", UriKind.Relative));
        }

        public void AddButtonsStudent()
        {
            Button GradeButton  = new Button();
            GradeButton.Margin = new Thickness(0,120,0,0);
            GradeButton.Background = Brushes.Transparent;
            GradeButton.BorderBrush = Brushes.Transparent;
            GradeButton.Click += Button_Click;

            Image ImgGrade = new Image();
            ImgGrade.Source = new BitmapImage(new Uri("pack://application:,,,/Img/StudentGrade.png"));
            ImgGrade.Width = 100;
            ImgGrade.Height = 100;

            GradeButton.Content = ImgGrade;

            Button SemesterButton = new Button();
            SemesterButton.Margin = new Thickness(0, 120, 0, 0);
            SemesterButton.Background = Brushes.Transparent;
            SemesterButton.BorderBrush = Brushes.Transparent;
            SemesterButton.Click += Button_Click1;

            Image ImgSemester = new Image();
            ImgSemester.Source = new BitmapImage(new Uri("pack://application:,,,/Img/SemesterIcon.png"));
            ImgSemester.Width = 100;
            ImgSemester.Height = 100;

            SemesterButton.Content = ImgSemester;

            Button ButtonToStudent = new Button();
            ButtonToStudent.Margin = new Thickness(0, 300, 0, 0);
            ButtonToStudent.Foreground = Brushes.Black;
            ButtonToStudent.FontSize = 20;
            ButtonToStudent.FontWeight = FontWeights.Bold;
            ButtonToStudent.Content = "naar docent";
            ButtonToStudent.Width = 130;
            ButtonToStudent.Click += ButtonToTeacher_Click;


            stackpanelButtons.Children.Add(GradeButton);
            stackpanelButtons.Children.Add(SemesterButton);
            stackpanelButtons.Children.Add(ButtonToStudent);
        }
        public void AddButtonsDocent()
        {
            Button GradeButton = new Button();
            GradeButton.Margin = new Thickness(0, 120, 0, 0);
            GradeButton.Background = Brushes.Transparent;
            GradeButton.BorderBrush = Brushes.Transparent;
            GradeButton.Click += ButtonToGradesTeacher_Click;

            Image ImgGrade = new Image();
            ImgGrade.Source = new BitmapImage(new Uri("pack://application:,,,/Img/StudentGrade.png"));
            ImgGrade.Width = 100;
            ImgGrade.Height = 100;

            GradeButton.Content = ImgGrade;


            Button Sb_StudentButton = new Button();
            Sb_StudentButton.Margin = new Thickness(0, 120, 0, 0);
            Sb_StudentButton.Background = Brushes.Transparent;
            Sb_StudentButton.BorderBrush = Brushes.Transparent;
            Sb_StudentButton.Click += ButtonToGradesSb_Student_Click;

            Image ImgSbStudent = new Image();
            ImgSbStudent.Source = new BitmapImage(new Uri("pack://application:,,,/Img/GradeSberStudent.png"));
            ImgSbStudent.Width = 100;
            ImgSbStudent.Height = 100;

            Sb_StudentButton.Content = ImgSbStudent;


            Button ButtonToStudent = new Button();
            ButtonToStudent.Margin = new Thickness(0, 300, 0, 0);
            ButtonToStudent.Foreground = Brushes.Black;
            ButtonToStudent.FontSize = 20;
            ButtonToStudent.FontWeight = FontWeights.Bold;
            ButtonToStudent.Content = "naar student";
            ButtonToStudent.Width = 130;
            ButtonToStudent.Click += ButtonToStudent_Click;


            stackpanelButtons.Children.Add(GradeButton);
            stackpanelButtons.Children.Add(Sb_StudentButton);
            stackpanelButtons.Children.Add(ButtonToStudent);
        }
    }
}
