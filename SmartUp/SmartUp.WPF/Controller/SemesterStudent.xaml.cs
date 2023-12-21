using Microsoft.Data.SqlClient;
using SmartUp.Core.Constants;
using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using SmartUp.DataAccess.SQLServer.Util;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace SmartUp.UI
{
    public partial class SemesterStudent : Page
    {
        private static Semester? SelectedSemester { get; set; }
        private static Border SelectedCard {  get; set; }

        public SemesterStudent()
        {
            InitializeComponent();
            LoadDataAllSemester();
            LoadDatafollowedSemester();
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
            using (SqlConnection? connection = DatabaseConnection.GetConnection())

                try
                {
                    SelectedSemester = semester;
                    SelectedCard = card;
                    SemesterName.Text = semester.Name;
                    StringBuilder stringBuilder = new StringBuilder();

                    List<SemesterCourse> CriteriaCourses = SemesterCriteriaDao.GetInstance().GetSemesterCriteriaBySemester(connection ,semester);
                    List<string> CoursesInSemester = SemesterCourseDao.GetInstance().GetSemesterCoursesBySemesterName(connection ,semester.Name);

                    if (CriteriaCourses.Count > 0)
                    {
                        stringBuilder.Append($"Vakken verplicht gehaald:\n");
                        foreach (SemesterCourse Course in CriteriaCourses)
                        {
                            if(GradeDao.GetInstance().IsGradePassed(connection, Course.CourseName))
                            {
                                stringBuilder.Append($"  - {Course.CourseName} √\n");
                            }
                            else
                            {
                                stringBuilder.Append($"  - {Course.CourseName}\n");
                            }
                        }
                    }
                    if (CoursesInSemester.Count > 0)
                    {
                        stringBuilder.Append($"\nVakken in dit semester:\n");
                        foreach (string courseName in CoursesInSemester)
                        {
                            if (GradeDao.GetInstance().IsGradePassed(connection, courseName))
                            {
                                stringBuilder.Append($"  - {courseName} √\n");
                            }
                            else
                            {
                                stringBuilder.Append($"  - {courseName}\n");
                            }   
                        }
                    }

                    stringBuilder.Append($"\n\n{semester.Description}");
                    SemesterDescription.Text = stringBuilder.ToString();
                    card.Background = Brushes.DarkGray;


                    if (!SemesterRegistrationDao.GetInstance().IsEnrolledForSemesterByStudentId(connection, Constants.STUDENT_ID, semester) && IsSemesterCriteriaMet(connection, CriteriaCourses) == true)
                    {
                        EnrollButton.IsEnabled = true;
                    }
                    else
                    {
                        EnrollButton.IsEnabled = false;
                    }

                    if (SemesterRegistrationDao.GetInstance().IsEnrolledForSemesterByStudentId(connection ,Constants.STUDENT_ID, semester) && GradeDao.GetInstance().HasObtainedGrade(connection, SelectedSemester.Name))
                    {
                        UnsubscribeButton.IsEnabled = true;
                    }
                    else
                    {
                        UnsubscribeButton.IsEnabled = false;
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

        private void EnrollForSemester(object sender, RoutedEventArgs eventArgs)
        {
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
                try
                {
                    SemesterRegistrationDao.UnsubscribeFromSemesterByStudentId(connection, Constants.STUDENT_ID, SelectedSemester.Name);
                    UnsubscribeButton.IsEnabled = false;
                    EnrollButton.IsEnabled = true;
                    SubscribeCard();
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

        private void UnsubscribeFromSemester(object sender, RoutedEventArgs eventArgs)
        {
            using (SqlConnection? connection = DatabaseConnection.GetConnection())
                try
                {
                    SemesterRegistrationDao.UnsubscribeFromSemesterByStudentId(connection, Constants.STUDENT_ID, SelectedSemester.Name);
                    UnsubscribeButton.IsEnabled = false;
                    EnrollButton.IsEnabled = true;
                    UnSubscribeCard();
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

        private void LoadDatafollowedSemester()
        {
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

        private void LoadDataAllSemester()
        {
            using (SqlConnection con = DatabaseConnection.GetConnection())
            {
                try
                {
                    foreach (Semester semester in SemesterDao.GetInstance().GetAllSemestersWithoutRegistration(con, Constants.STUDENT_ID))
                    {
                        AddSemesterBlock(semester);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in method {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                }
                finally
                {
                    DatabaseConnection.CloseConnection(con);
                }
            }
        }

        private bool IsSemesterCriteriaMet(SqlConnection connection, List<SemesterCourse> requiredCourses)
        {
            foreach (SemesterCourse course in requiredCourses)
            {
                if (!GradeDao.GetInstance().IsGradePassed(connection, course.CourseName)) return false;
            }
            return true;
        }

        private void SubscribeCard()
        {
            using (SqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    SemesterWrap.Children.Remove(SelectedCard);
                    AddSemesterFollowedBlock(SelectedSemester, SemesterCourseDao.GetInstance().GetPercentagePassed(connection, Constants.STUDENT_ID, SelectedCard.Name));
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

        private void UnSubscribeCard()
        {
            FollowedSemesterWrap.Children.Remove(SelectedCard);
            AddSemesterBlock(SelectedSemester);
        }
    }
}
