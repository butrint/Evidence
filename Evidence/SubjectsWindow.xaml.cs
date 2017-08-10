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
    /// Interaction logic for SubjectsWindow.xaml
    /// </summary>
    public partial class SubjectsWindow : Window
    {
        public SubjectsWindow()
        {
            InitializeComponent();
        }

        private void btnInsertSub_Click(object sender, RoutedEventArgs e)
        {
            //string ID = txtsubject_id.Text.ToString();
            string sub = txtsubject.Text.ToString();

            string existSubject = "SELECT subject_id FROM subjects where subject=" + sub;
            if (txtsubject.Text == "" || txtsubject.Text == null)
            {
                MessageBox.Show("Ju duhet t'i plotesoni fushat", "Informate");
            }
            else
           if (Methods.existsInDBS(existSubject))
                MessageBox.Show("Lenda me kete emer ekziston");
            else
            {
                string query = "insert into subjects(subject) values('" + sub + "'); ";

                string mesazhi = "Të dhënat u regjistrohen me sukses";
                Methods.updateOrInsertIntoTable(query, mesazhi);
            }
        }

        private void btnNewRecordSub_Click(object sender, RoutedEventArgs e)
        {
            txtsubject.Clear();
        }
    }
}
