using SmartUp.DataAccess.SQLServer.Dao;
using SmartUpAdmin.WPF;
using SmartUpAdmin.WPF.View;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartUp.WPF.View
{
    public partial class AddCourse : Page
    {

        private static CourseDao courseDao = CourseDao.GetInstance();

        public AddCourse()
        {
            InitializeComponent();
            AddSemestersToDropdown();
        }

        private void AddSemestersToDropdown()
        {
            List<string> semesterNames = SemesterDao.GetInstance().GetAllSemesterNames();
            Semesters.ItemsSource = semesterNames;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void AddCourseButtonClick(object sender, EventArgs e)
        {
            if (VerifyInput())
            {
                string courseName = NameField.Text;
                int ec = Int32.Parse(ECField.Text);
                string semesterName = Semesters.Text;

                CourseDao.GetInstance().AddNewCourse(courseName, ec);

                if (semesterName.Length > 0)
                {
                    CourseDao.GetInstance().AddCourseToSemester(courseName, semesterName);
                    this.NavigationService.Navigate(new ViewCourse());
                }

            }

        }

        private bool VerifyInput()
        {
            NameField.Background = Brushes.White;
            ECField.Background = Brushes.White;
            bool valid = true;
            if (string.IsNullOrWhiteSpace(NameField.Text))
            {
                valid = false;
                NameField.Background = Brushes.Red;
            }
            if (ECField.Text.Length == 0)
            {
                valid = false;
                ECField.Background = Brushes.Red;
            }
            return valid;
        }
    }
}
