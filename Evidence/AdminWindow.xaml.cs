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


namespace Evidence
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private static string myConnectionString = "Data Source=localhost;Initial Catalog=vijueshmeria;User ID=root;Password=";
        private static DateTime now = new DateTime();
        private static string today;
        private static string user_ID;

        public AdminWindow(string idAdmin)
        {
            // 2017-06-05
            InitializeComponent();
            user_ID = idAdmin;
            string query = "SELECT * FROM `scheduler` WHERE DATE_FORMAT(start_time, '%Y-%m-%d') = CURRENT_DATE();";
            Methods.fillGridTodaySubsProfs(gridTodaySubs, query);
        }

        private void Chk_Checked(object sender, RoutedEventArgs e)
        {
            //CheckBox ck = (CheckBox)sender;
            //activeStud += 1;
            //student st = (student)gridStudents.CurrentCell.Item;
            //st.Present = ck.IsChecked ?? false;
            //MessageBox.Show(st.Present.ToString());
            //lblStudActive.Content = "Studente te pranishem: " + activeStud.ToString();
        }

        private void Chk_Unchecked(object sender, RoutedEventArgs e)
        {
            //CheckBox ck = (CheckBox)sender;
            //activeStud -= 1;
            //student st = (student)gridStudents.CurrentCell.Item;
            //st.Present = ck.IsChecked ?? false;
            //MessageBox.Show(st.Present.ToString());
            //lblStudActive.Content = "Studente te pranishem: " + activeStud.ToString();
        }

        void StartSubject(object sender, RoutedEventArgs e)
        {
            scheduler sch = (scheduler)gridTodaySubs.SelectedItem;
            MySqlConnection myConnection = new MySqlConnection(myConnectionString);
            MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();
            int subject_ID = sch.subject_ID; //(int)cmbGroups.SelectedValue;
            int lush_ID = sch.lush_ID; //(int)cmbLush.SelectedValue;
            int group_ID = sch.group_ID; //(int)cmbSubjects.SelectedValue;
            string subject = sch.subject;
            string group = sch.group;
            string lush = sch.lush;
            DateTime startTime = new DateTime();
            TimeSpan compareStartToNow;
            try
            {
                string startTimeQuery = "SELECT sch.start_time FROM scheduler sch " +
                    "WHERE sch.subject_id=" + subject_ID + " AND sch.user_id=" + user_ID +
                    " AND sch.group_id=" + group_ID + " AND sch.lush_id=" + lush_ID + "";


                myCommand.CommandText = startTimeQuery;
                try
                {
                    myConnection.Open();
                    MySqlDataReader myReader = myCommand.ExecuteReader();
                    try
                    {
                        while (myReader.Read())
                            startTime = myReader.GetDateTime(0);
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            catch { }
            compareStartToNow = now.Subtract(startTime); //DateTime.Compare(now, startTime);
            // mos harro me unkodu pjesen kjo osht veq per testim true
            if (true)//compareStartToNow.TotalMinutes >= -15 && compareStartToNow.TotalMinutes <= 15)
            {
                try
                {
                    
                    bool check = (subject_ID > 0) && (lush_ID > 0) && (group_ID > 0);
                    if (check)
                    {
                        string getStudOnGroup = "SELECT st.num_id, st.name, st.surname FROM cslgs c INNER JOIN subjects sub ON sub.subject_id=c.subject_id " +
                            "INNER JOIN lush l ON l.lush_id=c.lush_id INNER JOIN groups g ON g.group_id=c.group_id " +
                            "INNER JOIN students st ON st.student_id=c.student_id WHERE c.subject_ID='" + subject_ID + "' " +
                            "AND c.lush_id='" + lush_ID + "' AND c.group_id='" + group_ID + "'";
                        //Methods.fillDGTextbox(gridStudents, getStudOnGroup);
                        //int studentsCount = (gri.Items.Count);
                        MessageBox.Show(string.Format("Ora {0} Grupi: {1} {2}\n ka filluar me sukses", subject, group, lush));
                        sch.subActive = true;

                    }
                }
                catch { }
            }
            else if (compareStartToNow.TotalMinutes < -15)
                MessageBox.Show("Me vjen keq!\n Eshte heret per ta filluar oren");
            else
                MessageBox.Show("Jeni Vonuar! Ju lutemi kerkoni leje nga dekani per te filluar kete ore.");
        }

        private void gridTodaySubs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid grid = sender as DataGrid;
                if (grid != null && grid.SelectedItems != null && grid.SelectedItems.Count == 1)
                {
                    DataGridRow dgr = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                    if (!dgr.IsMouseOver)
                    {
                        (dgr as DataGridRow).IsSelected = false;
                    }
                }
            }
        }



    }
}
