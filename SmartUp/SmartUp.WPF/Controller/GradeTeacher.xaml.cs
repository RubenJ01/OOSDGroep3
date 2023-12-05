using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace SmartUp.UI
{
    /// <summary>
    /// Interaction logic for GradeTeacher.xaml
    /// </summary>
    public partial class GradeTeacher : Page
    {
        public GradeTeacher()
        {
            InitializeComponent();
            List<string> Courses = CourseDao.GetInstance().GetAllCourseNames();
            CoursesCombobox.ItemsSource = Courses;
            List<string> Classes = ClassDao.GetInstance().GetClassNames();
            ClassesCombobox.ItemsSource = Classes;

            //GradesStudentGrid.ItemsSource = GradeDao.GetInstance().GetGradesByCourse("Advanced Web Technologies");

            //foreach (Grade grade in GradeDao.GetInstance().GetGradesByCourse("Advanced Web Technologies"))
            //{
            //    AddGradeView(grade);
            //}
        }

        //public void AddGradeView(Grade model)
        //{
        //    Grid grid = new Grid();
        //    grid.Height = 40;
        //    ColumnDefinition colDef1 = new ColumnDefinition();
        //    ColumnDefinition colDef2 = new ColumnDefinition();
        //    ColumnDefinition colDef3 = new ColumnDefinition();
        //    ColumnDefinition colDef4= new ColumnDefinition();
        //    ColumnDefinition colDef5 = new ColumnDefinition();
        //    colDef1.Width = new GridLength(1, GridUnitType.Star);
        //    colDef2.Width = new GridLength(1, GridUnitType.Star);
        //    colDef3.Width = new GridLength(1, GridUnitType.Star);
        //    colDef4.Width = new GridLength(1, GridUnitType.Star);
        //    colDef5.Width = new GridLength(1, GridUnitType.Star);
        //    grid.ColumnDefinitions.Add(colDef1);
        //    grid.ColumnDefinitions.Add(colDef2);
        //    grid.ColumnDefinitions.Add(colDef3);
        //    grid.ColumnDefinitions.Add(colDef4);
        //    grid.ColumnDefinitions.Add(colDef5);
        //    RowDefinition rowDef1 = new RowDefinition();
        //    grid.RowDefinitions.Add(rowDef1);

        //    TextBlock StudentId = new TextBlock();
        //    StudentId.Text = model.Student.StudentId;
        //    StudentId.FontSize = 14;
        //    StudentId.HorizontalAlignment = HorizontalAlignment.Center;
        //    StudentId.VerticalAlignment = VerticalAlignment.Center;
        //    Grid.SetRow(StudentId, 0);
        //    Grid.SetColumn(StudentId, 0);


        //    TextBlock Name = new TextBlock();
        //    Name.Text = model.Student.FullName;
        //    Name.FontSize = 14;
        //    Name.HorizontalAlignment = HorizontalAlignment.Left;
        //    Name.VerticalAlignment = VerticalAlignment.Center;
        //    Grid.SetRow(Name, 1);
        //    Grid.SetColumn(Name, 1);

        //    TextBlock Course = new TextBlock();
        //    Course.Text = model.CourseName;
        //    Course.FontSize = 14;
        //    Course.HorizontalAlignment = HorizontalAlignment.Left;
        //    Course.VerticalAlignment = VerticalAlignment.Center;
        //    Grid.SetRow(Course, 2);
        //    Grid.SetColumn(Course, 2);

        //    TextBlock Grade = new TextBlock();
        //    if (model.GradeNumber != null && !string.IsNullOrEmpty(model.GradeNumber.ToString()))
        //    {
        //        Grade.Text = model.GradeNumber.ToString();
        //    }
        //    else
        //    {
        //        Grade.Text = " ";
        //    }
        //    Grade.FontSize = 14;
        //    Grade.HorizontalAlignment = HorizontalAlignment.Center;
        //    Grade.VerticalAlignment = VerticalAlignment.Center;
        //    Grid.SetRow(Grade, 3);
        //    Grid.SetColumn(Grade, 3);

        //    TextBlock IsDefinitive = new TextBlock();

        //    string text = " ";
        //    if(model.IsDefinitive != null && !string.IsNullOrEmpty(model.IsDefinitive.ToString()))
        //    {
        //        text = model.IsDefinitive.ToString();
        //        if (model.IsDefinitive.GetValueOrDefault())
        //        {

        //            IsDefinitive.Text = "Definitief";
        //        }
        //        else
        //        {
        //            IsDefinitive.Text = "Voorlopig";

        //        }
        //    }
        //    Debug.WriteLine($"Test in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: test {model.GradeNumber} {model.IsDefinitive}");

        //    IsDefinitive.FontSize = 14;
        //    IsDefinitive.HorizontalAlignment = HorizontalAlignment.Center;
        //    IsDefinitive.VerticalAlignment = VerticalAlignment.Top;
        //    Grid.SetRow(IsDefinitive, 4);
        //    Grid.SetColumn(IsDefinitive, 4);

        //    grid.Children.Add(StudentId);
        //    grid.Children.Add(Name);
        //    grid.Children.Add(Course);
        //    grid.Children.Add(Grade);
        //    grid.Children.Add(IsDefinitive);

        //    GradesStudentOverview.Children.Add(grid);
        ////}
        //private void Course_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //    GradesStudentGrid.ItemsSource = GradeDao.GetInstance().GetGradesByCourse("Advanced Web Technologies");

        //    string selectedText = CoursesCombobox.SelectedItem.ToString();
        //    foreach (Grade grade in GradeDao.GetInstance().GetGradesByCourse(selectedText))
        //    {
        //        AddGradeView(grade);
        //    }

        //}
        //private void Class_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    GradesStudentOverview.Children.Clear();

        //    string selectedText = ClassesCombobox.SelectedItem.ToString();
        //    foreach (Grade grade in GradeDao.GetInstance().GetGradesByClass(selectedText))
        //    {
        //        AddGradeView(grade);
        //    }

        //}
        //public void setLayoutDataGrid()
        //{
        //    GradesStudentGrid.FontSize = 15;
        //    GradesStudentGrid.ColumnWidth = 315;
        //}

        private void Course_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedText = CoursesCombobox.SelectedItem.ToString();
            setLayoutDataGrid();
            GradesStudentGrid.ItemsSource = GradeDao.GetInstance().GetGradesByCourse("Advanced Web Technologies");

        }
        private void Class_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string selectedText = ClassesCombobox.SelectedItem.ToString();
            setLayoutDataGrid();
            GradesStudentGrid.ItemsSource = GradeDao.GetInstance().GetGradesByClass(selectedText);

        }
        public void setLayoutDataGrid()
        {
            GradesStudentGrid.FontSize = 15;
            GradesStudentGrid.ColumnWidth = 315;
        }

    }
}
