using SmartUp.Core.Constants;
using SmartUp.DataAccess.SQLServer.Dao;
using SmartUp.DataAccess.SQLServer.Model;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SmartUp.UI
{
    public partial class GradeStudent : Page
    {
        private static GradeDao gradeDao = GradeDao.GetInstance();
        public GradeStudent()
        {
            InitializeComponent();
            Dictionary<string, int> gradesAttempts = new Dictionary<string, int>();
            foreach (Grade grade in gradeDao.GetGradesByStudentId(Constants.STUDENT_ID))
            {
                if (!gradesAttempts.ContainsKey(grade.CourseName))
                {
                    gradesAttempts.Add(grade.CourseName, 1);
                }
                else
                {
                    gradesAttempts[grade.CourseName]++;
                }
            }

            foreach (KeyValuePair<string, int> grade in gradesAttempts)
            {
                if (grade.Value == 1)
                {
                    Grade gradeFirstAttempt = gradeDao.GetGradeByAttemptByCourseNameByStudentId(Constants.STUDENT_ID, grade.Key, grade.Value);
                    AddGradeView(gradeFirstAttempt);
                }
                else
                {
                    List<Grade> gradeAllAttempts = new List<Grade>
                    {
                        gradeDao.GetGradeByAttemptByCourseNameByStudentId(Constants.STUDENT_ID, grade.Key, grade.Value)
                    };
                    for (int i = 1; i <= grade.Value; i++)
                    {
                        gradeAllAttempts.Add(gradeDao.GetGradeByAttemptByCourseNameByStudentId(Constants.STUDENT_ID, grade.Key, i));
                    }
                    AddGradeView(gradeAllAttempts);
                }
            }
        }

        public void AddGradeView(Grade model)
        {
            Grid grid = new Grid();
            grid.Height = 120;
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            colDef1.Width = new GridLength(2, GridUnitType.Star);
            colDef2.Width = new GridLength(1, GridUnitType.Star);
            colDef3.Width = new GridLength(1, GridUnitType.Star);
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
            Course.TextWrapping = TextWrapping.Wrap;
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
            grade.Text = $"{model.GradeNumber}";
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

        public void AddGradeView(List<Grade> model)
        {
            Grid grid = new Grid();
            grid.Height = 120;
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            colDef1.Width = new GridLength(2, GridUnitType.Star);
            colDef2.Width = new GridLength(1, GridUnitType.Star);
            colDef3.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(colDef1);
            grid.ColumnDefinitions.Add(colDef2);
            grid.ColumnDefinitions.Add(colDef3);
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);

            TextBlock Course = new TextBlock();
            Course.Text = model[0].CourseName;
            Course.FontSize = 20;
            Course.HorizontalAlignment = HorizontalAlignment.Center;
            Course.VerticalAlignment = VerticalAlignment.Center;
            Course.FontWeight = FontWeights.SemiBold;
            Course.TextWrapping = TextWrapping.Wrap;
            Grid.SetRow(Course, 0);
            Grid.SetColumn(Course, 0);

            TextBlock isDefinitive = new TextBlock();
            if (model[0].IsDefinitive)
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
            credits.Text = $"{model[0].Credits} EC";
            credits.FontSize = 15;
            credits.HorizontalAlignment = HorizontalAlignment.Right;
            credits.VerticalAlignment = VerticalAlignment.Bottom;
            Grid.SetColumn(credits, 1);
            Grid.SetRow(credits, 0);

            ComboBox dropdown = new ComboBox();
            dropdown.IsEditable = false;
            dropdown.IsReadOnly = true;
            dropdown.HorizontalContentAlignment = HorizontalAlignment.Center;
            model.Reverse();
            foreach (Grade grade in model)
            {
                dropdown.Items.Add($"{grade}");
            };

            dropdown.DropDownOpened += (sender, e) =>
            {
                dropdown.Items.RemoveAt(model.Count - 1);
            };

            dropdown.DropDownClosed += (sender, e) =>
            {
                dropdown.Items.Insert(model.Count - 1, model[model.Count - 1]);
                dropdown.SelectedIndex = model.Count - 1;
            };

            dropdown.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
            {
                dropdown.SelectedIndex = model.Count - 1;
            };

            dropdown.SelectedIndex = model.Count - 1;
            dropdown.Width = 130;
            dropdown.FontWeight = FontWeights.SemiBold;
            dropdown.Margin = new Thickness(15);
            dropdown.FontSize = 20;
            Grid.SetColumn(dropdown, 2);
            Grid.SetRow(dropdown, 0);
            TextBlock date = new TextBlock();
            date.Text = $"{model[0].PublishedOn:dd-MM-yyyy}";
            date.FontSize = 15;
            date.HorizontalAlignment = HorizontalAlignment.Center;
            date.VerticalAlignment = VerticalAlignment.Top;
            Grid.SetColumn(date, 2);
            Grid.SetRow(date, 1);

            grid.Children.Add(Course);
            grid.Children.Add(isDefinitive);
            grid.Children.Add(credits);
            grid.Children.Add(dropdown);
            grid.Children.Add(date);

            GradeOverview.Children.Add(grid);
        }
    }
}
