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
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace Evidence
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        //private static string myConnectionString = "Data Source=localhost;Initial Catalog=vijueshmeria;User ID=root;Password=";
        private static DateTime now = new DateTime();
        private static string user_ID;
        private static SolidColorBrush gray = Methods.getColor("gray");
        private static SolidColorBrush green = Methods.getColor("green");
        private static SolidColorBrush red = Methods.getColor("red");
        private static SolidColorBrush orange = Methods.getColor("orange");
        private static SolidColorBrush gold = Methods.getColor("gold");
        private static ObservableCollection<TodayAllData> todayAllData = new ObservableCollection<TodayAllData>();

        public AdminWindow(string idAdmin)
        {
            // 2017-06-05
            InitializeComponent();
            user_ID = idAdmin;
            string query = "SELECT * FROM `scheduler` WHERE DATE_FORMAT(start_time, '%Y-%m-%d') = CURRENT_DATE();";
            todayAllData = Methods.getTodaySubsProfs(query);
            query = "SELECT name, surname FROM users WHERE user_id=" + user_ID;
            List<dynamic> tmpList = Methods.selectFromDbs(query);
            string name = tmpList[0];
            string surname = tmpList[1];
            lblCurrUser.Content = name + " " + surname;
            now = Methods.datetimeInMysql();
            gridTodaySubs.ItemsSource = todayAllData;
            lblTodayDate.Content = Methods.selectFromDbs("SELECT DATE(NOW())")[0].ToString("yyyy-MM-dd");
        }

        void StartSubject(object sender, RoutedEventArgs e)
        {
            Button btnFillo = (Button)sender;
            TodayAllData todayData = (TodayAllData)gridTodaySubs.SelectedItem;
            todayData.lecHasBeenHeld();
            bool lectureWasHeld = todayData.lecIsOver;
            //Color white = (Color)ColorConverter.ConvertFromString("Red");
            // 50,205,50 - limegreen
            Methods.selectFromDbs("SELECT * FROM scheduler");
            
            int subject_ID = todayData.subject_ID; //(int)cmbGroups.SelectedValue;
            int lush_ID = todayData.lush_ID; //(int)cmbLush.SelectedValue;
            int group_ID = todayData.group_ID; //(int)cmbSubjects.SelectedValue;
            string subject = todayData.subject;
            string group = todayData.group;
            string lush = todayData.lush;
            now = Methods.datetimeInMysql();
            DateTime maxLecDuration = new DateTime();
            double maxMinutesToEndSub = 30;
            maxLecDuration = todayData.end_Time.AddMinutes(maxMinutesToEndSub);
            bool canEndSub = false;
            
            if (todayData.subActive)
            {
                TimeSpan minToAdd = TimeSpan.FromMinutes(10);
                TimeSpan minOfSubThatHasBeenHeld;
                TimeSpan foreseenMinForSub;
                foreseenMinForSub = todayData.end_Time.Subtract(todayData.start_Time);
                minOfSubThatHasBeenHeld = (now.Subtract(todayData.hasStartedAt)).Add(minToAdd);
                if (minOfSubThatHasBeenHeld.TotalMinutes >= foreseenMinForSub.TotalMinutes)
                    canEndSub = true;
                else
                    MessageBox.Show("Me vjen keq nuk mund te mbyllni kete ore");
            }

            if (!todayData.subActive && btnFillo.IsEnabled && !lectureWasHeld)
            {
                DateTime startTime = new DateTime();
                TimeSpan compareStartToNow;
                string startTimeQuery = "SELECT sch.start_time FROM scheduler sch " +
                        "WHERE sch.subject_id=" + subject_ID + " AND sch.user_id=" + user_ID +
                        " AND sch.group_id=" + group_ID + " AND sch.lush_id=" + lush_ID + "";

                startTime = Methods.startTimeOfSub(startTimeQuery);

                compareStartToNow = now.Subtract(startTime);
                // mos harro me uncommentu pjesen kjo osht veq per testim true
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
                            MessageBox.Show("Ora " + todayData.subject + " filloi me sukses!");
                            todayData.subActive = true;
                            string startoOren = "INSERT INTO lecturetime(start_time, end_time, automatic_ended, schedule_id) VALUES('" + now.ToString("yyyy-MM-dd hh:mm:ss") + "', '" + maxLecDuration.ToString("yyyy-MM-dd hh:mm:ss") + "', 1, " + todayData.schedule_ID + ")";
                            Methods.updateOrInsertIntoTable(startoOren);
                            todayData.rowColor = gold;
                            todayData.hasStartedAt = now;
                        }
                    }
                    catch { }
                    todayData.btnContent = "Perfundo";
                }
                else if (compareStartToNow.TotalMinutes < -15)
                    MessageBox.Show("Me vjen keq!\n Eshte heret per ta filluar oren");
                else
                    MessageBox.Show("Jeni Vonuar! Ju lutemi kerkoni leje nga dekani per te filluar kete ore.");
            }
            else if (todayData.subActive && btnFillo.IsEnabled && canEndSub)
            {
                string endTime = Methods.datetimeInMysql().ToString("yyyy-MM-dd hh:mm:ss");
                ///////////////////////////  UPDATE FUNKSIONI QE THIRRET PREJ Methods.cs \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                // Qeshtu e shkruni stringin edhe e thrrni funksionin me qat string variablat endTime edhe sch.schedule_ID  \\
                // jon tdeklarume ma nalt.                                                                                    \\
                /////////////////////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                string mbaroOren = "UPDATE lecturetime SET end_time='" + endTime + "', automatic_ended='0' WHERE schedule_id=" + todayData.schedule_ID;
                Methods.updateOrInsertIntoTable(mbaroOren);
                todayData.subActive = false;
                todayData.lecIsOver = true;
                MessageBox.Show("Ora " + todayData.subject + " mbaroi me sukses!");
                todayData.isEnabled = false;
                todayData.btnContent = "E perfunduar";
                todayData.rowColor = green;
                todayData.hasEndedAt = now;
            }
            else if (lectureWasHeld && (todayData.hasEndedAt.ToShortDateString() != "1/1/0001"))
            {
                todayData.lecIsOver = true;
                todayData.isEnabled = false;
                MessageBox.Show("Ora " + todayData.subject + " eshte mbajtur");
                todayData.btnContent = "E perfunduar";
                todayData.rowColor = green;
            }
            else if (todayData.hasEndedAt.ToShortDateString() == "1/1/0001")
            {
                MessageBox.Show("dsdss");
            }
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
