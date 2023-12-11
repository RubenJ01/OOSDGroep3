using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUpAdmin.Core.NewFolder;
using SmartUpAdmin.DataAccess.SQLServer.Dao;
using SmartUpAdmin.DataAccess.SQLServer.Model;
using SmartUpAdmin.WPF.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartUpAdmin.WPF
{
    public partial class AddSemester : Page
    {

        private static CourseDao courseDao = CourseDao.GetInstance();
        private static SemesterDao semesterDao = SemesterDao.GetInstance();
        private static SemesterCriteriaDao semesterCriteriaDao = SemesterCriteriaDao.GetInstance();
        private static SemesterCourseDao semesterCourseDao = SemesterCourseDao.GetInstance();
        private static SemesterAvailabilityDao semesterAvailabilityDao = SemesterAvailabilityDao.GetInstance();

        public AddSemester()
        {
            InitializeComponent();
            AddCoursesToDropdown();
            AddCriteria();
        }

        private void AddSemesterButtonClick(object sender, EventArgs e)
        {
            List<Field> fields = new List<Field>();
            Field nameField = new Field(NameField, 5, 20, new Regex("[%$#@!]\r\n"));
            Debug.WriteLine(nameField.GetText());
            nameField.AddErrorCheck(() => semesterDao.GetSemesterByName(NameField.Text) != null, "Een semester met deze naam bestaat al.");
            fields.Add(nameField);
            Field afkortingField = new Field(AfkortingField, 2, 5, new Regex("[%$#@!]\r\n"));
            fields.Add(afkortingField);
            Field beschrijvingField = new Field(BeschrijvingField, 5, 500, null);
            fields.Add(beschrijvingField);
            Field ecField = new Field(ECField, 1, 2, new Regex("\\D"));
            fields.Add(ecField);
            if (AllFieldsValid(fields))
            {
                string semesterName = nameField.GetText();
                List<SemesterAvailability> semesterAvailabilities = GetSemesterAvailabilities(semesterName);
                List<SemesterCriteria> semesterCriterias = GetSemesterCriterias(semesterName);
                List<SemesterCourse> semesterCourses = GetSemesterCourses(semesterName);
                Semester semester = new Semester(semesterName, afkortingField.GetText(), beschrijvingField.GetText(), Int32.Parse(ecField.GetText()));
                AddSemesterToDatabase(semester, semesterCriterias, semesterCourses, semesterAvailabilities);
                this.NavigationService.Navigate(new ViewSemester());
            }
        }

        private bool AllFieldsValid(List<Field> fields)
        {
            bool isValid = true;
            foreach (Field field in fields)
            {
                if (!field.Validate())
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        private List<SemesterAvailability> GetSemesterAvailabilities(string semesterName)
        {
            List<SemesterAvailability> semesterAvailabilities = new List<SemesterAvailability>();
            if (Semester1CheckBox.IsChecked.GetValueOrDefault() == true)
            {
                semesterAvailabilities.Add(new SemesterAvailability(semesterName, 1));
            }
            if (Semester2CheckBox.IsChecked.GetValueOrDefault() == true)
            {
                semesterAvailabilities.Add(new SemesterAvailability(semesterName, 2));
            }
            return semesterAvailabilities;
        }

        private List<SemesterCourse> GetSemesterCourses(string semesterName)
        {
            List<SemesterCourse> semesterCourses = new List<SemesterCourse>();
            foreach (object selectedItem in Courses.SelectedItems)
            {
                Course course = courseDao.GetCourseByCourseName(selectedItem.ToString());
                semesterCourses.Add(new SemesterCourse(semesterName, course.Name));
            }
            return semesterCourses;
        }

        private List<SemesterCriteria> GetSemesterCriterias(string semesterName)
        {
            ListBox? criteriaListBox = CriteriaField.Template.FindName("CriteriaListBox", CriteriaField) as ListBox;
            List<SemesterCriteria> semesterCriterias = new List<SemesterCriteria>();
            if (criteriaListBox != null)
            {
                foreach (string item in criteriaListBox.SelectedItems)
                {
                    semesterCriterias.Add(new SemesterCriteria(semesterName, item));
                }
            }
            return semesterCriterias;
        }

        private void AddSemesterToDatabase(Semester semester, List<SemesterCriteria> semesterCriterias,
            List<SemesterCourse> semesterCourses, List<SemesterAvailability> semesterAvailabilities)
        {
            try
            {
                semesterDao.AddSemester(semester);
                semesterCriterias.ForEach(semesterCriteria => semesterCriteriaDao.AddSemesterCriteria(semesterCriteria));
                semesterCourses.ForEach(semesterCourse => semesterCourseDao.AddSemesterCourse(semesterCourse));
                semesterAvailabilities.ForEach(semesterAvailability => semesterAvailabilityDao.AddSemesterAvailability(semesterAvailability));
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
