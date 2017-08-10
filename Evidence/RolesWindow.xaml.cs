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
    /// Interaction logic for RolesWindow.xaml
    /// </summary>
    public partial class RolesWindow : Window
    {
        public RolesWindow()
        {
            InitializeComponent();
        }

        private void btnInsertR_Click(object sender, RoutedEventArgs e)
        {

            //string ID = txtrole_id.Text.ToString();
            string rl = txtrole.Text.ToString();

            string existRole = "SELECT role_id FROM roles where role=" + rl;
            if (txtrole.Text == "" || txtrole.Text == null)
            {
                MessageBox.Show("Ju duhet ta plotesoni fushën", "Informate");
            }
            else
            if (Methods.existsInDBS(existRole))
                MessageBox.Show("Roli me këtë emër ekziston");
            else
            {

                string query = "insert into roles (role) values('" + rl + "'); ";

                string mesazhi = "Të dhënat u regjistrohen me sukses";
                Methods.updateOrInsertIntoTable(query, mesazhi);
            }
        }

        private void btnNewRecordRole_Click(object sender, RoutedEventArgs e)
        {
            txtrole.Clear();
        }
    }
}
