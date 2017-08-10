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
using System.Net.Mail;
using System.Net;

namespace Evidence
{
    /// <summary>
    /// Interaction logic for InsertStudents.xaml
    /// </summary>

    public partial class InsertStudents : Window
    {
        public static ObservableCollection<student> students = new ObservableCollection<student>();
        public InsertStudents()
        {
            InitializeComponent();
            string sql = "SELECT * FROM students ORDER BY num_id";

            students = Methods.getStudents(sql);
            dGrid.ItemsSource = students;
            dtPickerBday.SelectedDateFormat = DatePickerFormat.Long;

            dtPickerBday.DisplayDate = DateTime.Today;
        }

        //string myConnectionString = "Data Source=localhost;Initial Catalog=vijueshmeria;User ID=root;Password=";

        //private void StudentValidate()
        //{
        //    if ((txtnum_id.Text == "" || txtnum_id.Text == null) || (txtname.Text == "" || txtname.Text == null)
        //            || (txtsurname.Text == "" || txtsurname.Text == null) || (txtemail.Text == "" || txtemail.Text == null)
        //            || (dtPickerBday.Text == "" || dtPickerBday.Text == null))
        //    {
        //        MessageBox.Show("Ju duhet t'i plotesoni fushat", "Informate");
        //        return;
        //    }
        //}

        private void btnInsertS_Click(object sender, RoutedEventArgs e)
        {

            // e merr nese ekziston nje student me ate num_id
            List<dynamic> lst = new List<dynamic>();
            student student = new student();
            string nid = txtnum_id.Text.ToString();
            string nm = txtname.Text.ToString();
            string sn = txtsurname.Text.ToString();
            string em = txtemail.Text.ToString();
            //string bd = dtPickerBday.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            DateTime bd = Convert.ToDateTime(dtPickerBday.SelectedDate);

            if (!(dtPickerBday.SelectedDate >= DateTime.Today))
            {

                if ((txtnum_id.Text == "" || txtnum_id.Text == null) || (txtname.Text == "" || txtname.Text == null)
                       || (txtsurname.Text == "" || txtsurname.Text == null) || (txtemail.Text == "" || txtemail.Text == null)
                       || (dtPickerBday.Text == "" || dtPickerBday.SelectedDate == null))
                {
                    MessageBox.Show("Ju duhet t'i plotesoni fushat", "Informate");
                    return;
                }

                student.updateStudent(nid, nm, sn, em, Convert.ToDateTime(bd));
                string existsNum_id = "SELECT `student_id` FROM `students` WHERE `num_id`='" + nid + "'";
                if (Methods.existsInDBS(existsNum_id))
                    MessageBox.Show("Studenti me kete id ekziston");
                else
                {
                    string query = "insert into students (num_id, name, surname, email, birthday) values('" + nid + "','" + nm + "','" + sn + "','" + em + "','" + bd + "'); ";

                    string mesazhi = "Të dhënat u regjistrohen me sukses";
                    Methods.updateOrInsertIntoTable(query, mesazhi);
                    student.setID();
                    students.Add(student);
                }
            }
            else
            {
                MessageBox.Show("Ju lutem zgjedhni date tjeter.");
            }

        }



        private void btnUpdateS_Click(object sender, RoutedEventArgs e)
        {
            student stud = (student)dGrid.SelectedItem;
            string ID = txtstudent_id.Text.ToString();
            string nid = txtnum_id.Text.ToString();
            string nm = txtname.Text.ToString();
            string sn = txtsurname.Text.ToString();
            string em = txtemail.Text.ToString();
            string bd = dtPickerBday.SelectedDate.Value.Date.ToString("yyyy-MM-dd");
            string query = "update students set num_id='" + nid + "', name='" + nm + "', surname='" + sn + "', email='" + em + "', birthday='" + bd + "' WHERE student_id='" + ID + "'";
            string mesazhi = "Të dhënat u modifikuan me sukses";
            Methods.updateOrInsertIntoTable(query, mesazhi);
            stud.updateStudent(nid, nm, sn, em, Convert.ToDateTime(bd));
        }


