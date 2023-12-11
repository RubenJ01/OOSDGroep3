using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.WPF.View;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            card.Width = 240;
            card.Height = 120;

            Grid cardGrid = new Grid();
            RowDefinition rowDefinition0 = new RowDefinition();
            cardGrid.RowDefinitions.Add(rowDefinition0);

            TextBlock courseName = new TextBlock();
            courseName.Text = course.Name;
            courseName.VerticalAlignment = VerticalAlignment.Center;
            courseName.HorizontalAlignment = HorizontalAlignment.Center;
            courseName.FontSize = 24;
            courseName.TextWrapping = TextWrapping.Wrap;
            courseName.TextAlignment = TextAlignment.Center;
            courseName.FontWeight = FontWeights.SemiBold;
            courseName.Foreground = Brushes.White;
            courseName.TextTrimming = TextTrimming.CharacterEllipsis;
            courseName.MaxWidth = 220;
            Grid.SetRow(courseName, 0);

            Ellipse circle = new Ellipse();
            circle.Height = 30;
            circle.Width = 30;
            circle.Fill = Brushes.White;
            circle.HorizontalAlignment = HorizontalAlignment.Right;
            circle.VerticalAlignment = VerticalAlignment.Bottom;
            circle.Margin = new Thickness(0, 0, 10, 10);
            circle.ToolTip = course.Name;
            Grid.SetRow(circle, 1);

            TextBlock informationI = new TextBlock();
            informationI.Padding = new Thickness(0, 0, 23, 15);
            informationI.Text = "i";
            informationI.FontSize = 15;
            informationI.FontWeight = FontWeights.Bold;
            informationI.Foreground = Brushes.Black;
            informationI.VerticalAlignment = VerticalAlignment.Bottom;
            informationI.HorizontalAlignment = HorizontalAlignment.Right;
            informationI.ToolTip = course.Name;
            Grid.SetRow(informationI, 1);

            cardGrid.Children.Add(courseName);
            card.Child = cardGrid;

            CourseWrap.Children.Add(card);
        }








    }
}
