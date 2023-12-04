using SmartUp.DataAccess.SQLServer.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartUp.WPF.View
{
    public partial class AddCourse : Page
    {
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
    }
}
