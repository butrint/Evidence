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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Window
    {
        private static string idUser;
        public AdminPage(string idAdmin)
        {
            InitializeComponent();
            idUser = idAdmin;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void Users(object sender, RoutedEventArgs e)
        {
            InsertUsers eW = new InsertUsers();
            eW.Show();
        }



        private void Students(object sender, RoutedEventArgs e)
        {
            InsertStudents eW = new InsertStudents();
            eW.Show();
        }

        private void Rolet(object sender, RoutedEventArgs e)
        {
            RolesWindow eW = new RolesWindow();
            eW.Show();
        }



        private void Lendet(object sender, RoutedEventArgs e)
        {
            SubjectsWindow eW = new SubjectsWindow();
            eW.Show();
        }

        private void Evidenca(object sender, RoutedEventArgs e)
        {
            DekanisWindow eW = new DekanisWindow(idUser);
            eW.Show();
        }

        private void Fakultetet(object sender, RoutedEventArgs e)
        {
            FacultiesWindow eW = new FacultiesWindow();
            eW.Show();
        }

        private void Departamentet(object sender, RoutedEventArgs e)
        {
            DepartmentsWindow eW = new DepartmentsWindow();
            eW.Show();
        }

        private void Lush(object sender, RoutedEventArgs e)
        {
            LushWindow eW = new LushWindow();
            eW.Show();
        }

        private void Rreth(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aplikacionin e punuan: \n\nButrint Rashiti, \nJeton Selimi, \nYllka Emini dhe \nEda Agushi", "Rreth nesh");
        }

        private void Zevendesimi(object sender, RoutedEventArgs e)
        {
            InsertSubstition eW = new InsertSubstition();
            eW.Show();
        }

        private void Çkyçu(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            this.Close();
            if (!this.Activate())
                mw.Show();
        }
    }
}
