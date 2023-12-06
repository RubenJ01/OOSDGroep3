using Microsoft.Identity.Client;
using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;


namespace SmartUp.UI
{

    public partial class GradeTeacher : Page
    {
        private Brush originalBackgroundColor = null;
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
            if (ClassesCombobox.SelectedIndex > -1)
            {
                string selectedText = CoursesCombobox.SelectedItem.ToString();
                GradesStudentGrid.ItemsSource = GradeDao.GetInstance().GetGradesByCourseAndClass(selectedText, ClassesCombobox.SelectedItem.ToString());
                setLayoutDataGrid();
                List<string> Classes = ClassDao.GetInstance().GetClassNameByCourse(selectedText);
                ClassesCombobox.ItemsSource = Classes;
            }
            else
            {
                string selectedText = CoursesCombobox.SelectedItem.ToString();
                GradesStudentGrid.ItemsSource = GradeDao.GetInstance().GetGradesByCourse(selectedText);
                setLayoutDataGrid();
                List<string> Classes = ClassDao.GetInstance().GetClassNameByCourse(selectedText);
                ClassesCombobox.ItemsSource = Classes;
            }
        }
        private void Class_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CoursesCombobox.SelectedIndex > -1)
            {
                string selectedText = ClassesCombobox.SelectedItem.ToString();
                GradesStudentGrid.ItemsSource = GradeDao.GetInstance().GetGradesByCourseAndClass(CoursesCombobox.SelectedItem.ToString(), selectedText);
                setLayoutDataGrid();
                List<string> Courses = CourseDao.GetInstance().GetCoursNameByClass(selectedText);
                CoursesCombobox.ItemsSource = Courses;
            }
            else
            {
                string selectedText = ClassesCombobox.SelectedItem.ToString();
                GradesStudentGrid.ItemsSource = GradeDao.GetInstance().GetGradesByClass(selectedText);
                setLayoutDataGrid();
                List<string> Courses = CourseDao.GetInstance().GetCoursNameByClass(selectedText);
                CoursesCombobox.ItemsSource = Courses;
            }
        }
        private void GradesStudentGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit && e.Column.DisplayIndex == 3)
            {
                TextBox textBox = e.EditingElement as TextBox;
                if (textBox != null)
                {
                    string newGradeText = textBox.Text;
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: test2.6");
                    Debug.WriteLine($"Type of e.Row.Item: {e.Row.Item.GetType().FullName}");
                    if (e.Row.Item is SmartUp.DataAccess.SQLServer.Model.GradeTeacher gradeTeacher)
                    {
                        UpdateGrade(gradeTeacher.StudentId, gradeTeacher.Vak, newGradeText, e);
                    }
                }

            }
        }

        public void setLayoutDataGrid()
        {
            GradesStudentGrid.FontSize = 24;
            GradesStudentGrid.Columns[0].Width = 293;
            GradesStudentGrid.Columns[1].Width = 293;
            GradesStudentGrid.Columns[2].Width = 475;
            GradesStudentGrid.Columns[3].Width = 243;
            GradesStudentGrid.Columns[4].Width = 268;
        }
        public bool IsValid(Decimal grade, DataGridCellEditEndingEventArgs e)
        {
            DataGridCell cell = GetCell(e.Row, e.Column);

            if (originalBackgroundColor == null)
            {
                originalBackgroundColor = cell.Background;
            }

            if (grade > 0 && grade <= 10)
            {
                cell.Background = originalBackgroundColor;
                return true;
            }
            cell.Background = Brushes.Red;
            return false;
        }

        public void UpdateGrade(string studentId, string course, string newGradeText, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                decimal.TryParse(newGradeText, out decimal newGrade);
                if (IsValid(newGrade, e))
                {
                    GradeDao.GetInstance().UpdateGradeFirsTry(studentId, course, newGrade);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        private DataGridCell GetCell(DataGridRow row, DataGridColumn column)
        {
            if (column != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);

                if (presenter != null)
                {
                    int columnIndex = GradesStudentGrid.Columns.IndexOf(column);

                    if (columnIndex > -1)
                    {
                        DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex) as DataGridCell;
                        return cell;
                    }
                }
            }

            return null;
        }

        private childItem GetVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = GetVisualChild<childItem>(child);

                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }

            return null;
        }
    }
}
