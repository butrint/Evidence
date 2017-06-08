using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
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
    /// Interaction logic for EvidenceWindow.xaml
    /// </summary>
    public partial class EvidenceWindow : Window
    {
        List<dbTablesIdName> sub = new List<dbTablesIdName>();

        private static string myConnectionString = "Data Source=localhost;Initial Catalog=vijueshmeria;User ID=root;Password=";
        private static MySqlConnection myConnection = new MySqlConnection(myConnectionString);
        private static MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();
        private static Process p = new Process();
        private static DateTime now = new DateTime();
        private static string today;
        private static string user_ID;
        private static scheduler sch;
        private static int activeStud = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idProf"></param>
        public EvidenceWindow(string idProf)
        {
            user_ID = idProf;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            InitializeComponent();

            now = Methods.datetimeInMysql();
            

            stopVirtualWifi();
            string cmbFirstItem = "Zgjedhni Lenden...";
            string getProfSubjectsQuery = "SELECT s.subject_id, s.subject FROM cps c INNER JOIN subjects s ON s.subject_id=c.subject_id INNER JOIN users u ON u.user_id=c.user_id WHERE c.user_id='" + idProf + "'";
            //Methods.fillCombo(cmbSubjects, getProfSubjectsQuery, cmbFirstItem);
            sch = Methods.SubClosestToDate(user_ID);
            cmbSubjects.Items.Add(sch.subject);
            cmbLush.Items.Add(sch.lush);
            cmbGroups.Items.Add(sch.group);
        }

        private void cmbSubjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //    cmbLush.SelectedIndex = -1;
            //    cmbLush.SelectedItem = null;
            //    cmbGroups.SelectedIndex = -1;
            //    cmbGroups.SelectedItem = null;
            //    try
            //    {
            //        ComboBox cmb = (ComboBox)sender;
            //        //MessageBox.Show(cmb.SelectedValue.ToString());
            //        int subject_ID = (int)cmb.SelectedValue;
            //        if (subject_ID != -1)
            //        {
            //            string cmbFirstItem = "Zgjedhni Llojin...";
            //            string getSubsLushQuery = "SELECT DISTINCT l.lush_id, l.lush FROM cslgs c INNER JOIN lush l ON c.lush_id=l.lush_id INNER JOIN subjects s ON c.subject_id=s.subject_id WHERE c.subject_id='" + subject_ID + "'";
            //            Methods.fillCombo(cmbLush, getSubsLushQuery, cmbFirstItem);
            //            cmbLush.SelectedValue = -1;
            //        }
            //    }
            //    catch { }
        }

        private void cmbLush_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //    try
            //    {
            //        ComboBox cmb = (ComboBox)sender;
            //        //MessageBox.Show(cmb.SelectedValue.ToString());
            //        int lush_ID = (int)cmb.SelectedValue;
            //        if (lush_ID != -1)
            //        {
            //            string cmbFirstItem = "Zgjedhni Grupin...";
            //            string getLushGroupQuery = "SELECT DISTINCT g.group_id, g.group FROM cslgs c INNER JOIN lush l ON c.lush_id=l.lush_id INNER JOIN groups g ON c.group_id=g.group_id WHERE c.lush_id='" + lush_ID + "'";
            //            Methods.fillCombo(cmbGroups, getLushGroupQuery, cmbFirstItem);
            //            cmbGroups.SelectedValue = -1;
            //        }
            //    }
            //    catch { }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            int subject_ID = sch.subject_ID; //(int)cmbGroups.SelectedValue;
            int lush_ID = sch.lush_ID; //(int)cmbLush.SelectedValue;
            int group_ID = sch.group_ID; //(int)cmbSubjects.SelectedValue;
            DateTime startTime = new DateTime();
            TimeSpan compareStartToNow;
            try
            {
                string startTimeQuery = "SELECT sch.start_time FROM scheduler sch "+
                    "WHERE sch.subject_id="+subject_ID+" AND sch.user_id="+user_ID+
                    " AND sch.group_id="+group_ID+" AND sch.lush_id="+lush_ID+"";
                
                
                myCommand.CommandText = startTimeQuery;
                try
                {
                    myConnection.Open();
                    MySqlDataReader myReader = myCommand.ExecuteReader();
                    try
                    {
                        while(myReader.Read())
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
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            catch { }
            compareStartToNow = now.Subtract(startTime); //DateTime.Compare(now, startTime);
            if (compareStartToNow.TotalMinutes >= -15 && compareStartToNow.TotalMinutes <= 15)
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
                        Methods.fillDGTextbox(gridStudents, getStudOnGroup);
                        int studentsCount = (gridStudents.Items.Count);
                        lblAllStud.Content = "Gjithesej student jane: " + studentsCount.ToString();

                    }
                    stopVirtualWifi();
                    prepareVirtualWifi();
                }
                catch { }
            }
            else if (compareStartToNow.TotalMinutes < -15)
                MessageBox.Show("Me vjen keq!\n Eshte heret per ta filluar oren");
            else
                MessageBox.Show("Jeni Vonuar! Ju lutemi kerkoni leje nga dekani per te filluar kete ore.");

        }

        private void gridStudents_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()+1).ToString();
        }
        
        private static void stopVirtualWifi()
        {
            p.StartInfo.FileName = "netsh";
            p.StartInfo.Arguments = "wlan stop hostednetwork";
            try
            {
                using (Process execute = Process.Start(p.StartInfo))
                {
                    execute.WaitForExit();
                }
            }
            catch { }
        }
        
        private void prepareVirtualWifi()
        {
            p.StartInfo.FileName = "netsh";
            p.StartInfo.Arguments = "wlan stop hostednetwork";
            try
            {
                using (Process execute = Process.Start(p.StartInfo))
                {
                    execute.WaitForExit();
                    setVirtualWifi();
                }
            }
            catch { }
        }

        private void setVirtualWifi()
        {
            string subject="Lenda";
            string group = "";
            string lush = "";
            string myssid = "";
            if (cmbSubjects.SelectedIndex != -1)
            {
                subject = cmbSubjects.Text;
                group = cmbGroups.Text;
                lush = cmbLush.Text.Substring(0, 1); 
            }
            group = string.IsNullOrEmpty(group) ? "" : "Gr."+group;
            myssid = subject + " " + group + " " + lush;
            //System.Environment.SetEnvironmentVariable("mySSID", subject + " " + group + " " + lush);
            p.StartInfo.FileName = "netsh";
            p.StartInfo.Arguments = "wlan set hostednetwork mode=allow ssid=\""+ myssid + "\" key=fjalekalimi";
            try
            {
                using (Process execute = Process.Start(p.StartInfo))
                {
                    execute.WaitForExit();
                    startVirtualWifi();
                }
            }
            catch { }
        }

        private static void startVirtualWifi()
        {
            p.StartInfo.FileName = "netsh";
            p.StartInfo.Arguments = "wlan start hostednetwork";
            try
            {
                using (Process execute = Process.Start(p.StartInfo))
                {
                    execute.WaitForExit();
                }
            }
            catch { }
        }

        private void gridStudents_CurrentCellChanged(object sender, EventArgs e)
        {
            //int activeStud = 0;
            //for (int i = 0; i < gridStudents.Items.Count; i++)
            //{
            //    var item = gridStudents.Items[i];
            //    var mycheckbox = gridStudents.Columns[3].GetCellContent(item) as CheckBox;
            //    try
            //    {
            //        if ((bool)mycheckbox.IsChecked)
            //            activeStud++;
            //    }
            //    catch { }
            //}
            //lblStudActive.Content = "Studente te pranishem: " + activeStud.ToString();
        }

        private void gridStudents_TargetUpdated(object sender, DataTransferEventArgs e)
        {

            //int activeStud = 0;
            //for (int i = 0; i < gridStudents.Items.Count; i++)
            //{
            //    var item = gridStudents.Items[i];
            //    var mycheckbox = gridStudents.Columns[3].GetCellContent(item) as CheckBox;
            //    try
            //    {
            //        if ((bool)mycheckbox.IsChecked)
            //            activeStud++;
            //    }
            //    catch { }
            //}
            //lblStudActive.Content = "Studente te pranishem: " + activeStud.ToString();
        }

        private void Chk_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = (CheckBox)sender;
            activeStud += 1;
            student st = (student) gridStudents.CurrentCell.Item;
            st.Present = ck.IsChecked ?? false;
            MessageBox.Show(st.Present.ToString());
            lblStudActive.Content = "Studente te pranishem: " + activeStud.ToString();
        }

        private void Chk_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = (CheckBox)sender;
            activeStud -= 1;
            student st = (student)gridStudents.CurrentCell.Item;
            st.Present = ck.IsChecked ?? false;
            MessageBox.Show(st.Present.ToString());
            lblStudActive.Content = "Studente te pranishem: " + activeStud.ToString();
        }
    }
}
