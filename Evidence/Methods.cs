using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Evidence
{
    public static class Methods
    {
        private static string myConnectionString = "Data Source=localhost;Initial Catalog=vijueshmeria;User ID=root;Password=";
        //private static MySqlConnection myConnection = new MySqlConnection(myConnectionString);
        //private static MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();
        public static string nowInMysql(string query, DateTime now)
        {
            MySqlConnection myConnection = new MySqlConnection(myConnectionString);
            MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();

            myCommand.CommandText = query;
            try
            {
                myConnection.Open();
                MySqlDataReader myReader = myCommand.ExecuteReader();
                try
                {
                    while (myReader.Read())
                    {
                        now = myReader.GetDateTime(0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    myReader.Close();
                    myConnection.Close();
                }
            }
            catch { }
            return now.ToString("yyyy-mm-dd");
        }
        public static void fillCombo(ComboBox cmb, string query, string cmbFirstItem)
        {
            List<dbTablesIdName> tbName = new List<dbTablesIdName>();
            tbName.Add(new dbTablesIdName() { ID = -1, Name = cmbFirstItem });
            MySqlConnection myConnection = new MySqlConnection(myConnectionString);
            MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();
            myCommand.CommandText = query;
            try
            {
                myConnection.Open();
                MySqlDataReader myReader = myCommand.ExecuteReader();
                try
                {
                    while (myReader.Read())
                        tbName.Add(new dbTablesIdName() { ID = myReader.GetInt32(0), Name = myReader.GetString(1) });
                }
                finally
                {
                    myReader.Close();
                    myConnection.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Për momentin lidhja dështoi. Provoni më vonë");
                //MessageBox.Show(ex.Message);
            }
            tbName.OrderBy(i => i.ID);
            cmb.SelectedValuePath = "ID";
            cmb.DisplayMemberPath = "Name";
            cmb.ItemsSource = tbName;
        }

        public static void fillGridTodaySubsProfs(DataGrid dtg, string query)
        {
            List<scheduler> scheduler = new List<scheduler>();
            MySqlConnection myConnection = new MySqlConnection(myConnectionString);
            MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();
            myCommand.CommandText = query;
            try
            {
                myConnection.Open();
                MySqlDataReader myReader = myCommand.ExecuteReader();
                try
                {
                    int i = 0;
                    while (myReader.Read())
                    {
                        scheduler.Add(new scheduler() { schedule_ID = myReader.GetInt32(0), start_Time = myReader.GetDateTime(1), end_Time = myReader.GetDateTime(2), group_ID = myReader.GetInt32(3), hall_ID = myReader.GetInt32(4), lush_ID = myReader.GetInt32(5), user_ID = myReader.GetInt32(6), subject_ID = myReader.GetInt32(7) });
                        int sched_id = myReader.GetInt32(0);
                        GetFNamesFrmSched(scheduler[i], sched_id);
                        i++;
                    }
                }
                finally
                {
                    myReader.Close();
                    myConnection.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Për momentin lidhja dështoi. Provoni më vonë");
                //MessageBox.Show(ex.Message);
            }
            //tbName.OrderBy(i => i.ID);
            dtg.ItemsSource = scheduler;
        }

        public static void GetFNamesFrmSched(scheduler sch, int schedule_id)
        {
            string query = "CALL getFNamesFrmSched('" + schedule_id + "');";
            MySqlConnection myConnection = new MySqlConnection(myConnectionString);
            MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();
            myCommand.CommandText = query;

            try
            {
                myConnection.Open();
                MySqlDataReader myReader = myCommand.ExecuteReader();
                try
                {
                    while (myReader.Read())
                    {
                        sch.group = myReader.GetString(0);
                        sch.hall = myReader.GetString(1);
                        sch.lush = myReader.GetString(2);
                        sch.username = myReader.GetString(3);
                        sch.subject = myReader.GetString(4);
                    }
                    //tbName.Add(new dbTablesIdName() { ID = myReader.GetInt32(0), Name = myReader.GetString(1) });

                }
                finally
                {
                    myReader.Close();
                    myConnection.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Për momentin lidhja dështoi. Provoni më vonë");
                //MessageBox.Show(ex.Message);
            }
        }

        public static scheduler SubClosestToDate(string prof_ID)
        {
            string query = "SELECT * FROM scheduler sch WHERE start_time BETWEEN CURDATE() AND timestamp(DATE_ADD(NOW(), INTERVAL 30 MINUTE)) AND user_id='" + prof_ID+ "' ORDER BY start_time DESC LIMIT 1";
            
            scheduler sch = new scheduler();
            MySqlConnection myConnection = new MySqlConnection(myConnectionString);
            MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();
            myCommand.CommandText = query;

            try
            {
                myConnection.Open();
                MySqlDataReader myReader = myCommand.ExecuteReader();
                try
                {
                    while (myReader.Read())
                    {
                        sch.schedule_ID = myReader.GetInt32(0);
                        sch.start_Time = myReader.GetDateTime(1);
                        sch.end_Time = myReader.GetDateTime(2);
                        sch.group_ID = myReader.GetInt32(3);
                        sch.hall_ID = myReader.GetInt32(4);
                        sch.lush_ID = myReader.GetInt32(5);
                        sch.user_ID = myReader.GetInt32(6);
                        sch.subject_ID = myReader.GetInt32(7);
                    }
                        //tbName.Add(new dbTablesIdName() { ID = myReader.GetInt32(0), Name = myReader.GetString(1) });

                }
                finally
                {
                    myReader.Close();
                    myConnection.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Për momentin lidhja dështoi. Provoni më vonë");
                //MessageBox.Show(ex.Message);
            }
            GetFNamesFrmSched(sch, sch.schedule_ID);
            return sch;

        }

        public static void fillDGTextbox(DataGrid dtg, string query)
        {
            List<student> students = new List<student>();
            MySqlConnection myConnection = new MySqlConnection(myConnectionString);
            MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();
            myCommand.CommandText = query;
            try
            {
                myConnection.Open();
                MySqlDataReader myReader = myCommand.ExecuteReader();
                try
                {
                    while (myReader.Read())
                        students.Add(new student() { ID = myReader.GetInt32(0), Name = myReader.GetString(1), Surname = myReader.GetString(2) });
                }
                finally
                {
                    myReader.Close();
                    myConnection.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Për momentin lidhja dështoi. Provoni më vonë");
                //MessageBox.Show(ex.Message);
            }
            //tbName.OrderBy(i => i.ID);
            dtg.ItemsSource = students;
        }
    }

    public class dbTablesIdName
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public bool Present { get; set; }

    }

    public class scheduler
    {
        public int schedule_ID { get; set; }
        public DateTime start_Time { get; set; }
        public DateTime end_Time { get; set; }
        public int group_ID { get; set; }
        public string group { get; set; }
        public int hall_ID { get; set; }
        public string hall { get; set; }
        public int lush_ID { get; set; }
        public string lush { get; set; }
        public int user_ID { get; set; }
        public string username { get; set; }
        public int subject_ID { get; set; }
        public string subject { get; set; }
        public bool subActive { get; set; }
    }
}
