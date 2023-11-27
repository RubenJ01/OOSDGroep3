using Microsoft.Data.SqlClient;
using SmartUp.DataAccess.SQLServer.DBO.StudentGrade;
using SmartUp.WPF.Model;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SmartUp.UI
{
    public partial class GradeStudent : Page
    {
        public GradeStudent()
        {
            InitializeComponent();
            int studentID = 1;
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\abeam\\OneDrive\\Documents\\local.SmartUpDB.mdf;Integrated Security=True;Connect Timeout=30";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT grade.grade, grade.isDefinitive, grade.date, grade.courseName, course.credits FROM grade JOIN course ON course.name=grade.courseName WHERE grade.studentId = {studentID}";
                GradesModel.grades = StudentGradeList.GetStudentGrades(connection, sql);
                foreach (GradeStudentModel grade in GradesModel.grades)
                {
                    AddGradeView(grade);
                }
            }
        }
        public void AddGradeView(GradeStudentModel model)
        {
            Debug.WriteLine("addGradeView");
            Grid grid = new Grid();
            grid.Height = 100;
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            grid.ColumnDefinitions.Add(colDef1);
            grid.ColumnDefinitions.Add(colDef2);
            grid.ColumnDefinitions.Add(colDef3);
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);

            TextBlock Course = new TextBlock();
            Course.Text = model.CourseName;
            Course.FontSize = 20;
            Course.HorizontalAlignment = HorizontalAlignment.Center;
            Course.VerticalAlignment = VerticalAlignment.Center;
            Course.FontWeight = FontWeights.SemiBold;
            Grid.SetRow(Course, 0);
            Grid.SetColumn(Course, 0);

            TextBlock isDefinitive = new TextBlock();
            if (model.IsDefinitive)
            {

                isDefinitive.Text = "Definitief";
            }
            else
            {
                isDefinitive.Text = "Voorlopig";

            }
            isDefinitive.FontSize = 15;
            isDefinitive.HorizontalAlignment = HorizontalAlignment.Center;
            isDefinitive.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetRow(isDefinitive, 1);
            Grid.SetColumn(isDefinitive, 0);

            TextBlock credits = new TextBlock();
            credits.Text = $"{model.Credits} EC";
            credits.FontSize = 15;
            credits.HorizontalAlignment = HorizontalAlignment.Right;
            credits.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumn(credits, 1);
            Grid.SetRow(credits, 0);

            TextBlock grade = new TextBlock();
            grade.Text = $"{model.Grade}";
            grade.FontSize = 20;
            grade.HorizontalAlignment = HorizontalAlignment.Center;
            grade.VerticalAlignment = VerticalAlignment.Center;
            grade.FontWeight = FontWeights.SemiBold;
            Grid.SetColumn(grade, 2);
            Grid.SetRow(grade, 0);

            TextBlock date = new TextBlock();
            date.Text = $"{model.PublishedOn:dd-MM-yyyy}";
            date.FontSize = 15;
            date.HorizontalAlignment = HorizontalAlignment.Center;
            date.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumn(date, 2);
            Grid.SetRow(date, 1);

            grid.Children.Add(Course);
            grid.Children.Add(isDefinitive);
            grid.Children.Add(credits);
            grid.Children.Add(grade);
            grid.Children.Add(date);

            GradeOverview.Children.Add(grid);
        }
    }
}
