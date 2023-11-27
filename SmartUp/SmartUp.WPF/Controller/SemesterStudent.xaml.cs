using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace SmartUp.UI
{
    public partial class SemesterStudent : Page
    {
        public SemesterStudent()
        {
            InitializeComponent();
            AddSemesterBlock("OOSDD");
            AddSemesterBlock("DBMS");
            AddSemesterBlock("AC");
            AddSemesterBlock("PVV");
            AddSemesterBlock("VVD");
            AddSemesterBlock("CDA");
            AddSemesterBlock("FVD");
            AddSemesterBlock("GL");
            AddSemesterBlock("BBB");
            AddSemesterBlock("NLP");
        }
        public void AddSemesterBlock(string semestername)
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
            semesterName.Text = semestername;
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
        }
    }
}
