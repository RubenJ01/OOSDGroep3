using SmartUp.DataAccess.SQLServer.Dao;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;


namespace SmartUp.UI
{

    public partial class GradeTeacher : Page
    {
        private Brush originalBackgroundColor = null;
        private string selectedCourse;
        private string selectedClass;
        private List<DataAccess.SQLServer.Model.GradeTeacher> GradesTeacherList;
        
        public GradeTeacher()
        {
            InitializeComponent();
            List<string> Courses = CourseDao.GetInstance().GetAllCourseNames();
            CoursesCombobox.ItemsSource = Courses;
            List<string> Classes = ClassDao.GetInstance().GetClassNames();
            ClassesCombobox.ItemsSource = Classes;
            MakeDefinitiveButton.IsEnabled = false;
            GradesTeacherList = new List<DataAccess.SQLServer.Model.GradeTeacher>();
        }

        private void Course_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCourse = CoursesCombobox.SelectedItem.ToString();
            LoadTableCourse();
        }
        private void Class_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedClass = ClassesCombobox.SelectedItem.ToString();
            LoadTableClass();
        }
        private void GradesStudentGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (e.EditAction == DataGridEditAction.Commit)
                {
                    TextBox textBox = e.EditingElement as TextBox;
                    if (textBox != null)
                    {
                        string newGradeText = textBox.Text;
                        if (e.Row.Item is SmartUp.DataAccess.SQLServer.Model.GradeTeacher gradeTeacher)
                        {
                            UpdateGrade(gradeTeacher.StudentId, gradeTeacher.Vak, newGradeText, e);
                        }
                    }
                }
            }

            catch(Exception ex)
            {
                Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
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
            GradesStudentGrid.Columns[3].CellStyle = GetDoubleCellStyle();
            GradesStudentGrid.Columns[4].Width = 268;
            GradesStudentGrid.Columns[4].IsReadOnly = true;

            GradesStudentGrid_LoadingRow();
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
                decimal newGrade;
                decimal.TryParse(newGradeText, NumberStyles.Number, CultureInfo.InvariantCulture, out newGrade);
                Debug.WriteLine(newGrade);
                if (IsValid(newGrade, e))
                {
                    GradeDao.GetInstance().UpdateGrade(studentId, course, newGrade);
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

        private DataGridCell GetCell(int rowIndex, DataGridColumn column)
        {
            if (column != null)
            {
                DataGridRow row = GradesStudentGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;

                if (row != null)
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
            }
            Debug.WriteLine("Cell was not found and is set to null");
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

        private Style GetDoubleCellStyle()
        {
            var style = new Style(typeof(DataGridCell));
            style.Setters.Add(new EventSetter(UIElement.PreviewKeyDownEvent, new KeyEventHandler(PreviewKeyDownHandler)));
            return style;
        }

        private void PreviewKeyDownHandler(object sender, KeyEventArgs e)
        {
            TextBox textBox = e.OriginalSource as TextBox;

            if (textBox != null)
            {
                if (e.Key == Key.OemPeriod || e.Key == Key.Decimal)
                {
                    // Allow decimal point if not already present
                    if (textBox.Text.Contains("."))
                    {
                        e.Handled = true;
                    }
                }
                else if (!IsDigitKey(e.Key) && e.Key != Key.Back && e.Key != Key.Enter)
                {
                    e.Handled = true;
                }
            }
        }

        private bool IsDigitKey(Key key)
        {
            return (key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9);
        }

        private void MakeDefinitive(object sender, RoutedEventArgs e)
        {
            if (ClassesCombobox.SelectedIndex > -1)
            {
                GradeDao.GetInstance().UpdateIsDefinitiveByCourseAndClass(selectedCourse, selectedClass);
            }
            else
            {
                GradeDao.GetInstance().UpdateIsDefinitiveByCourse(selectedCourse);
            }
        }

        public void LoadTableCourse()
        {
            if (ClassesCombobox.SelectedIndex > -1)
            {
                if (GradesTeacherList.Count > 0)
                {
                    GradesTeacherList.Clear();
                }
                GradesTeacherList = GradeDao.GetInstance().GetGradesByCourseAndClass(CoursesCombobox.SelectedItem.ToString(), selectedClass);
                GradesStudentGrid.ItemsSource = null;
                GradesStudentGrid.ItemsSource = GradesTeacherList;
                setLayoutDataGrid();
                List<string> Classes = ClassDao.GetInstance().GetClassNameByCourse(selectedCourse);
                ClassesCombobox.ItemsSource = Classes;
                MakeDefinitiveButton.IsEnabled = true;
            }
            else
            {
                if (GradesTeacherList.Count > 0)
                {
                    GradesTeacherList.Clear();
                }
                GradesTeacherList = GradeDao.GetInstance().GetGradesByCourse(selectedCourse);
                GradesStudentGrid.ItemsSource = null;
                GradesStudentGrid.ItemsSource = GradeDao.GetInstance().GetGradesByCourse(selectedCourse);
                setLayoutDataGrid();
                List<string> Classes = ClassDao.GetInstance().GetClassNameByCourse(selectedCourse);
                ClassesCombobox.ItemsSource = Classes;
                MakeDefinitiveButton.IsEnabled = true;
            }
        }

        public void LoadTableClass()
        {
            if (CoursesCombobox.SelectedIndex > -1)
            {
                if (GradesTeacherList.Count > 0)
                {
                    GradesTeacherList.Clear();
                }
                GradesTeacherList = GradeDao.GetInstance().GetGradesByCourseAndClass(CoursesCombobox.SelectedItem.ToString(), selectedClass);
                GradesStudentGrid.ItemsSource = GradesTeacherList;
                setLayoutDataGrid();
                List<string> Courses = CourseDao.GetInstance().GetCoursNameByClass(selectedClass);
                CoursesCombobox.ItemsSource = Courses;
            }
            else
            {
                if (GradesTeacherList.Count > 0)
                {
                    GradesTeacherList.Clear();
                }
                GradesTeacherList = GradeDao.GetInstance().GetGradesByClass(selectedClass);
                GradesStudentGrid.ItemsSource = GradesTeacherList;
                setLayoutDataGrid();
                List<string> Courses = CourseDao.GetInstance().GetCoursNameByClass(selectedClass);
                CoursesCombobox.ItemsSource = Courses;
            }
        }
        private void GradesStudentGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            for(int i = 0; i < GradesTeacherList.Count; i++)
            {
                DataAccess.SQLServer.Model.GradeTeacher item = GradesTeacherList[i];
                if (item != null)
                {
                    //Debug.WriteLine(item.ToString());
                    bool isFifthColumnTrue = false;
                    if (item.Status == "Definitief")
                    {
                        isFifthColumnTrue = true;
                    }
                    
                    if (isFifthColumnTrue)
                    {
                        Debug.WriteLine($"{item.Status} en {isFifthColumnTrue}");
                        DataGridColumn columnIndex = GradesStudentGrid.Columns[3];
                        DataGridCell cell = GetCell(e.Row, columnIndex);

                        if (cell != null)
                            if (cell != null)
                        {
                            cell.IsEnabled = false;
                            Debug.WriteLine($"{i} has been disabled");
                        }
                    }
                }
            }
        }
    }
}
