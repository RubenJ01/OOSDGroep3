using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using SmartUpAdmin.Core.NewFolder;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System;
using System.Security.Cryptography;

namespace SmartUpAdmin.WPF.View
{
    public partial class ViewSemester : Page
    {
        private Semester? SelectedSemester { get; set; }
        private List<Field> fields { get; set; }
        public ViewSemester()
        {
            InitializeComponent();
            foreach (Semester semester in SemesterDao.GetInstance().GetAllSemesters())
            {
                AddSemesterBlock(semester);
            }
        }

        private void AddSemesterBlock(Semester semester)
        {
            Border card = new Border();
            card.CornerRadius = new CornerRadius(20, 20, 20, 20);
            card.Background = Brushes.Gray;
            card.Margin = new Thickness(5);
            card.Width = 245;
            card.Height = 120;

            Grid cardGrid = new Grid();
            RowDefinition rowDefinition0 = new RowDefinition();
            RowDefinition rowDefinition1 = new RowDefinition();
            cardGrid.RowDefinitions.Add(rowDefinition0);
            cardGrid.RowDefinitions.Add(rowDefinition1);

            TextBlock semesterName = new TextBlock();
            semesterName.Text = semester.Abbreviation;
            semesterName.VerticalAlignment = VerticalAlignment.Bottom;
            semesterName.HorizontalAlignment = HorizontalAlignment.Center;
            semesterName.FontSize = 35;
            semesterName.FontWeight = FontWeights.Bold;
            semesterName.Foreground = Brushes.White;
            Grid.SetRow(semesterName, 0);

            Ellipse circle = new Ellipse();
            circle.Height = 40;
            circle.Width = 40;
            circle.Fill = Brushes.White;
            circle.HorizontalAlignment = HorizontalAlignment.Right;
            circle.VerticalAlignment = VerticalAlignment.Bottom;
            circle.Margin = new Thickness(0, 0, 10, 10);
            circle.ToolTip = semester.Name;
            Grid.SetRow(circle, 1);

            TextBlock informationI = new TextBlock();
            informationI.Padding = new Thickness(0, 0, 26, 17);
            informationI.Text = "i";
            informationI.FontWeight = FontWeights.Bold;
            informationI.Foreground = Brushes.Black;
            informationI.FontSize = 20;
            informationI.HorizontalAlignment = HorizontalAlignment.Right;
            informationI.VerticalAlignment = VerticalAlignment.Bottom;
            informationI.ToolTip = semester.Name;
            Grid.SetRow(informationI, 1);

            cardGrid.Children.Add(circle);
            cardGrid.Children.Add(informationI);
            cardGrid.Children.Add(semesterName);

            card.Child = cardGrid;

            SemesterWrap.Children.Add(card);

            card.PreviewMouseDown += (sender, e) => CardMouseDown(semester, card);
            SemesterWrap.MouseDown += (sender, e) => SemesterWrapMouseDown(card);
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
            AddSemesterCourse.Visibility = Visibility.Visible;
            AddSemesterCourse.Click += (sender, e) => AddSemesterCourseClick();
            StringBuilder stringBuilder = BuildDescriptionString(semester);
            SemesterDescription.Text = stringBuilder.ToString();
            card.Background = Brushes.DarkGray;
        }

        private StringBuilder BuildDescriptionString(Semester semester)
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<SemesterCourse> CriteriaCourses = SemesterCriteriaDao.GetInstance().GetSemesterCriteriaBySemester(semester);
            List<string> CoursesInSemster = SemesterCourseDao.GetInstance().GetSemesterCoursesBySemesterName(semester.Name);
            double UpperMargin = 75;
            if (CriteriaCourses.Count > 0)
            {
                UpperMargin += CriteriaCourses.Count * 25 + 50;
            }
            AddSemesterCourse.Margin = new Thickness(0, UpperMargin, 40, 0);

            stringBuilder.Append($"Ingangseisen:\n");
            if (CriteriaCourses.Count > 0)
            {
                stringBuilder.Append($"   - Vakken\n");
                foreach (SemesterCourse Course in CriteriaCourses)
                {
                    stringBuilder.Append($"        * {Course.CourseName}\n");
                }
            }

            stringBuilder.Append($"\nVakken in dit semester:\n");

            if (CoursesInSemster.Count > 0)
            {
                foreach (string courseName in CoursesInSemster)
                {
                    stringBuilder.Append($"  - {courseName}\n");
                }
            }
            stringBuilder.Append($"\n\n{semester.Description}");
            return stringBuilder;
        }

        private void AddSemesterCourseClick()
        {
            SemesterDescription.Text = string.Empty;
            AddSemesterCourse.Visibility = Visibility.Hidden;
            AddSemesterCourseForm.Visibility = Visibility.Visible;
            SemesterName.Text = $"Vak toevoegen aan: {SelectedSemester.Abbreviation}";
            fields = new List<Field>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();

                    Field CourseNameField = new Field(CourseName, 5, 64, new Regex("[%$#@!]\r\n"));
                    //                                           Error is thrown here because the connection string has not been initialized when called when valadating
                    //                                                                                          vvvvvvvvv
                    CourseNameField.AddErrorCheck(() => SemesterCourseDao.GetInstance().GetSemesterCourseByName(connection, CourseNameField.GetText()) != null, "Een semestervak met deze naam bestaat al."); 
                    //                                                                                          ^^^^^^^^^
                    fields.Add(CourseNameField);
                    Field CourseECField = new Field(CourseEC, 1, 2, new Regex("\\D"));
                    fields.Add(CourseECField);
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

            foreach (Field field in fields)
            {
                Debug.WriteLine(field.ToString());
            }
        }

        private bool AllFieldsValid(List<Field> fields)
        {
            bool isValid = true;
            Debug.WriteLine("OpenTest");
            foreach (Field field in fields)
            {
                if (!field.Validate())
                {
                    isValid = false;
                }
            }
            Debug.WriteLine("CloseTest");
            return isValid;
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            AddSemesterCourseForm.Visibility = Visibility.Hidden;
            AddSemesterCourse.Visibility = Visibility.Visible;
            StringBuilder stringBuilder = BuildDescriptionString(SelectedSemester);
            SemesterDescription.Text = stringBuilder.ToString();
            CourseName.Text = string.Empty;
            CourseEC.Text = string.Empty;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                Debug.WriteLine("open");
                try
                {
                    Debug.WriteLine("try");
                    Debug.WriteLine(AllFieldsValid(fields));
                    if (AllFieldsValid(fields))
                    {
                        Debug.WriteLine("IF-statement passed");
                        Course course = new Course(fields[0].GetText(), Int32.Parse(fields[1].GetText()));
                        SemesterCourse semesterCourse = new SemesterCourse(SelectedSemester.Name, course.Name);
                        CourseDao.GetInstance().AddNewCourse(course.Name, course.Credits);
                        SemesterCourseDao.GetInstance().AddSemesterCourse(connection, semesterCourse);
                        AddSemesterCourseForm.Visibility = Visibility.Hidden;
                        AddSemesterCourse.Visibility = Visibility.Visible;
                        StringBuilder stringBuilder = BuildDescriptionString(SelectedSemester);
                        SemesterDescription.Text = stringBuilder.ToString();
                        CourseName.Text = string.Empty;
                        CourseEC.Text = string.Empty;
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
        private void AddSemester(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddSemester());
        }
    }
}
