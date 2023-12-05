using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUpAdmin.DataAccess.SQLServer.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartUpAdmin.WPF
{
    public partial class AddSemester : Page
    {

        private static CourseDao courseDao = CourseDao.GetInstance();

        public AddSemester()
        {
            InitializeComponent();
            AddCoursesToDropdown();
            AddCriteria();
        }

        private void AddSemesterButtonClick(object sender, EventArgs e)
        {
            string semesterName = NameField.Text;
            string semesterAbbreviation = AfkortingField.Text;
            string semesterDescription = BeschrijvingField.Text;
            int semesterRequiredCreditsFromP = int.Parse(ECField.Text);
            List<SemesterCriteria> semesterCriterias = new List<SemesterCriteria>();
            List<SemesterCourse> semesterCourses = new List<SemesterCourse>();
            ListBox? criteriaListBox = CriteriaField.Template.FindName("CriteriaListBox", CriteriaField) as ListBox;
            foreach (object selectedItem in Courses.SelectedItems)
            {
                Course course = courseDao.GetCourseByCourseName(selectedItem.ToString());
                semesterCourses.Add(new SemesterCourse(semesterName, course.Name));
            }
            if (criteriaListBox != null)
            {
                foreach (string item in criteriaListBox.SelectedItems)
                {
                    semesterCriterias.Add(new SemesterCriteria(semesterName, item));
                }
            }
            Semester semester = new Semester(semesterName, semesterAbbreviation, semesterDescription, semesterRequiredCreditsFromP);
            try
            {
                SemesterDao.GetInstance().AddSemester(semester);
                semesterCriterias.ForEach(semesterCriteria => SemesterCriteriaDao.GetInstance().AddSemesterCriteria(semesterCriteria));
                semesterCourses.ForEach(semesterCourse => SemesterCourseDao.GetInstance().AddSemesterCourse(semesterCourse));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message} {ex.Source.ToUpper()}");
            }
        }

        private void AddCoursesToDropdown()
        {
            Courses.ItemsSource = courseDao.GetAllCourseNames();
        }

        private void AddCriteria()
        {
            courseDao.GetAllCourseNames().ForEach(courseName => CriteriaField.Items.Add(courseName));
        }

        private void OnCourseSelected(object sender, SelectionChangedEventArgs e)
        {
            int totaleEc = 0;
            
            foreach (object selectedItem in Courses.SelectedItems)
            {
                Course course = courseDao.GetCourseByCourseName(selectedItem.ToString());
                totaleEc += course.Credits;
            }
            GeselecteerdeEC.Text = $"Geselecteerde EC: {totaleEc}";
        }

        private void CriteriaSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void ListBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (CriteriaField != null)
                {
                    Point point = e.GetPosition(CriteriaField);

                    if (CriteriaField.InputHitTest(point) != null && !CriteriaField.InputHitTest(point).Equals(CriteriaField))
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Grid_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CriteriaField.IsDropDownOpen = false;
        }

        private void OnGridClick(object sender, RoutedEventArgs e)
        {
            CriteriaField.IsDropDownOpen = false;
        }

        private void CriteriaField_DropDownClosed(object sender, System.EventArgs e)
        {
            CriteriaField.SelectedIndex = -1;
        }
    }
}
