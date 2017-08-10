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
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Evidence
{
    /// <summary>
    /// Interaction logic for InsertUsers.xaml
    /// </summary>
    public partial class InsertUsers : Window
    {
        public static ObservableCollection<user> users = new ObservableCollection<user>();
        public InsertUsers()
        {

            InitializeComponent();

            string sql = "SELECT * FROM users ORDER BY prof_id";

            users = Methods.getUsers(sql);
            dGrid.ItemsSource = users;
            dtPickerBday.SelectedDateFormat = DatePickerFormat.Long;

            string query = "SELECT * FROM roles";
            List<role> roles = new List<role>();
            roles = Methods.getRoles(query);

            cmbrole_id.ItemsSource = roles;
            cmbrole_id.SelectedValuePath = "Role_ID";
            cmbrole_id.DisplayMemberPath = "Role";

        }

        string myConnectionString = "Data Source=localhost;Initial Catalog=vijueshmeria;User ID=root;Password=";

        //private void UserValidate()
        //{
        //    if ((txtprof_id.Text == "" || txtprof_id.Text == null) || (txtusername.Text == "" || txtusername.Text == null)
        //            || (txtpassword.Text == "" || txtpassword.Text == null) || (txtname.Text == "" || txtname.Text == null)
        //           || (txtsurname.Text == "" || txtsurname.Text == null) || (dtPickerBday.Text == "" || dtPickerBday.Text == null))
        //    {
        //        MessageBox.Show("Ju duhet t'i plotesoni fushat", "Informate");
        //        return;
        //    }
        //}

        private void btnInsertU_Click(object sender, RoutedEventArgs e)
        {
            // e merr nese ekziston nje user me ate num_id
            List<dynamic> lst = new List<dynamic>();
            user user = new user();
            string pid = txtprof_id.Text.ToString();
            string usr = txtusername.Text.ToString();
            string pass = txtpassword.Text.ToString();
            string passMD5 = Methods.Encode(pass);
            string nm = txtname.Text.ToString();
            string sn = txtsurname.Text.ToString();
            //string bd = dtPickerBday.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            DateTime bd = Convert.ToDateTime(dtPickerBday.SelectedDate);
            if ((txtprof_id.Text == "" || txtprof_id.Text == null) || (txtusername.Text == "" || txtusername.Text == null)
              || (txtpassword.Text == "" || txtpassword.Text == null) || (txtname.Text == "" || txtname.Text == null)
             || (txtsurname.Text == "" || txtsurname.Text == null) || (dtPickerBday.Text == "" || dtPickerBday.Text == null))
            {
                MessageBox.Show("Ju duhet t'i plotesoni fushat", "Informate");
                return;
            }
            int rid = (int)cmbrole_id.SelectedValue;
            user.insertUser(pid, usr, passMD5, nm, sn, Convert.ToDateTime(bd), rid);
            string existsProf_id = "SELECT prof_id FROM users WHERE prof_id=" + pid;
            if (Methods.existsInDBS(existsProf_id))
                MessageBox.Show("Profesori me kete id ekziston");
            else
            {

                string query = "INSERT INTO users(prof_id, username, password, name, surname, birthday, role_id) values('" + pid + "', '" + usr + "', '" + passMD5 + "', '" + nm + "', '" + sn + "', '" + bd + "', '" + rid + "'); ";
                string mesazhi = "Të dhënat u regjistrohen me sukses";
                Methods.updateOrInsertIntoTable(query, mesazhi);
                user.setID();
                user.setRole();
                users.Add(user);

            }
        }

        private void btnUpdateU_Click(object sender, RoutedEventArgs e)
        {

            user user = (user)dGrid.SelectedItem;
            string ID = txtuser_id.Text.ToString();
            string pid = txtprof_id.Text.ToString();
            string usr = txtusername.Text.ToString();
            string pass = txtpassword.Text.ToString();
            string passMD5 = Methods.Encode(pass);
            string nm = txtname.Text.ToString();
            string sn = txtsurname.Text.ToString();
            string bd = dtPickerBday.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            int rid = (int)cmbrole_id.SelectedValue;
            string query = "update users set prof_id='" + pid + "', username ='" + usr + "', name='" + nm + "', surname='" + sn + "', birthday='" + bd + "', role_id='" + rid + "' WHERE user_id='" + ID + "'";
            string mesazhi = "Të dhënat u modifikuan me sukses";
            Methods.updateOrInsertIntoTable(query, mesazhi);
            user.updateUser(pid, usr, nm, sn, Convert.ToDateTime(bd), rid);


        }

        private void btnDeleteU_Click(object sender, RoutedEventArgs e)
        {
            user usr = (user)dGrid.SelectedItem;
            string ID = txtuser_id.Text.ToString();

            string query = "delete from users where user_id='" + ID + "';";
            string mesazhi = "Të dhënat u fshiten me sukses";
            Methods.updateOrInsertIntoTable(query, mesazhi);
            users.Remove(usr);
        }

        private void btnClearU_Click(object sender, RoutedEventArgs e)
        {

            txtuser_id.Clear();
            txtprof_id.Clear();
            txtusername.Clear();
            txtpassword.Clear();
            txtname.Clear();
            txtsurname.Clear();
            dtPickerBday.Text = "";
            cmbrole_id.Text = "";



        }


        private void btnSearchUsers_Click(object sender, RoutedEventArgs e)
        {
            string filterText = txtSearchU.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(dGrid.ItemsSource);

            if (!string.IsNullOrEmpty(filterText))
            {
                cv.Filter = o =>
                {
                    /* change to get data row value */
                    user u = o as user;
                    return (u.Prof_ID.ToUpper().StartsWith(filterText.ToUpper()));
                    /* end change to get data row value */
                };
            }
            else
            {
                cv.Filter = o =>
                {
                    /* change to get data row value */
                    user u = o as user;
                    return true;
                    /* end change to get data row value */
                };
            }
        }
    }
}

