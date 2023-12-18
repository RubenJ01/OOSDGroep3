using Microsoft.Data.SqlClient;
using SmartUp.Core.Constants;
using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SmartUp.UI
{
    public partial class SemesterStudent : Page
    {
        private int CreditsFromP = StudentDao.GetInstance().GetCreditsFromPByStudentID(Constants.STUDENT_ID);
        private static Semester? SelectedSemester { get; set; }

        public SemesterStudent()
        {
            InitializeComponent();
            foreach (Semester semester in SemesterDao.GetInstance().GetAllSemesters())
            {
                if (semester.RequiredCreditsFromP <= CreditsFromP && StudentMeetsSemesterCriteria(Constants.STUDENT_ID, semester))
                {
                    AddSemesterBlock(semester);
                }
            }
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    foreach (Semester semester in SemesterDao.GetInstance().GetAllSemestersWithRegistration(connection, Constants.STUDENT_ID))
                    {
                        AddSemesterFollowedBlock(semester, SemesterCourseDao.GetInstance().GetPercentagePassed(connection, Constants.STUDENT_ID, semester.Name));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
                finally
                {
                    DatabaseConnection.CloseConnection(connection);
                }
            }

        }

        public void AddSemesterBlock(Semester semester)
        {
            Border card = new Border();
            card.CornerRadius = new CornerRadius(20, 20, 20, 20);
            card.Background = Brushes.Gray;
            card.Margin = new Thickness(5);
            card.Width = 150;
            card.Height = 80;

            Grid cardGrid = new Grid();
            RowDefinition rowDefinition0 = new RowDefinition();
            RowDefinition rowDefinition1 = new RowDefinition();
            cardGrid.RowDefinitions.Add(rowDefinition0);
            cardGrid.RowDefinitions.Add(rowDefinition1);

            TextBlock semesterName = new TextBlock();
            semesterName.Text = semester.Abbreviation;
            semesterName.VerticalAlignment = VerticalAlignment.Center;
            semesterName.HorizontalAlignment = HorizontalAlignment.Center;
            semesterName.FontSize = 20;
            semesterName.FontWeight = FontWeights.Bold;
            semesterName.Foreground = Brushes.White;
            Grid.SetRow(semesterName, 0);

            Ellipse circle = new Ellipse();
            circle.Height = 30;
            circle.Width = 30;
            circle.Fill = Brushes.White;
            circle.HorizontalAlignment = HorizontalAlignment.Right;
            circle.Margin = new Thickness(0, 0, 10, 0);
            Grid.SetRow(circle, 1);

            TextBlock informationI = new TextBlock();
            informationI.Padding = new Thickness(0, 0, 22, 0);

            informationI.Text = "i";
            informationI.FontWeight = FontWeights.Bold;
            informationI.Foreground = Brushes.Black;
            informationI.FontSize = 15;
            informationI.HorizontalAlignment = HorizontalAlignment.Right;
            informationI.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(informationI, 1);

            cardGrid.Children.Add(circle);
            cardGrid.Children.Add(informationI);
            cardGrid.Children.Add(semesterName);

            card.Child = cardGrid;

            SemesterWrap.Children.Add(card);

            card.PreviewMouseDown += (sender, e) => CardMouseDown(semester, card);
            SemesterWrap.MouseDown += (sender, e) => SemesterWrapMouseDown(card);
        }

        public void AddSemesterFollowedBlock(Semester semester, Decimal percentagePassed)
        {
            Border card = new Border();
            card.CornerRadius = new CornerRadius(20, 20, 20, 20);
            card.Background = Brushes.Gray;
            card.Margin = new Thickness(5);
            card.Width = 150;
            card.Height = 80;

            Grid cardGrid = new Grid();
            RowDefinition rowDefinition0 = new RowDefinition();
            RowDefinition rowDefinition1 = new RowDefinition();
            cardGrid.RowDefinitions.Add(rowDefinition0);
            cardGrid.RowDefinitions.Add(rowDefinition1);

            TextBlock semesterName = new TextBlock();
            semesterName.Text = $"{semester.Abbreviation}";
            semesterName.VerticalAlignment = VerticalAlignment.Center;
            semesterName.HorizontalAlignment = HorizontalAlignment.Center;
            semesterName.FontSize = 20;
            semesterName.FontWeight = FontWeights.Bold;
            semesterName.Foreground = Brushes.White;
            Grid.SetRow(semesterName, 0);

            ProgressBar progress = new ProgressBar();
            progress.Minimum = 0;
            progress.Maximum = 100;
            progress.Value = (int)percentagePassed;
            progress.Width = 80;
            progress.Height = 12;
            progress.ToolTip = $"{percentagePassed}%";
            progress.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7F8FAF");
            progress.Margin = new Thickness(15, 0, 0, 0);
            progress.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetRow(progress, 1);

            Ellipse circle = new Ellipse();
            circle.Height = 30;
            circle.Width = 30;
            circle.Fill = Brushes.White;
            circle.HorizontalAlignment = HorizontalAlignment.Right;
            circle.Margin = new Thickness(0, 0, 10, 0);
            circle.ToolTip = $"{semester.Name}";
            Grid.SetRow(circle, 1);

            TextBlock informationI = new TextBlock();
            informationI.ToolTip = $"{semester.Name}";
            informationI.Padding = new Thickness(0, 0, 22, 0);
            informationI.Text = "i";
            informationI.FontWeight = FontWeights.Bold;
            informationI.Foreground = Brushes.Black;
            informationI.FontSize = 15;
            informationI.HorizontalAlignment = HorizontalAlignment.Right;
            informationI.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(informationI, 1);

            cardGrid.Children.Add(circle);
            cardGrid.Children.Add(progress);
            cardGrid.Children.Add(informationI);
            cardGrid.Children.Add(semesterName);

            card.Child = cardGrid;

            FollowedSemesterWrap.Children.Add(card);

            card.PreviewMouseDown += (sender, e) => CardMouseDown(semester, card);
            FollowedSemesterWrap.MouseDown += (sender, e) => SemesterWrapMouseDown(card);
        }



        private static void SemesterWrapMouseDown(Border card)
        {
            if (!card.IsMouseOver)
            {
                card.Background = Brushes.Gray;
            }
        }

        private void CardMouseDown(Semester semester, Border card)
        {
            SelectedSemester = semester;
            SemesterName.Text = semester.Name;
            StringBuilder stringBuilder = new StringBuilder();

            List<SemesterCourse> CriteriaCourses = SemesterCriteriaDao.GetInstance().GetSemesterCriteriaBySemester(semester);
            List<string> CoursesInSemster = SemesterCourseDao.GetInstance().GetSemesterCoursesBySemesterName(semester.Name);

            if (CriteriaCourses.Count > 0)
            {
                stringBuilder.Append($"Vakken verplicht gehaald:\n");
                foreach (SemesterCourse Course in CriteriaCourses)
                {
                    stringBuilder.Append($"  - {Course.CourseName}\n");
                }
            }
            if (CoursesInSemster != null)
            {
                stringBuilder.Append($"\nVakken in dit semester:\n");
                foreach (string courseName in CoursesInSemster)
                {
                    stringBuilder.Append($"  - {courseName}\n");
                }
            }

            stringBuilder.Append($"\n\n{semester.Description}");
            SemesterDescription.Text = stringBuilder.ToString();
            card.Background = Brushes.DarkGray;


            if (SemesterRegistrationDao.GetInstance().IsEnrolledForSemesterByStudentId(Constants.STUDENT_ID) == true)
            {
                EnrollButton.IsEnabled = false;
            }
            else
            {
                EnrollButton.IsEnabled = true;
            }

            if (SemesterRegistrationDao.GetInstance().IsEnrolledForSemesterByStudentId(Constants.STUDENT_ID, semester))
            {
                UnsubscribeButton.IsEnabled = true;
            }
            else
            {
                UnsubscribeButton.IsEnabled = false;
            }
        }

        private void EnrollForSemester(object sender, RoutedEventArgs eventArgs)
        {
            if (SelectedSemester.Name != null)
            {
                SemesterRegistrationDao.CreateRegistrationByStudentIdBasedOnSemester(Constants.STUDENT_ID, SelectedSemester.Name);
                UnsubscribeButton.IsEnabled = true;
                EnrollButton.IsEnabled = false;
            }
        }

        private void UnsubscribeFromSemester(object sender, RoutedEventArgs eventArgs)
        {
            SemesterRegistrationDao.UnsubscribeFromSemesterByStudentId(Constants.STUDENT_ID);
            UnsubscribeButton.IsEnabled = false;
            EnrollButton.IsEnabled = true;
        }

        private bool StudentMeetsSemesterCriteria(string studentID, Semester semester)
        {
            List<SemesterCourse> semestersCriteria = SemesterCriteriaDao.GetInstance().GetSemesterCriteriaBySemester(semester);
            Dictionary<string, decimal> gradesStudent = GradeDao.GetInstance().ReturnGradesAsDictionaryByStudentId(studentID);
            foreach (SemesterCourse criteria in semestersCriteria)
            {
                if (!gradesStudent.ContainsKey(criteria.CourseName) || Convert.ToDecimal(gradesStudent[criteria.CourseName]) < 5.5M)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
