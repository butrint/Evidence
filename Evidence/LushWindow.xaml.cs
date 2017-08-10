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
    /// Interaction logic for LushWindow.xaml
    /// </summary>
    public partial class LushWindow : Window
    {
        public LushWindow()
        {
            InitializeComponent();
        }

        private void btnInsertLush_Click(object sender, RoutedEventArgs e)
        {
            //string ID = txtlush_id.Text.ToString();
            string lsh = txtlush.Text.ToString();

            string existLush = "SELECT lush_id FROM lush where lush=" + lsh;
            if (txtlush.Text == "" || txtlush.Text == null)
            {
                MessageBox.Show("Ju duhet ta plotesoni fushën", "Informate");
            }
            else
            if (Methods.existsInDBS(existLush))
                MessageBox.Show("Lloji i lëndës me këtë emër ekziston");
            else
            {

                string query = "insert into lush (lush) values('" + lsh + "'); ";

                string mesazhi = "Të dhënat u regjistrohen me sukses";
                Methods.updateOrInsertIntoTable(query, mesazhi);
            }
        }

        private void btnNewRecordLush_Click(object sender, RoutedEventArgs e)
        {
            txtlush.Clear();
        }
    }
}
