using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using SmartUpAdmin.Core.NewFolder;
using SmartUpAdmin.DataAccess.SQLServer.Model;


namespace SmartUpAdmin.WPF.View
{
    public partial class ViewSemester : Page
    {
        private Semester? SelectedSemester { get; set; }
        private List<Field> Fields { get; set; }
        private bool IsUpdate { get; set; }
        private Course? SelectedCourse { get; set; }

        public ViewSemester()
        {
            InitializeComponent();
            Fields = new List<Field>();
            IsUpdate = false;
            LoadSemesters();
        }


        private void LoadSemesters()
        {
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
            AddSemesterCourseForm.Visibility = Visibility.Hidden;
            CourseName.Text = string.Empty;
            CourseEC.Text = string.Empty;
            CourseName.BorderBrush = Brushes.Black;
            CourseEC.BorderBrush = Brushes.Black;
            AddSemesterCourse.Click += (sender, e) => AddSemesterCourseClick();
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    SemesterDescription = BuildDescriptionPanel(connection, SelectedSemester);
                    card.Background = Brushes.DarkGray;
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

        private StackPanel BuildDescriptionPanel(SqlConnection connection, Semester semester)
        {

            List<SemesterCriteria> criteriaCourses = SemesterCriteriaDao.GetInstance().GetSemesterCriteriaBySemester(connection, semester);
            List<SemesterCourse> coursesInSemester = SemesterCourseDao.GetInstance().GetSemesterCoursesBySemesterName(connection, semester.Name);

            double upperMargin = 75;
            if (criteriaCourses.Count > 0)
            {
                upperMargin += criteriaCourses.Count * 25 + 50;
            }

            AddSemesterCourse.Margin = new Thickness(0, upperMargin, 40, 0);

            StackPanel stackPanel = SemesterDescription;
            stackPanel.Children.Clear();

            AddTextBlock(stackPanel, $"Ingangseisen:", false, null);

            if (criteriaCourses.Count > 0)
            {
                AddTextBlock(stackPanel, $"   - Vakken", false, null);

                foreach (SemesterCriteria course in criteriaCourses)
                {
                    AddTextBlock(stackPanel, $"        * {course.CourseName}", false, null);
                }
            }

            AddTextBlock(stackPanel, $"\nVakken in dit semester:", false, null);

            if (coursesInSemester.Count > 0)
            {
                foreach (SemesterCourse course in coursesInSemester)
                {
                    AddTextBlock(stackPanel, $"  - {course.CourseName}", true, course);
                }
            }

            AddTextBlock(stackPanel, $"\n\n{semester.Description}", false, null);
            return stackPanel;
        }

        private void AddTextBlock(StackPanel stackPanel, string text, bool addEvent, SemesterCourse? course)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = 20;
            textBlock.TextWrapping = TextWrapping.Wrap;
            if (addEvent)
            {
                textBlock.MouseEnter += (sender, e) => TextBlock_MouseEnter(textBlock);
                textBlock.MouseLeave += (sender, e) => TextBlock_MouseLeave(textBlock);
                textBlock.PreviewMouseDown += (sender, e) =>
                {
                    using (SqlConnection connection = DatabaseConnection.GetConnection())
                    {
                        try
                        {
                            connection.Open();

                            TextBlock_MouseDown(CourseDao.GetInstance().GetCourseByCourseName(connection, course.CourseName));
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
                };
            }
            stackPanel.Children.Add(textBlock);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBlock_MouseDown(Course course)
        {
            SelectedCourse = course;
            IsUpdate = true;
            SemesterDescription.Children.Clear();
            AddSemesterCourse.Visibility = Visibility.Hidden;
            AddSemesterCourseForm.Visibility = Visibility.Visible;
            CourseName.Text = course.Name;
            CourseEC.Text = course.Credits.ToString();
            SemesterName.Text = $"Vak aanpassen: {course.Name}";
            Fields = new List<Field>();
            Field CourseNameField = new Field(CourseName, 5, 64, new Regex("[%$#@!]\r\n"));
            CourseNameField.AddErrorCheck(connection => SemesterCourseDao.GetInstance().GetSemesterCourseByName(connection, CourseNameField.GetText()) != null, "Een semestervak met deze naam bestaat al.");
            Fields.Add(CourseNameField);
            Field CourseECField = new Field(CourseEC, 1, 2, new Regex("\\D"));
            Fields.Add(CourseECField);
        }

        private void TextBlock_MouseEnter(TextBlock textBlock)
        {
            textBlock.TextDecorations = TextDecorations.Underline;
        }

        private void TextBlock_MouseLeave(TextBlock textBlock)
        {
            textBlock.TextDecorations = null;
        }

        private void AddSemesterCourseClick()
        {
            IsUpdate = false;
            SemesterDescription.Children.Clear();
            AddSemesterCourse.Visibility = Visibility.Hidden;
            AddSemesterCourseForm.Visibility = Visibility.Visible;
            SemesterName.Text = $"Vak toevoegen aan: {SelectedSemester.Abbreviation}";
            Fields = new List<Field>();

            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();

                    Field CourseNameField = new Field(CourseName, 5, 64, new Regex("[%$#@!]\r\n"));
                    CourseNameField.AddErrorCheck(connection => SemesterCourseDao.GetInstance().GetSemesterCourseByName(connection, CourseNameField.GetText()) != null, "Een semestervak met deze naam bestaat al.");
                    Fields.Add(CourseNameField);
                    Field CourseECField = new Field(CourseEC, 1, 2, new Regex("\\D"));
                    Fields.Add(CourseECField);
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

        private bool AllFieldsValid(List<Field> fields, SqlConnection connection)
        {
            return Fields.All(field => field.Validate(connection));
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            AddSemesterCourseForm.Visibility = Visibility.Hidden;
            AddSemesterCourse.Visibility = Visibility.Visible;
            SemesterName.Text = SelectedSemester.Name;
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                try
                {
                    SemesterDescription = BuildDescriptionPanel(connection, SelectedSemester);
                    CourseName.Text = string.Empty;
                    CourseEC.Text = string.Empty;
                    CourseName.BorderBrush = Brushes.Black;
                    CourseEC.BorderBrush = Brushes.Black;
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

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                connection.Open();
                try
                {
                    if (IsUpdate && SelectedCourse.Name.Equals(CourseName.Text) && SelectedCourse.Credits.Equals(Convert.ToInt32(CourseEC.Text)))
                    {
                        LoadSemesterDescriptionProperties(connection);
                    }
                    else if ((SelectedCourse.Name.Equals(CourseName.Text) || AllFieldsValid(Fields, connection)) && IsUpdate)
                    {
                        Course course = new Course(Fields[0].GetText(), Int32.Parse(Fields[1].GetText()));
                        CourseDao.GetInstance().UpdateCourse(connection, course, SelectedCourse);

                        LoadSemesterDescriptionProperties(connection);
                    }
                    else if (AllFieldsValid(Fields, connection) && !IsUpdate)
                    {
                        Course course = new Course(Fields[0].GetText(), Int32.Parse(Fields[1].GetText()));
                        SemesterCourse semesterCourse = new SemesterCourse(SelectedSemester.Name, course.Name);

                        CourseDao.GetInstance().AddNewCourse(connection, course.Name, course.Credits);
                        SemesterCourseDao.GetInstance().AddSemesterCourse(connection, semesterCourse);

                        LoadSemesterDescriptionProperties(connection);
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

        private void LoadSemesterDescriptionProperties(SqlConnection? connection)
        {
            AddSemesterCourseForm.Visibility = Visibility.Hidden;
            AddSemesterCourse.Visibility = Visibility.Visible;
            SemesterDescription = BuildDescriptionPanel(connection, SelectedSemester);
            SemesterName.Text = SelectedSemester.Name;
            CourseName.Text = string.Empty;
            CourseEC.Text = string.Empty;
            CourseName.BorderBrush = Brushes.Black;
            CourseEC.BorderBrush = Brushes.Black;
        }

        private void AddSemester(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddSemester());
        }
    }
}