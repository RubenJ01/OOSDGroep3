using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
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
using System.Windows.Shapes;
using SmartUp.Core.Constants;
using System.Xml.Linq;
using System.Diagnostics;

namespace SmartUp.UI
{
    public partial class SbStudent : Page
    {
        List<Student> SbStudents {  get; set; }

        public SbStudent()
        {
            InitializeComponent();
            SbStudents = StudentDao.GetInstance().GetStudentNameByMentor((Constants.DOCENT_ID));
            SbStudentsNames.ItemsSource = SbStudents;
            SbStudentsNames.DisplayMemberPath = "FullName";


        }
        public void AddGradeView(DataAccess.SQLServer.Model.GradeStudent model)
        {
            Grid grid = new Grid();
            grid.Height = 120;
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            colDef1.Width = new GridLength(2, GridUnitType.Star);
            colDef2.Width = new GridLength(1, GridUnitType.Star);
            colDef3.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(colDef1);
            grid.ColumnDefinitions.Add(colDef2);
            grid.ColumnDefinitions.Add(colDef3);
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);

            TextBlock Course = new TextBlock();
            Course.Text = model.CourseName;
            Course.FontSize = 20;
            Course.HorizontalAlignment = HorizontalAlignment.Center;
            Course.VerticalAlignment = VerticalAlignment.Center;
            Course.FontWeight = FontWeights.SemiBold;
            Course.TextWrapping = TextWrapping.Wrap;
            Grid.SetRow(Course, 0);
            Grid.SetColumn(Course, 0);

            TextBlock isDefinitive = new TextBlock();
            if (model.IsDefinitive.GetValueOrDefault())
            {

                isDefinitive.Text = "Definitief";
            }
            else
            {
                isDefinitive.Text = "Voorlopig";

            }
            isDefinitive.FontSize = 15;
            isDefinitive.HorizontalAlignment = HorizontalAlignment.Center;
            isDefinitive.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(isDefinitive, 1);
            Grid.SetColumn(isDefinitive, 0);

            TextBlock credits = new TextBlock();
            credits.Text = $"{model.Credits} EC";
            credits.FontSize = 15;
            credits.HorizontalAlignment = HorizontalAlignment.Right;
            credits.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumn(credits, 1);
            Grid.SetRow(credits, 0);

            TextBlock grade = new TextBlock();
            grade.Text = $"{model.GradeNumber}";
            grade.FontSize = 20;
            grade.HorizontalAlignment = HorizontalAlignment.Center;
            grade.VerticalAlignment = VerticalAlignment.Center;
            grade.FontWeight = FontWeights.SemiBold;
            Grid.SetColumn(grade, 2);
            Grid.SetRow(grade, 0);

            TextBlock date = new TextBlock();
            date.Text = $"{model.PublishedOn:dd-MM-yyyy}";
            date.FontSize = 15;
            date.HorizontalAlignment = HorizontalAlignment.Center;
            date.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumn(date, 2);
            Grid.SetRow(date, 1);

            grid.Children.Add(Course);
            grid.Children.Add(isDefinitive);
            grid.Children.Add(credits);
            grid.Children.Add(grade);
            grid.Children.Add(date);

            GradeSbStudentOverview.Children.Add(grid);
        }
        private void SbStudentsNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GradeSbStudentOverview.Children.Clear();

            if (SbStudentsNames.SelectedItem != null && SbStudentsNames.SelectedItem is Student selectedStudent)
            {
                string selectedText = selectedStudent.StudentId;
                foreach (DataAccess.SQLServer.Model.GradeStudent grade in GradeDao.GetInstance().GetGradesByStudentId(selectedText))
                {
                    AddGradeView(grade);
                }
                TitleSbStudent.Text = "Cijfers van: " + selectedStudent.FullName;
            }

        }

    }
}