        private void btnDeleteS_Click(object sender, RoutedEventArgs e)
        {
            //string myConnectionString = "Data Source=localhost;Initial Catalog=vijueshmeria;User ID=root;Password=";
            student stud = (student)dGrid.SelectedItem;
            string ID = txtstudent_id.Text.ToString();

            string query = "delete from students where student_id='" + ID + "';";
            string mesazhi = "Të dhënat u fshiten me sukses";
            Methods.updateOrInsertIntoTable(query, mesazhi);
            students.Remove(stud);
        }



        private void dGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //DataGridRow row = e.Row;
            //row.Height = 30;

        }

        private void dGrid_Search_Load(object sender, EventArgs e)
        {
            searchData("");
        }


        public void searchData(string valueToSearch)
        {
            string query = "SELECT * FROM students WHERE CONCAT(`num_id`, `name`, `surname`, `birthday`) like '%" + valueToSearch + "%'";
            Methods.updateOrInsertIntoTable(query);

        }

        private void btnSearchS_Click(object sender, RoutedEventArgs e)
        {
            string valueToSearch = txtSearchS.Text.ToString();
            searchData(valueToSearch);

        }

        private void btnClearS_Click(object sender, RoutedEventArgs e)
        {
            txtstudent_id.Clear();
            txtnum_id.Clear();
            txtname.Clear();
            txtsurname.Clear();
            txtemail.Clear();
            dtPickerBday.Text = "";
        }

        private void btnSearchStudent_Click(object sender, RoutedEventArgs e)
        {
            string filterText = txtSearchS.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(dGrid.ItemsSource);

            if (!string.IsNullOrEmpty(filterText))
            {
                cv.Filter = o =>
                {
                    /* change to get data row value */
                    student s = o as student;
                    return (s.Num_ID.ToUpper().StartsWith(filterText.ToUpper()));

                    /* end change to get data row value */
                };
            }
            else
            {
                cv.Filter = o =>
                {
                    /* change to get data row value */
                    student s = o as student;
                    return true;
                    /* end change to get data row value */
                };
            }
        }

        private void btnInsertPass_Click(object sender, RoutedEventArgs e)
        {
            string pass = Methods.Encode(pBoxPassword.Password);
            student stud = students.FirstOrDefault<student>(c => c.Num_ID.Equals(txtnum_id.Text));
            //student stud = (student)dGrid.SelectedItem;
            if (stud != null)
            {
                List<dynamic> lastId = new List<dynamic>();
                string query = "INSERT INTO devicereg(password) VALUE('" + pass + "')";
                Methods.updateOrInsertIntoTable(query);
                query = "SELECT devicereg_id FROM devicereg ORDER BY devicereg_id DESC limit 1";
                lastId = Methods.selectFromDbs(query);
                query = "UPDATE students  SET devicereg_id =" + lastId[0] + " WHERE student_id = "+stud.ID;
                Methods.updateOrInsertIntoTable(query);
                
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("evidencaproject@gmail.com");
                    mail.To.Add(stud.Email);
                    mail.Subject = "Fjalekalimi per evidencen";
                    mail.Body = "Ky eshte fjalekalimi juaj: " + pBoxPassword.Password;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("evidencaproject@gmail.com", "Jimmy123");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
        }

        private void txtnum_id_TextChanged(object sender, TextChangedEventArgs e)
        {
            student stud = students.FirstOrDefault<student>(c => c.Num_ID.Equals(txtnum_id.Text));
            if (stud != null)
            {
                txtstudent_id.Text = stud.ID.ToString();
                txtname.Text = stud.Name;
                txtsurname.Text = stud.Surname;
                txtemail.Text = stud.Email;
                dtPickerBday.SelectedDate = stud.Birthday;
            }
            else
            {
                txtstudent_id.Clear();
                txtname.Clear();
                txtsurname.Clear();
                txtemail.Clear();
                dtPickerBday.SelectedDate = null;
                dtPickerBday.DisplayDate = DateTime.Today;
            }
        }
    }
}




