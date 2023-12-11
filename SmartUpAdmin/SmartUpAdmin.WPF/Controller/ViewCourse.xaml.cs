using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartUpAdmin.WPF
{
    public partial class ViewCourse : Page
    {
        public ViewCourse()
        {
            InitializeComponent();
            foreach (Course course in CourseDao.GetInstance().GetAllCourses())
            {
                AddCourseBlock(course);
            }
        }

        private void AddCourseButton(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddCourse());
        }

        private void AddCourseBlock(Course course)
        {
            Border card = new Border();
            card.CornerRadius = new CornerRadius(20, 20, 20, 20);
            card.Background = Brushes.Gray;
            card.Margin = new Thickness(5);

            Grid cardGrid = new Grid();
            RowDefinition rowDefinition0 = new RowDefinition();
            rowDefinition0.Height = new GridLength(50); // Set a fixed height (adjust as needed)
            RowDefinition rowDefinition1 = new RowDefinition();
            rowDefinition1.Height = new GridLength(30); // Adjust as needed

            cardGrid.RowDefinitions.Add(rowDefinition0);
            cardGrid.RowDefinitions.Add(rowDefinition1);

            TextBlock courseName = new TextBlock();
            courseName.Text = course.Name;
            courseName.VerticalAlignment = VerticalAlignment.Center;
            courseName.HorizontalAlignment = HorizontalAlignment.Center;
            courseName.FontSize = 20;
            courseName.FontWeight = FontWeights.Bold;
            courseName.Foreground = Brushes.White;
            courseName.TextTrimming = TextTrimming.CharacterEllipsis;
            courseName.MaxWidth = 150;
            Grid.SetRow(courseName, 0);

            Ellipse circle = new Ellipse();
            circle.Fill = Brushes.White;
            circle.HorizontalAlignment = HorizontalAlignment.Right;
            circle.Margin = new Thickness(0, 0, 10, 0);
            Grid.SetRow(circle, 1);

            TextBlock informationI = new TextBlock();
            informationI.Padding = new Thickness(0, 0, 22, 0);
            informationI.Text = "i";
            informationI.FontWeight = FontWeights.Bold;
            informationI.Foreground = Brushes.White;
            informationI.VerticalAlignment = VerticalAlignment.Center;
            informationI.HorizontalAlignment = HorizontalAlignment.Right;
            Grid.SetRow(informationI, 1);

            cardGrid.Children.Add(courseName);
            cardGrid.Children.Add(circle);
            cardGrid.Children.Add(informationI);
            card.Child = cardGrid;

            CourseWrap.Children.Add(card);
        }








    }
}
