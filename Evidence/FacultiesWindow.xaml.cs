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
    /// Interaction logic for FacultiesWindow.xaml
    /// </summary>
    public partial class FacultiesWindow : Window
    {
        public FacultiesWindow()
        {
            InitializeComponent();
        }

        private void btnInsertFacult_Click(object sender, RoutedEventArgs e)
        {
            //string ID = txtfaculty_id.Text.ToString();
            string fc = txtfaculty.Text.ToString();

            string existFaculty = "SELECT faculty_id FROM faculties where faculty=" + fc;
            if (txtfaculty.Text == "" || txtfaculty.Text == null)
            {
                MessageBox.Show("Ju duhet ta plotesoni fushën", "Informate");
            }
            else
            if (Methods.existsInDBS(existFaculty))
                MessageBox.Show("Fakulteti me këtë emër ekziston");
            else
            {

                string query = "insert into faculties (faculty) values('" + fc + "'); ";

                string mesazhi = "Të dhënat u regjistrohen me sukses";
                Methods.updateOrInsertIntoTable(query, mesazhi);
            }
        }

        private void btnNewRecordFacult_Click(object sender, RoutedEventArgs e)
        {
            txtfaculty.Clear();
        }
    }
}
