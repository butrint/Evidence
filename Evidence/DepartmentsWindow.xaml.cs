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
using System.Windows.Shapes;

namespace Evidence
{
    /// <summary>
    /// Interaction logic for DepartamentsWindow.xaml
    /// </summary>
    public partial class DepartmentsWindow : Window
    {
        public DepartmentsWindow()
        {
            InitializeComponent();
        }

        private void btnInsertDep_Click(object sender, RoutedEventArgs e)
        {
            //string ID = txtdepartment_id.Text.ToString();
            string dep = txtdepartment.Text.ToString();

            string existDepartment = "SELECT department_id FROM departments where department=" + dep;
            if (txtdepartment.Text == "" || txtdepartment.Text == null)
            {
                MessageBox.Show("Ju duhet ta plotesoni fushën", "Informate");
            }
            else
            if (Methods.existsInDBS(existDepartment))
                MessageBox.Show("Fakulteti me këtë emër ekziston");
            else
            {

                string query = "insert into departments (department) values('" + dep + "'); ";

                string mesazhi = "Të dhënat u regjistrohen me sukses";
                Methods.updateOrInsertIntoTable(query, mesazhi);
            }
        }

        private void btnNewRecordDep_Click(object sender, RoutedEventArgs e)
        {
            txtdepartment.Clear();
        }
    }
}
