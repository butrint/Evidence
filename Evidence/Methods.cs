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
using System.Security.Cryptography;

namespace Evidence
{
    public static class Methods
    {
        private static string myConnectionString = "Data Source=localhost;Initial Catalog=vijueshmeria;User ID=root;Password=; Convert Zero Datetime=True";
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

        public static ObservableCollection<TodayAllData> getScheduler()
        {
            ObservableCollection<TodayAllData> scheduler = new ObservableCollection<TodayAllData>();
            string query = "CALL getAllFNamesFrmSched();";

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
                        scheduler.Add(new TodayAllData() { schedule_ID = (int)myReader["schedule_id"], start_Time = (DateTime)myReader["start_time"], end_Time = (DateTime)myReader["end_time"], group = (string)myReader["group"], hall = (string)myReader["hall"], lush = (string)myReader["lush"], subject = (string)myReader["subject"] });
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
            return scheduler;
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

        public static Dictionary<string, List<dynamic>> getReport(string query)
        {
            // Permban rekorde per profesora si daten e mbajtjes koha e rregullt zevendesim etj
            Dictionary<string,  List<dynamic>> report = new Dictionary<string, List<dynamic>>();

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
                        report["start_time"].Add(myReader["start_time"]);
                        report["end_time"].Add(myReader["end_time"]);
                        report["automatic_ended"].Add(myReader["automatic_ended"]);
                        report["isActive"].Add(myReader["isActive"]);
                        report["user_id"].Add(myReader["user_id"]);
                        report["regular_start_time"].Add(myReader["strt_tm"]);
                        report["regular_end_time"].Add(myReader["ed_tm"]);
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

            return report;
        }
	
	    public static ObservableCollection<student> getStudents(string query)
	    {
		    ObservableCollection<student> students = new ObservableCollection<student>();

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
                        //string tel_id = myReader[""] == DBNull.Value ? "" : (string)myReader["phone_id"];
                        string tel_id = false ? "" : "";//(string)myReader["phone_id"];
                        int dev_id = myReader["devicereg_id"] == DBNull.Value ? -1 : (int)myReader["devicereg_id"];
                        students.Add(new student() { ID = (int)myReader["student_id"], Num_ID = (string)myReader["num_id"], Name = (string)myReader["name"], Surname = (string)myReader["surname"], Email = (string)myReader["email"], Birthday = (DateTime)myReader["birthday"], Phone_ID = tel_id, devicereg_id = dev_id });
                    }
                }
                catch(Exception ex)
                {
                    string erro = ex.Message;
                    MessageBox.Show("Errori ne MYsqL: "+erro);
                }
            }
            catch { }
		    
