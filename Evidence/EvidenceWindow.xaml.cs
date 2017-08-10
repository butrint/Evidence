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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;
using System.Net.Mail;

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
        private static Socket serveri;
        private static readonly List<Socket> klientet = new List<Socket>();
        private const int _BUFFER_SIZE = 2048;
        private const int PORT = 8989;
        private static readonly byte[] _buffer = new byte[_BUFFER_SIZE];
        private bool startedServer = false;
        private static EvidenceWindow eW;
        private static Process p = new Process();
        private static DateTime now = new DateTime();
        private static string today;
        private static string user_ID;
        private static scheduler sch;
        private static int activeStud = 0;
        private static ObservableCollection<student> students = new ObservableCollection<student>();
        private static ObservableCollection<Devicereg> devices = new ObservableCollection<Devicereg>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idProf"></param>
        public EvidenceWindow(string idProf)
        {
            user_ID = idProf;
            eW = this;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            InitializeComponent();

            now = Methods.datetimeInMysql();
            string query = "SELECT * FROM devicereg";
            devices = Methods.getDevices(query);
            
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

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            int subject_ID = 1;////sch.subject_ID; //(int)cmbGroups.SelectedValue;
            int lush_ID = 1; ////sch.lush_ID; //(int)cmbLush.SelectedValue;
            int group_ID = 1;////sch.group_ID; //(int)cmbSubjects.SelectedValue;
            DateTime startTime = new DateTime();
            TimeSpan compareStartToNow;
            now = Methods.datetimeInMysql();
            if (!startedServer)
            {
                //unkomento
                startedServer = true;
                //SetupServer();
            }
            else if (startedServer)
            {
                //unkomento
                startedServer = false;
                //stopVirtualWifi();
                serveri.Close();
            }

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
                            startTime = (DateTime)myReader["start_time"];
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
            if (true)//compareStartToNow.TotalMinutes >= -15 && compareStartToNow.TotalMinutes <= 15) ////&& !startedServer)
            {
                try
                {
                    bool check = (subject_ID > 0) && (lush_ID > 0) && (group_ID > 0);
                    if (check)
                    {
                        string getStudOnGroup = "SELECT st.* FROM cslgs c INNER JOIN subjects sub ON sub.subject_id=c.subject_id " +
                            "INNER JOIN lush l ON l.lush_id=c.lush_id INNER JOIN groups g ON g.group_id=c.group_id " +
                            "INNER JOIN students st ON st.student_id=c.student_id WHERE c.subject_ID='" + subject_ID + "' " +
                            "AND c.lush_id='" + lush_ID + "' AND c.group_id='" + group_ID + "'";
                        students = Methods.getStudents(getStudOnGroup);
                        gridStudents.ItemsSource = students;
                        //Methods.fillDGTextbox(gridStudents, getStudOnGroup);
                        int studentsCount = (gridStudents.Items.Count);
                        lblAllStud.Content = "Gjithesej student jane: " + studentsCount.ToString();
                        SetupServer();
                        startedServer = true;
                        stopVirtualWifi();
                        prepareVirtualWifi();
                    }
                }
                catch { }
            }
            else if (compareStartToNow.TotalMinutes < -15)
                MessageBox.Show("Me vjen keq!\n Eshte heret per ta filluar oren");
            else if (compareStartToNow.TotalMinutes > 15)
                MessageBox.Show("Jeni Vonuar! Ju lutemi kerkoni leje nga dekani per te filluar kete ore.");
            //else if (startedServer)
            //{
            //    startedServer = false;
            //    stopVirtualWifi();
            //}

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

        private static Boolean SetupServer()
        {
            //IPHostEntry iphostInfo = Dns.GetHostEntry(Dns.GetHostName());

            //IPAddress ipAddress = iphostInfo.AddressList[4];
            //IPAddress ipAddress = IPAddress.Parse("192.168.137.1");
            //MessageBox.Show(ipAddress+"");

            //IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];

            Thread trd = new Thread(() =>
            {
                var ServerUdp = new UdpClient(8889);
                var ResponseData = Encoding.ASCII.GetBytes("SomeResponseData");

                while (true)
                {
                    IPAddress ipAddressUdp = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
                    var ClientEp = new IPEndPoint(ipAddressUdp, 11000);
                    var ClientRequestData = ServerUdp.Receive(ref ClientEp);
                    var ClientRequest = Encoding.ASCII.GetString(ClientRequestData);

                    Debug.WriteLine("Recived {0} from {1}, sending response", ClientRequest, ClientEp.Address.ToString());
                    ServerUdp.Send(ResponseData, ResponseData.Length, ClientEp);
                }
            });
            trd.Start();

            IPAddress ipAddressTcp = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            // Qetu munet met qit problem pershkak t'IP-s.
            IPEndPoint localEndpoint = new IPEndPoint(IPAddress.Any, PORT);
            try
            {
                serveri = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serveri.Bind(localEndpoint);
                serveri.Listen(2048);
                serveri.BeginAccept(AcceptCallback, serveri);

                // new IPEndPoint(ipAddress, 0);
            }
            catch { return false; }

            return true;
        }
        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;
            try
            {
                socket = serveri.EndAccept(AR);
                MessageBox.Show("U konektua");
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            socket.BeginReceive(_buffer, 0, _BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            serveri.BeginAccept(AcceptCallback, null);
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received; student stud;

            try
            {
                received = current.EndReceive(AR);
                if (received != 0)
                {
                    byte[] recBuf = new byte[received];
                    Array.Copy(_buffer, recBuf, received);
                    string text = Encoding.UTF8.GetString(recBuf);
                    string[] details = Encoding.ASCII.GetString(recBuf).Split(',');

                    string studId = details[1];
                    string studEmail = details[2];
                    string password = "1234567";
                    string passEncoded = Methods.Encode(password);
                    
                    student std = students.FirstOrDefault<student>(c => c.Num_ID.Equals(studId) && c.Email.Equals(studEmail));
                    //student stud = (student)dGrid.SelectedItem;
                    if (std != null)
                    {
                        List<dynamic> lastId = new List<dynamic>();
                        string query = "INSERT INTO devicereg(password) VALUE('" + passEncoded + "')";
                        Methods.updateOrInsertIntoTable(query);
                        query = "SELECT devicereg_id FROM devicereg ORDER BY devicereg_id DESC limit 1";
                        lastId = Methods.selectFromDbs(query);
                        query = "UPDATE students  SET devicereg_id =" + lastId[0] + " WHERE student_id = " + std.ID;
                        Methods.updateOrInsertIntoTable(query);
                        std.password = password;

                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("evidencaproject@gmail.com");
                            mail.To.Add(std.Email);
                            mail.Subject = "Fjalekalimi per evidencen";
                            mail.Body = "Ky eshte fjalekalimi juaj: " + password;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new NetworkCredential("evidencaproject@gmail.com", "Jimmy123");
                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                            }
                        }
                    }

                    if (details[0].Equals("student"))
                    {
                        studId = details[1];
                        string studPass = details[2];
                        string studPassEn = Methods.Encode(studPass);
                        std = students.FirstOrDefault<student>(c => c.password.Equals(studPassEn) && c.Num_ID.Equals(studId));

                        if (std != null)
                        {
                            byte[] data = Encoding.UTF8.GetBytes("vazhdo");
                            int length = data.Length;
                            //current.Send(Encoding.UTF8.GetBytes(length.ToString()));
                            current.Send(data);
                        }
                    }

                    if (details[0].Equals("device"))
                    {
                        string device_id = details[1];
                        studId = details[2];
                        stud = students.FirstOrDefault<student>(c => c.Num_ID.Equals(studId));
                        if (stud != null)
                        {
                            string deviceNum = "";
                            string deviceExistsQ = "SELECT * FROM devicereg WHERE device1='" + device_id + "' OR device2='" + device_id + "' OR device3='" + device_id + "' ";
                            bool deviceExists = Methods.existsInDBS(deviceExistsQ);
                            bool freeSpotAndDeviceNotRegistered = (String.IsNullOrEmpty(stud.device1) || String.IsNullOrEmpty(stud.device2) || String.IsNullOrEmpty(stud.device3)) && (!deviceExists);
                            Debug.WriteLine("Gati me hi");
                            if (freeSpotAndDeviceNotRegistered)
                            {
                                Debug.WriteLine("Hini");
                                if (String.IsNullOrEmpty(stud.device1))
                                {
                                    deviceNum = "device1";
                                    stud.device1 = device_id;
                                }
                                else if (String.IsNullOrEmpty(stud.device1))
                                {
                                    deviceNum = "device2";
                                    stud.device2 = device_id;
                                }
                                else if (String.IsNullOrEmpty(stud.device1))
                                {
                                    deviceNum = "device3";
                                    stud.device3 = device_id;
                                }
                                if (!(String.IsNullOrEmpty(deviceNum)))
                                {
                                    string query = "UPDATE devicereg SET `"+deviceNum+"`='" + device_id + "' WHERE devicereg_id='" + stud.devicereg_id + "' ";
                                    Methods.updateOrInsertIntoTable(query);
                                }
                                byte[] data = Encoding.UTF8.GetBytes("vazhdo");
                                int length = data.Length;
                                //current.Send(Encoding.UTF8.GetBytes(length.ToString()));
                                current.Send(data);
                            }
                            else
                                Debug.WriteLine("S'hini");
                        }
                    }

                    stud = students.FirstOrDefault<student>(c => c.device1.Equals(details[1]) || c.device2.Equals(details[1]) || c.device3.Equals(details[1]));

                    //unkoment
                    if (stud != null)//details[0].Equals("student"))
                    {
                        stud.ip_Address = current.RemoteEndPoint.ToString();
                        stud.isPresent = true;
                        stud.countPresence += 1;
                        
                        //unkoment
                        if (false)//!eW.students.Any<Student>(c => c.Id.ToString().Equals(details[1].Trim())))
                        {
                            //unkoment
                            MessageBox.Show("true");
                            klientet.Add(current);
                            //studentet.Add(new Student { Nr = count, Id = int.Parse(details[1]), Emri = details[2], Mbiemri = details[3], Check = "+(A)", countPresence = 1 });
                            //count++;
                            //eW.showStudentet(details);
                        }
                        else
                        {
                            // unkoment
                            //byte[] data = Encoding.UTF8.GetBytes("exists");
                            //int length = data.Length;
                            //current.Send(Encoding.UTF8.GetBytes(length.ToString()));
                            //current.Send(data);
                        }
                    }
                    if (details[0].Equals("here"))
                    {
                        //var user = studentet.Single(x => x.Id == int.Parse(details[1].Trim()));
                        //user.countPresence = user.countPresence + 1;
                        //MessageBox.Show("Student " + details[1] + "is here" + user.countPresence);
                    }
                    if (false)//text.Equals(eW.validateStudent))
                    {
                        // unkoment
                        //byte[] data = Encoding.UTF8.GetBytes(defaultMessage);
                        //int length = data.Length;
                        //current.Send(Encoding.UTF8.GetBytes(length.ToString()));
                        //current.Send(data);
                    }
                    else
                    {
                        //foreach (var item in details)
                        //    MessageBox.Show(item.ToString());
                        byte[] data = Encoding.ASCII.GetBytes("mbyll");
                        int length = data.Length;
                        //current.Send(Encoding.UTF8.GetBytes(length.ToString()));
                        //current.Send(data);
                    }
                    current.BeginReceive(_buffer, 0, _BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
                }
                else
                {
                    try
                    {
                        //MessageBox.Show("Studenti " + current.LocalEndPoint.ToString() + " u diskonenkt");
                        stud = students.FirstOrDefault<student>(c => c.ip_Address.Equals(current.RemoteEndPoint.ToString()));
                        current.Close(); // Dont shutdown because the socket may be disposed and its disconnected anyway
                                         //klientet.Remove(current);
                        if (stud != null)
                        {
                            stud.isPresent = false;
                        }
                    }
                    catch { }
                }
            }
            catch (SocketException ex)
            {
                //MessageBox.Show(ex.Message);
                //MessageBox.Show("Studenti " + current.LocalEndPoint + " u diskonenkt");
                stud = students.FirstOrDefault<student>(c => c.ip_Address.Equals(current.RemoteEndPoint.ToString()));
                if (stud != null)
                {
                    stud.isPresent = false;
                    
                    //eW.Dispatcher.Invoke(() =>
                    //{
                    //    int dd = eW.listBox.Items.IndexOf(stu.name);
                    //    eW.listBox.Items.RemoveAt(dd);
                    //});
                }
                current.Close(); // Dont shutdown because the socket may be disposed and its disconnected anyway
                //klientet.Remove(current);
                return;
            }
        }
        private void closeServer()
        {
            try
            {
                serveri.Close();
            }
            catch { }
        }
    }
}
