using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Evidence
{
    public static class Methods
    {
        private static string myConnectionString = "Data Source=localhost;Initial Catalog=vijueshmeria;User ID=root;Password=";
        //private static MySqlConnection myConnection = new MySqlConnection(myConnectionString);
        //private static MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();
        private static Color gr = Color.FromRgb(50, 205, 50);
        public static SolidColorBrush green = new SolidColorBrush(gr);
        private static Color rd = Color.FromRgb(240, 20, 20);
        public static SolidColorBrush red = new SolidColorBrush(rd);
        private static Color gra = (Color)ColorConverter.ConvertFromString("#FFF0F0F0");
        public static SolidColorBrush gray = new SolidColorBrush(gra);
        private static Color org = Color.FromRgb(255, 69, 0);
        public static SolidColorBrush orange = new SolidColorBrush(org);
        private static Color gld = Color.FromRgb(255, 223, 0);
        public static SolidColorBrush gold = new SolidColorBrush(gld);
        


        public static SolidColorBrush getColor(string color)
        {
            SolidColorBrush brush = new SolidColorBrush();
            green.Freeze(); red.Freeze(); orange.Freeze(); gold.Freeze(); brush.Freeze();
            switch (color)
            {
                case "gray" : return gray;
                case "red" : return red;
                case "green" : return green;
                case "orange": return orange;
                case "gold": return gold;
                default : return brush;
            }
        }

        public static DateTime datetimeInMysql()
        {
            string query = "SELECT NOW()";
            DateTime now = new DateTime();
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
            return now;
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



        public static void checkIfSubjectHasStarted(ObservableCollection<TodayAllData> todayAllData)
        {
            DateTime now = datetimeInMysql();
            foreach (var todayData in todayAllData)
            {
                todayData.lecHasBeenHeld();
                string lecHasBeenHeld = "SELECT * FROM lecturetime WHERE shcedule_id=" + todayData.schedule_ID;
                int nowIsGreaterThanStartTime = DateTime.Compare(now, todayData.start_Time);
                if ((nowIsGreaterThanStartTime > 0) && (!todayData.subActive) && (!todayData.lecIsOver))
                    if (todayData.rowColor != red)
                    {
                        todayData.rowColor = red;
                        todayData.rowColor.Freeze();
                    }
                    else if (todayData.subActive)
                        if (todayData.rowColor != gold)
                        {
                            todayData.rowColor = gold;
                            todayData.rowColor.Freeze();
                        }
                        else if (todayData.lecIsOver)
                            if (todayData.rowColor != green)
                            {
                                todayData.rowColor = green;
                                todayData.rowColor.Freeze();
                            }
            }
        }

        public static ObservableCollection<TodayAllData> getTodaySubsProfs(string query)
        {
            ObservableCollection<TodayAllData> dtgPr = new ObservableCollection<TodayAllData>();
            ObservableCollection<scheduler> scheduler = new ObservableCollection<scheduler>();
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
                        dtgPr.Add(new TodayAllData() { schedule_ID = myReader.GetInt32(0), start_Time = myReader.GetDateTime(1), end_Time = myReader.GetDateTime(2), group_ID = myReader.GetInt32(3), hall_ID = myReader.GetInt32(4), lush_ID = myReader.GetInt32(5), user_ID = myReader.GetInt32(6), subject_ID = myReader.GetInt32(7) });
                        int sched_id = myReader.GetInt32(0);
                        GetFNamesFrmSched(dtgPr[i], sched_id);
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
            dtgPr.OrderBy(a => a.start_Time);
            return dtgPr;
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

        public static void dekanisSubjectView()
        {
            
        }

        public static scheduler SubClosestToDate(string prof_ID)
        {
            string query = "SELECT * FROM scheduler sch WHERE start_time BETWEEN CURDATE() AND timestamp(DATE_ADD(NOW(), INTERVAL 30 MINUTE)) AND user_id='" + prof_ID + "' ORDER BY start_time DESC LIMIT 1";

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
        
        public static List<dynamic> selectFromDbs(string query)
        {
            List<dynamic> lst = new List<dynamic>();
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
                        for (int i = 0; i < myReader.FieldCount; i++)
                            lst.Add(myReader.GetValue(i));
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

            return lst;
        }

        public static void updateOrInsertIntoTable(string query)
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
                    while (myReader.Read()) { }
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
        
        public static DateTime startTimeOfSub(string query)
        {
            DateTime setStartTime = new DateTime();
            MySqlConnection myConnection = new MySqlConnection(myConnectionString);
            MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();

            
            try
            {
                myCommand.CommandText = query;
                try
                {
                    myConnection.Open();
                    MySqlDataReader myReader = myCommand.ExecuteReader();
                    try
                    {
                        while (myReader.Read())
                            setStartTime = myReader.GetDateTime(0);
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
            return setStartTime;
        }

        public static DataGridRow GetGridRow(DataGrid dtg, int index)
        {
            DataGridRow row = (DataGridRow)dtg.ItemContainerGenerator.ContainerFromIndex(index);
            return row;
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
    }

    public class TodayAllData : scheduler, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void getValuesFromScheduler(scheduler sch)
        {
            this.schedule_ID = sch.schedule_ID;
            this.start_Time = sch.start_Time;
            this.end_Time = sch.end_Time;
            this.group_ID = sch.group_ID;
            this.group = sch.group;
            this.hall_ID = sch.hall_ID;
            this.hall = sch.hall;
            this.lush_ID = sch.lush_ID;
            this.lush = sch.lush;
            this.user_ID = sch.user_ID;
            this.username = sch.username;
            this.subject_ID = sch.subject_ID;
            this.subject = sch.subject;
        }

        private bool subAct = false;
        public bool subActive
        {
            get
            {
                return subAct;
            }
            set
            {
                subAct = value;
            }
        }
        private bool isEnabl = true;
        public bool isEnabled
        {
            get { return this.isEnabl; }
            set
            {
                if (this.isEnabl != value)
                {
                    this.isEnabl = value;
                    this.NotifyPropertyChanged("isEnabled");
                }
            }
        }
        private Brush _rowColor = Methods.getColor("gray");
        public Brush rowColor
        {
            get { return this._rowColor; }
            set
            {
                if (this._rowColor != value)
                {
                    this._rowColor = value;
                    this.NotifyPropertyChanged("rowColor");
                }
            }
        }
        private bool _lecIsOver = false;

        public bool lecIsOver
        {
            get { return _lecIsOver; }
            set {
                    if (this.lecIsOver != value)
                        this._lecIsOver = value;
                }
        }

        public DateTime hasStartedAt;
        public DateTime hasEndedAt;

        public void lecHasBeenHeld()
        {
            List<dynamic> lst = new List<dynamic>();
            string query = "SELECT * FROM lecturetime WHERE schedule_id=" + this.schedule_ID;
            lst = Methods.selectFromDbs(query);
            this._lecIsOver = lst.Any();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        private string _btnContent = "Fillo";
        public string btnContent
        {
            get { return this._btnContent; }
            set
            {
                if (this._btnContent != value)
                {
                    this._btnContent = value;
                    this.NotifyPropertyChanged("btnContent");
                }
            }
        }
    }
}
