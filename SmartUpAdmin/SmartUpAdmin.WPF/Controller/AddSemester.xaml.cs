using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using SmartUpAdmin.Core.NewFolder;
using SmartUpAdmin.DataAccess.SQLServer.Dao;
using SmartUpAdmin.DataAccess.SQLServer.Model;
using SmartUpAdmin.WPF.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartUpAdmin.WPF
{
    public partial class AddSemester : Page
    {

        private static CourseDao courseDao = CourseDao.GetInstance();
        private static SemesterDao semesterDao = SemesterDao.GetInstance();
        private static SemesterCriteriaDao semesterCriteriaDao = SemesterCriteriaDao.GetInstance();
        private static SemesterCourseDao semesterCourseDao = SemesterCourseDao.GetInstance();
        private static SemesterAvailabilityDao semesterAvailabilityDao = SemesterAvailabilityDao.GetInstance();
        private static SemesterRequiredPercentageDao semesterRequiredPercentageDao = SemesterRequiredPercentageDao.GetInstance();
        private List<SemesterCriteria> semesterCriteriaOnSemester;
        private List<Semester> allSemesters = semesterDao.GetAllSemesters();
        private List<Course> allCourses = courseDao.GetAllCourses();
        private List<SemesterAvailability> semesterAvailabilities;
        private List<SemesterRequiredPercentage> requiredPercentages = new List<SemesterRequiredPercentage>();
        private List<SemesterRequiredPercentage> requiredPercentagesOld = new List<SemesterRequiredPercentage>();
        private List<SemesterCourse> semesterCourses = new List<SemesterCourse>();
        private readonly Semester? SelectedSemester;

        public AddSemester(Semester? selectedSemester)
        {
            InitializeComponent();
            AddCoursesToDropdown();
            AddCriteria();
            if (selectedSemester != null)
            {
                using (SqlConnection connection = DatabaseConnection.GetConnection())
                {
                    try
                    {
                        connection.Open();
                        SelectedSemester = selectedSemester;
                        requiredPercentages = semesterRequiredPercentageDao.GetAllSemesterRequiredPercentageBySemester(connection, SelectedSemester);
                        requiredPercentagesOld = semesterRequiredPercentageDao.GetAllSemesterRequiredPercentageBySemester(connection, SelectedSemester);
                        foreach (SemesterRequiredPercentage requiredPercentage in requiredPercentages)
                        {
                            CreateSemesterPercentageGrid(requiredPercentage);
                        }
                        semesterCriteriaOnSemester = semesterCriteriaDao.GetSemesterCriteriaBySemester(connection, SelectedSemester);
                        semesterCourses = semesterCourseDao.GetSemesterCoursesBySemesterName(connection, SelectedSemester.Name);
                        semesterAvailabilities = semesterAvailabilityDao.GetSemesterAvailabiltyBySemester(connection, SelectedSemester);
                        NameField.Text = selectedSemester.Name;
                        AfkortingField.Text = selectedSemester.Abbreviation;
                        BeschrijvingField.Text = selectedSemester.Description;

                        foreach (SemesterCourse semesterCourse in semesterCourses)
                        {
                            Courses.SelectedItems.Add(semesterCourse.CourseName);
                        }
                        foreach (SemesterAvailability semesterAvailability in semesterAvailabilities)
                        {
                            if (semesterAvailability != null && semesterAvailability.AvailableInSemester == 1)
                            {
                                Semester1CheckBox.IsChecked = true;
                            }
                            if (semesterAvailability != null && semesterAvailability.AvailableInSemester == 2)
                            {
                                Semester1CheckBox.IsChecked = true;
                            }
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
        }

        private void AddSemesterPercentage(object sender, RoutedEventArgs e)
        {
            SemesterRequiredPercentage semesterRequiredPercentage = new SemesterRequiredPercentage();
            requiredPercentages.Add(semesterRequiredPercentage);
            CreateSemesterPercentageGrid(semesterRequiredPercentage);
        }

        private void CreateSemesterPercentageGrid(SemesterRequiredPercentage semesterRequiredPercentage)
        {
            Grid semesterPercentageGrid = new Grid();

            ColumnDefinition columnDefinition0 = new ColumnDefinition();
            columnDefinition0.Width = new GridLength(2, GridUnitType.Star);
            ColumnDefinition columnDefinition1 = new ColumnDefinition();
            columnDefinition1.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition columnDefinition2 = new ColumnDefinition();
            columnDefinition2.Width = new GridLength(3, GridUnitType.Star);
            ColumnDefinition columnDefinition3 = new ColumnDefinition();
            columnDefinition3.Width = new GridLength(1, GridUnitType.Star);

            semesterPercentageGrid.ColumnDefinitions.Add(columnDefinition0);
            semesterPercentageGrid.ColumnDefinitions.Add(columnDefinition1);
            semesterPercentageGrid.ColumnDefinitions.Add(columnDefinition2);
            semesterPercentageGrid.ColumnDefinitions.Add(columnDefinition3);

            RowDefinition rowDefinition0 = new RowDefinition();
            rowDefinition0.Height = new GridLength(100);
            semesterPercentageGrid.RowDefinitions.Add(rowDefinition0);

            ComboBox comboBoxPercentage = new ComboBox();
            comboBoxPercentage.Margin = new Thickness(20);
            comboBoxPercentage.FontSize = 24;
            comboBoxPercentage.HorizontalContentAlignment = HorizontalAlignment.Left;
            comboBoxPercentage.VerticalContentAlignment = VerticalAlignment.Center;
            comboBoxPercentage.Name = "ComboBoxPercentage";
            Grid.SetRow(comboBoxPercentage, 0);
            Grid.SetColumn(comboBoxPercentage, 0);

            if (semesterRequiredPercentage.RequiredSemesterName != null)
            {
                comboBoxPercentage.SelectedItem = semesterRequiredPercentage.RequiredSemesterName;
            }

            comboBoxPercentage.ItemsSource = allSemesters;
            comboBoxPercentage.DisplayMemberPath = "Name";

            comboBoxPercentage.SelectionChanged += (sender, e) =>
            {
                Semester selectedSemester = comboBoxPercentage.SelectedItem as Semester;

                if (selectedSemester != null)
                {
                    requiredPercentages[requiredPercentages.IndexOf(semesterRequiredPercentage)].RequiredSemesterName = selectedSemester.Name;
                }
            };

            Slider percentageSlider = new Slider();
            percentageSlider.HorizontalAlignment = HorizontalAlignment.Center;
            percentageSlider.VerticalAlignment = VerticalAlignment.Center;
            percentageSlider.Width = 400;
            percentageSlider.Maximum = 100;
            percentageSlider.Minimum = 0;
            percentageSlider.SmallChange = 1;
            Grid.SetRow(percentageSlider, 0);
            Grid.SetColumn(percentageSlider, 2);

            TextBlock percentageTextblock = new TextBlock();
            percentageTextblock.FontSize = 24;
            percentageTextblock.Text = "0%";
            percentageTextblock.HorizontalAlignment = HorizontalAlignment.Center;
            percentageTextblock.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(percentageTextblock, 0);
            Grid.SetColumn(percentageTextblock, 1);

            if (semesterRequiredPercentage.RequiredPercentage > 0)
            {
                percentageSlider.Value = semesterRequiredPercentage.RequiredPercentage;
                percentageTextblock.Text = $"{semesterRequiredPercentage.RequiredPercentage:N0}%";
            }

            percentageSlider.ValueChanged += (sender, e) =>
            {
                percentageTextblock.Text = $"{percentageSlider.Value:N0}%";
                requiredPercentages[requiredPercentages.IndexOf(semesterRequiredPercentage)].RequiredPercentage = Convert.ToInt32(percentageSlider.Value);
            };

            TextBlock deleteButton = new TextBlock();
            deleteButton.Text = "X";
            deleteButton.Foreground = Brushes.Red;
            deleteButton.FontSize = 24;
            deleteButton.HorizontalAlignment = HorizontalAlignment.Center;
            deleteButton.VerticalAlignment = VerticalAlignment.Center;
            deleteButton.Name = "DeleteButton";
            Grid.SetRow(deleteButton, 0);
            Grid.SetColumn(deleteButton, 3);

            deleteButton.PreviewMouseDown += (sender, e) =>
            {
                StackPanelPercentage.Children.Remove(semesterPercentageGrid);
                requiredPercentages.Remove(semesterRequiredPercentage);
            };


            Border border = new Border();
            border.BorderThickness = new Thickness(1);
            border.BorderBrush = new BrushConverter().ConvertFromString("#4e4e4e") as Brush;
            border.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetRow(border, 0);
            Grid.SetColumnSpan(border, 4);

            semesterPercentageGrid.Children.Add(comboBoxPercentage);
            semesterPercentageGrid.Children.Add(percentageTextblock);
            semesterPercentageGrid.Children.Add(percentageSlider);
            semesterPercentageGrid.Children.Add(deleteButton);
            semesterPercentageGrid.Children.Add(border);

            StackPanelPercentage.Children.Insert(0, semesterPercentageGrid);

            if (semesterRequiredPercentage != null && semesterRequiredPercentage.RequiredSemesterName != null)
            {
                Semester selectedSemester = allSemesters.Find(semester => semester.Name.Equals(semesterRequiredPercentage.RequiredSemesterName));
                comboBoxPercentage.SelectedItem = selectedSemester;
            }
            if (semesterRequiredPercentage != null && semesterRequiredPercentage.RequiredPercentage != null)
            {
                percentageSlider.Value = semesterRequiredPercentage.RequiredPercentage;
            }
        }


        private void AddSemesterButtonClick(object sender, EventArgs e)
        {
            List<Field> fields = new List<Field>();
            Field nameField = new Field(NameField, 5, 64, new Regex("[%$#@!]\r\n"));
            if (SelectedSemester == null)
            {
                nameField.AddErrorCheck(() => semesterDao.GetSemesterByName(NameField.Text) != null, "Een semester met deze naam bestaat al.");
            }
            fields.Add(nameField);
            Field afkortingField = new Field(AfkortingField, 2, 5, new Regex("[%$#@!]\r\n"));
            fields.Add(afkortingField);
            Field beschrijvingField = new Field(BeschrijvingField, 5, 500, null);
            fields.Add(beschrijvingField);
            if (AllFieldsValid(fields))
            {
                string semesterName = nameField.GetText();
                List<SemesterAvailability> semesterAvailabilities = GetSemesterAvailabilities(semesterName);
                List<SemesterCriteria> semesterCriterias = GetSemesterCriterias(semesterName);
                List<SemesterCourse> semesterCourses = GetSemesterCourses(semesterName);
                Semester semester = new Semester(semesterName, afkortingField.GetText(), beschrijvingField.GetText());
                foreach (SemesterRequiredPercentage semesterRequiredPercentage in requiredPercentages)
                {
                    semesterRequiredPercentage.SemesterName = semester.Name;
                }
                if (SelectedSemester != null)
                {
                    UpdateSemesterInDatabase(semester, semesterCriterias, semesterCourses, semesterAvailabilities, requiredPercentages);
                }
                else
                {
                    AddSemesterToDatabase(semester, semesterCriterias, semesterCourses, semesterAvailabilities, requiredPercentages);
                }
                this.NavigationService.Navigate(new ViewSemester());
            }
        }

        private void UpdateSemesterInDatabase(Semester semester, List<SemesterCriteria> semesterCriterias, List<SemesterCourse> semesterCourses, List<SemesterAvailability> semesterAvailabilities, List<SemesterRequiredPercentage> semesterRequiredPercentages)
        {
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    semesterDao.UpdateSemester(connection, semester, SelectedSemester);
                    semesterCriteriaDao.UpdateSemesterCriteria(connection, semesterCriteriaOnSemester, semesterCriterias);
                    semesterCourseDao.UpdateSemesterCourse(connection, this.semesterCourses, semesterCourses);
                    semesterAvailabilityDao.UpdateSemesterAvailability(connection, this.semesterAvailabilities, semesterAvailabilities);
                    semesterRequiredPercentageDao.UpdateSemesterRequiredPercentage(connection, requiredPercentagesOld, semesterRequiredPercentages);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message} {ex.Source.ToUpper()}");
                }
                finally
                {
                    DatabaseConnection.CloseConnection(connection);
                }
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
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();

                    foreach (object selectedItem in Courses.SelectedItems)
                    {
                        Course course = courseDao.GetCourseByCourseName(connection, selectedItem.ToString());
                        semesterCourses.Add(new SemesterCourse(semesterName, course.Name));
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

        private void AddSemesterToDatabase(Semester semester, List<SemesterCriteria> semesterCriterias, List<SemesterCourse> semesterCourses, List<SemesterAvailability> semesterAvailabilities, List<SemesterRequiredPercentage> semesterRequiredPercentages)
        {
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();
                    semesterDao.AddSemester(connection, semester);
                    semesterCriterias.ForEach(semesterCriteria => semesterCriteriaDao.AddSemesterCriteria(connection, semesterCriteria));
                    semesterCourses.ForEach(semesterCourse => semesterCourseDao.AddSemesterCourse(connection, semesterCourse));
                    semesterAvailabilities.ForEach(semesterAvailability => semesterAvailabilityDao.AddSemesterAvailability(connection, semesterAvailability));
                    semesterRequiredPercentages.ForEach(semesterRequiredPercentage => semesterRequiredPercentageDao.AddSemesterRequiredPercentage(connection, semesterRequiredPercentage));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message} {ex.Source.ToUpper()}");
                }
                finally
                {
                    DatabaseConnection.CloseConnection(connection);
                }
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
            foreach (string selectedItem in Courses.SelectedItems)
            {
                Course? FindCourse = allCourses.Find(course => selectedItem.Equals(course.Name));
                Course course = FindCourse;
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

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CriteriaField.IsDropDownOpen = false;
        }

        private void OnGridClick(object sender, RoutedEventArgs e)
        {
            CriteriaField.IsDropDownOpen = false;
        }

        private void CriteriaField_DropDownClosed(object sender, EventArgs e)
        {
            CriteriaField.SelectedIndex = -1;
        }

        private void CancelAddingSemesterClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ViewSemester());
        }
    }
}