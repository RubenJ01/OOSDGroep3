using SmartUp.DataAccess.SQLServer.Dao;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;


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
        }

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
            if (e.EditAction == DataGridEditAction.Commit )
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
            GradesStudentGrid.Columns[0].IsReadOnly = true;
            GradesStudentGrid.Columns[1].Width = 293;
            GradesStudentGrid.Columns[1].IsReadOnly = true;
            GradesStudentGrid.Columns[2].Width = 475;
            GradesStudentGrid.Columns[2].IsReadOnly = true;
            GradesStudentGrid.Columns[3].Width = 243;
            GradesStudentGrid.Columns[4].Width = 268;
            GradesStudentGrid.Columns[4].IsReadOnly = true;
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