		    return students;
	    }

        public static ObservableCollection<Devicereg> getDevices(string query)
        {
            ObservableCollection<Devicereg> devices = new ObservableCollection<Devicereg>();

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
                        //string tel_id = myReader[""] == DBNull.Value ? "" : (string)myReader["phone_id"];
                        string pass = myReader["password"] == DBNull.Value ? "" : (string)myReader["password"];
                        string dev1 = myReader["device1"] == DBNull.Value ? "" : (string)myReader["device1"];
                        string dev2 = myReader["device2"] == DBNull.Value ? "" : (string)myReader["device2"];
                        string dev3 = myReader["device3"]== DBNull.Value ? "" : (string)myReader["device3"];

                        devices.Add(new Devicereg() { ID = (int)myReader["devicereg_id"], password = pass, device1 = dev1, device2 = dev2, device3 = dev3 });


                    }
                }
                catch (Exception ex)
                {
                    string erro = ex.Message;
                    MessageBox.Show("Errori ne MYsqL: " + erro);
                }
            }
            catch { }

            return devices;
        }
			
		public static string Encode(string original)
		{
			MD5CryptoServiceProvider MD5Code = new MD5CryptoServiceProvider();
			byte[] b = Encoding.UTF8.GetBytes(original);
			b = MD5Code.ComputeHash(b);
			StringBuilder sb = new StringBuilder();
			foreach (byte ba in b)
			{
				sb.Append(ba.ToString("x2").ToLower());
			}
			    return sb.ToString();
		}
        

		public static ObservableCollection<user> getUsers(string query)
			{
			List<dynamic> usr = new List<dynamic>();
			ObservableCollection<user> users = new ObservableCollection<user>();
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
			                users.Add(new user() { ID = myReader.GetInt32(0), Prof_ID = myReader.GetString(1), Username = myReader.GetString(2), Password = myReader.GetString(3), Name = myReader.GetString(4), Surname = myReader.GetString(5), Birthday = myReader.IsDBNull(6) ? new DateTime() : myReader.GetDateTime(6), Photo_path = myReader.IsDBNull(7) ? "" : myReader.GetString(7), Role_ID = myReader.GetInt32(8) });
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
		foreach (var useri in users)
			useri.setRole();
		return users;
		}

		public static List<role> getRoles(string query)
		{
			List<dynamic> lst = new List<dynamic>();
			List<role> roles = new List<role>();
			
			lst = selectFromDbs(query);
			for (int i = 0; i < lst.Count; i+=2)
			    roles.Add(new role() { Role_ID = lst[i], Role = lst[i+1] });
			
			return roles;
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
                todayData.checkIfSubActiveFrmDbs();
                todayData.lecHasBeenHeld();
                string lecHasBeenHeld = "SELECT * FROM `lecturetime` WHERE `shcedule_id`='" + todayData.schedule_ID+"';";
                int nowIsGreaterThanStartTime = DateTime.Compare(now, todayData.start_Time);
                if ((nowIsGreaterThanStartTime > 0) && (!todayData.subActive) && (!todayData.lecIsOver))
                {
                    if (todayData.rowColor != red)
                    {
                        todayData.rowColor = red;
                        todayData.rowColor.Freeze();
                    }
                }
                else if (todayData.subActive)
                {
                    if (todayData.rowColor != gold)
                    {
                        todayData.rowColor = gold;
                        todayData.rowColor.Freeze();
                    }
                }
                else if (todayData.lecIsOver)
                {
                    if (todayData.rowColor != green)
                    {
                        todayData.rowColor = green;
                        todayData.rowColor.Freeze();
                    }
                }
            }
        }

        public static ObservableCollection<TodayAllData> getTodaySubsProfs(string query, string query1="")
        {
            ObservableCollection<TodayAllData> dtgPr = new ObservableCollection<TodayAllData>();
            List<dynamic> lst1 = new List<dynamic>();
            List<dynamic> lst2 = new List<dynamic>();
            
            //lst2 = query != "" ? selectFromDbs(query1) : lst2;
            //lst1 = selectFromDbs(query);

            //if (lst2.Any())
            //    lst1.AddRange(lst2);

            ObservableCollection<scheduler> scheduler = new ObservableCollection<scheduler>();

            //for (int i = 0; i < lst1.Count; i+=8)
            //{

            //}
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
                        {
                            bool isNull = myReader.GetValue(i) == DBNull.Value;
                            if (!isNull)
                                lst.Add(myReader.GetValue(i));
                            else
                                lst.Add(null);
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

            return lst;
        }

        public static void updateOrInsertIntoTable(string query, string message = "")
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
		            if (message != "")
		            {
                                MessageBox.Show(message);
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
                //MessageBox.Show("Për momentin lidhja dështoi. Provoni më vonë");
                MessageBox.Show(ex.Message);
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
	//////////////////////////////////////////////////////
	////////////////////////////////
	/// QET Pjes duhet me e bo update
	///////////////////
		public static bool existsInDBS(string query)
		{
			List<dynamic> lst = new List<dynamic>();
			lst = selectFromDbs(query);
			if (!lst.Any())
			    return false;
			else
			    return true;
		}
    }

    public class dbTablesIdName
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    
    public class student : INotifyPropertyChanged
    {
	    public event PropertyChangedEventHandler PropertyChanged;
	    private int _ID;
	    public int ID
	    {
		    get
		    {
			    return this._ID;
		    }
		    set
		    {
			    if (this._ID != value)
			    {
			        this._ID = value;
			        this.NotifyPropertyChanged("ID");
			    }
			
		    }
	    }
		private string _Num_ID;
		public string Num_ID
		{
			get
			{
			    return this._Num_ID;
			}
			set
			{
			    if (this._Num_ID != value)
			    {
			        this._Num_ID = value;
			        this.NotifyPropertyChanged("Num_ID");
			    }
			
			}
		}
		private string _Name;
		public string Name
		{
			get
			{
			    return this._Name;
			}
			set
			{
			    if (this._Name != value)
			    {
			        this._Name = value;
			        this.NotifyPropertyChanged("Name");
			    }
			}
		}
		private string _Surname;
		public string Surname
		{
			get
			{
			    return this._Surname;
			}
			set
			{
			    if (this._Surname != value)
			    {
			        this._Surname = value;
			        this.NotifyPropertyChanged("Surname");
			    }
			}
		}
		private string _Email;
		public string Email
		{
			get
			{
			    return this._Email;
			}
			set
			{
			    if (this._Email != value)
			    {
			        this._Email = value;
			        this.NotifyPropertyChanged("Email");
			    }
			}
		}
			
		private DateTime _Birthday;
		public DateTime Birthday
		{
			get
			{
			    return _Birthday.Date;
			}
			set
			{
			    this._Birthday = value;
			    this.NotifyPropertyChanged("Birthday");
			}
		}

        private string _Phone_ID;
        public string Phone_ID
        {
            get { return _Phone_ID; }
            set
            {
                if (this.Phone_ID != value)
                {
                    if (!String.IsNullOrEmpty(value))
                        this.hasPhone = true;
                    this._Phone_ID = value;
                    this.NotifyPropertyChanged("Phone_ID");
                }
            }
        }

        private bool? _isPresent = false;
        public bool? isPresent
        {
            get { return (_isPresent != null) ? _isPresent : false; }
            set
            {
                if (this._isPresent != value)
                {
                    this._isPresent = value;
                    this.NotifyPropertyChanged("isPresent");
                }
            }
        }

        private bool _hasPhone;
        public bool hasPhone
        {
            get { return this._hasPhone; }
            set
            {
                if (this._hasPhone != value)
                {
                    this._hasPhone = value;
                    this.NotifyPropertyChanged("hasPhone");
                }
            }
        }

        private int _devicereg_id=-1;

        public int devicereg_id
        {
            get { return this._devicereg_id; }
            set
            {
                if (this._devicereg_id != value)
                {
                    this._devicereg_id = value;
                    setPassAndDevices();
                    this.NotifyPropertyChanged("devicereg_id");
                }
            }
        }

        private string _device1 = "";
        public string device1
        {
            get { return this._device1; }
            set
            {
                if (this._device1 != value)
                {
                    this._device1 = value;
                    this.NotifyPropertyChanged("device1");
                }
            }
        }

        private string _device2 = "";
        public string device2
        {
            get { return this._device2; }
            set
            {
                if (this._device2 != value)
                {
                    this._device2 = value;
                    this.NotifyPropertyChanged("device2");
                }
            }
        }

        private string _device3 = "";
        public string device3
        {
            get { return this._device3; }
            set
            {
                if (this._device3 != value)
                {
                    this._device3 = value;
                    this.NotifyPropertyChanged("device3");
                }
            }
        }

        private string _password;
        public string password
        {
            get { return this._password; }
            set
            {
                this._password = value;
                this.NotifyPropertyChanged("password");
            }
        }

        private string _ip_Address = "";
        public string ip_Address
        {
            get { return this._ip_Address; }
            set
            {
                this._ip_Address = value;
                this.NotifyPropertyChanged("ip_Address");
            }

        }

        private int _countPresence=0;
        public int countPresence
        {
            get { return this._countPresence; }
            set
            {
                if (this._countPresence != value)
                {
                    this._countPresence = value;
                    this.NotifyPropertyChanged("countPresence");
                }
            }
        }

        public void updateStudent(string num_id, string name, string surname, string email, DateTime birthday)
		{
			this.Num_ID = num_id;
			this.Name = name;
			this.Surname = surname;
			this.Email = email;
			this.Birthday = birthday;
		}

		public void setID()
		{
			List<dynamic> lst = new List<dynamic>();
			string query = "SELECT `student_id` FROM `students` WHERE `num_id`='" + this.Num_ID+"'";
			lst = Methods.selectFromDbs(query);
            if(lst.Any())
			    this.ID = lst[0];
		}

        public void setPassAndDevices()
        {
            if (this.devicereg_id > 0)
            {
                List<dynamic> lst = new List<dynamic>();
                string query = "SELECT password, device1, device2, device3 FROM devicereg ";
                lst = Methods.selectFromDbs(query);
                if (lst.Any())
                {
                    this.password = String.IsNullOrEmpty((string)lst[0]) ? "" : lst[0];
                    this.device1 = String.IsNullOrEmpty((string)lst[1]) ? "" : lst[1];
                    this.device2 = String.IsNullOrEmpty((string)lst[2]) ? "" : lst[2];
                    this.device3 = String.IsNullOrEmpty((string)lst[3]) ? "" : lst[3];
                }
            }
        }

		public void NotifyPropertyChanged(string propName)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
	}

    public class user : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int _ID;
        public int ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                if (this._ID != value)
                {
                    this._ID = value;
                    this.NotifyPropertyChanged("ID");
                }

            }
        }
        private string _Prof_ID;
        public string Prof_ID
        {
            get
            {
                return this._Prof_ID;
            }
            set
            {
                if (this._Prof_ID != value)
                {
                    this._Prof_ID = value;
                    this.NotifyPropertyChanged("Prof_ID");
                }

            }
        }
        private string _Username;
        public string Username
        {
            get
            {
                return this._Username;
            }
            set
            {
                if (this._Username != value)
                {
                    this._Username = value;
                    this.NotifyPropertyChanged("Username");
                }

            }
        }
        private string _Password;
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                if (this._Password != value)
                {
                    this._Password = value;
                    this.NotifyPropertyChanged("Password");
                }

            }
        }
        private string _Name;
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if (this._Name != value)
                {
                    this._Name = value;
                    this.NotifyPropertyChanged("Name");
                }

            }
        }
        private string _Surname;
        public string Surname
        {
            get
            {
                return this._Surname;
            }
            set
            {
                if (this._Surname != value)
                {
                    this._Surname = value;
                    this.NotifyPropertyChanged("Surname");
                }
            }
        }
        private DateTime _Birthday;
        public DateTime Birthday
        {
            get
            {
                return _Birthday.Date;
            }
            set
            {
                this._Birthday = value;
                this.NotifyPropertyChanged("Birthday");
            }
        }

        private string _Photo_Path;
        public string Photo_path
        {
            get
            {
                return _Photo_Path;
            }
            set
            {
                this._Photo_Path = value;
                this.NotifyPropertyChanged("Photo_path");
            }
        }
        
        private int _Role_ID;
        public int Role_ID
        {
            get
            {
                return this._Role_ID;
            }
            set
            {
                if (this._Role_ID != value)
                {
                    this._Role_ID = value;
                    this.NotifyPropertyChanged("Role_ID");
                }
            }
        }

        private string _Role;
        public string Role
        {
            get
            {
                return this._Role;
            }
            set
            {
                if (this._Role != value)
                {
                    this._Role = value;
                    this.NotifyPropertyChanged("Role");
                }
            }
        }

        public void insertUser(string prof_id, string username, string password, string name, string surname, DateTime birthday, int role_id)
        {
            this.Prof_ID = prof_id;
            this.Username = username;
            this.Password = password;
            this.Name = name;
            this.Surname = surname;
            this.Birthday = birthday;
            this.Role_ID = role_id;
        }

        public void updateUser(string prof_id, string username, string name, string surname, DateTime birthday, int role_id)
        {
            this.Prof_ID = prof_id;
            this.Username = username;
            this.Name = name;
            this.Surname = surname;
            this.Birthday = birthday;
            this.Role_ID = role_id;
        }

        public void setID()
        {
            List<dynamic> lst = new List<dynamic>();
            string query = "SELECT user_id FROM users WHERE prof_id=" +this.Prof_ID;
            lst = Methods.selectFromDbs(query);
            this.ID = lst[0];
        }

        public void setRole()
        {
            List<dynamic> lst = new List<dynamic>();
            string query = "SELECT r.role FROM users u INNER JOIN roles r ON r.role_id=u.role_id WHERE prof_id=" + this.Prof_ID;
            lst = Methods.selectFromDbs(query);
            this.Role = lst[0];
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }


    public class role
    {
        public int Role_ID { get; set; }
        public string Role { get; set; }
    }

    public class subject
    {
        public int ID { get; set; }
        public string Subject { get; set; }
    }

    public class faculty
    {
        public int ID { get; set; }
        public string Faculty { get; set; }
    }

    public class department
    {
        public int ID { get; set; }
        public string Department { get; set; }
    }

    public class lush
    {
        public int ID { get; set; }
        public string Lush { get; set; }
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

    public class Devicereg : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _ID;
        public int ID
        {
            get { return this._ID; }
            set
            {
                if (this._ID != value)
                {
                    this._ID = value;
                    this.NotifyPropertyChanged("ID");
                }
            }
        }
        private string _password;
        public string password
        {
            get { return this._password; }
            set
            {
                if (this._password != value)
                {
                    this._password = value;
                    this.NotifyPropertyChanged("password");
                }
            }
        }

        private string _device1;
        public string device1
        {
            get { return this._device1; }
            set
            {
                this._device1 = value;
                this.NotifyPropertyChanged("device1");
            }
        }
        private string _device2;
        public string device2
        {
            get { return this._device2; }
            set
            {
                this._device2 = value;
                this.NotifyPropertyChanged("device2");
            }
        }
        private string _device3;
        public string device3
        {
            get { return this._device3; }
            set
            {
                this._device3 = value;
                this.NotifyPropertyChanged("device3");
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }

    public class TodayAllData : scheduler, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private int _schedule_ID;
        public new int schedule_ID
        {
            get
            {
                return _schedule_ID;
            }
            set
            {
                this._schedule_ID = value;
                this.setIsSubtitute(this._schedule_ID);
                this.NotifyPropertyChanged("schedule_ID");
            }
        }

        private DateTime _start_Time;
        public new DateTime start_Time
        {
            get { return _start_Time; }
            set
            {
                if (this._start_Time != value)
                {
                    this._start_Time = value;
                    this.NotifyPropertyChanged("start_Time");
                }
            }

        }

        private DateTime _end_Time;
        public new DateTime end_Time
        {
            get { return _end_Time; }
            set
            {
                if (this._end_Time != value)
                {
                    this._end_Time = value;
                    this.NotifyPropertyChanged("end_Time");
                }
            }

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

        private string _isSubstitute="E rregullt";
        public string isSubstitute
        {
            get
            {
                return _isSubstitute;
            }
            set
            {
                if (this._isSubstitute != value)
                {
                    this._isSubstitute = value;
                    this.NotifyPropertyChanged("isSubstitute");
                }
            }
        }

        private void setIsSubtitute(int schedule_id)
        {
            string query = "SELECT * FROM substitution WHERE schedule_id='" + schedule_id + "';";
            if (Methods.existsInDBS(query))
                isSubstitute = "Zevendesim";
            else
                isSubstitute = "E rregullt";
        }

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

        public void checkIfSubActiveFrmDbs()
        {
            string query = "SELECT isActive FROM lecturetime WHERE `schedule_id`='"+schedule_ID+"' AND `isActive`='1';";
            if (Methods.existsInDBS(query))
                this.subActive = true;
            else
                this.subActive = false;
        }

        public void lecHasBeenHeld()
        {
            if (!this.subActive)
            {
                List<dynamic> lst = new List<dynamic>();
                string query = "SELECT * FROM lecturetime WHERE schedule_id=" + this.schedule_ID;
                lst = Methods.selectFromDbs(query);
                this._lecIsOver = lst.Any();
            }
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
