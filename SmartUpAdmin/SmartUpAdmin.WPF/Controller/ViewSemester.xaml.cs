﻿using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartUpAdmin.WPF.View
{
    public partial class ViewSemester : Page
    {
        private static Semester? SelectedSemester { get; set; }
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
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"EC nodig van propedeuse: {semester.RequiredCreditsFromP}\n\n");

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
            if (CoursesInSemster.Count > 0)
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

        }

        private void AddSemester(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddSemester());
        }
    }
}