using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for InsertSubstition.xaml
    /// </summary>
    public partial class InsertSubstition : Window
    {
        private static ObservableCollection<TodayAllData> scheduler = new ObservableCollection<TodayAllData>();
        public InsertSubstition()
        {
            InitializeComponent();

            scheduler = Methods.getScheduler();

            dGrid.ItemsSource = scheduler;
        }

        private void btnUpdateS_Click(object sender, RoutedEventArgs e)
        {
            bool canInsert = true;
            DateTime start_time = new DateTime();
            DateTime end_time = new DateTime();
            TodayAllData sched = (TodayAllData)dGrid.SelectedItem;
            try
            {
                start_time = DateTime.Parse(txtStartTime.Text);
                end_time = DateTime.Parse(txtEndTime.Text);
            }
            catch
            {
                canInsert = false;
                MessageBox.Show("Ju lutem shkruani daten ne formatin e duhur \n(Viti-Muaji-Dita Ora:Min:Sec p.sh 2017-06-20 08:00:00).");
            }
            
            string reason = txtReason.Text;
            int schedule_id = int.Parse(txtschedule_id.Text);
            string query = "INSERT INTO substitution(start_time, end_time, reason, schedule_id) VALUES ('" + start_time.ToString("yyyy-MM-dd hh:mm:ss")+"', '"+end_time.ToString("yyyy-MM-dd hh:mm:ss") + "', '"+reason+"', '"+ schedule_id+"')"; sched.start_Time = DateTime.Parse(txtStartTime.Text);
            string existsDB = "SELECT * FROM `substitution` WHERE `schedule_id`='" + schedule_id+"'";
            canInsert = canInsert && end_time > start_time;

            if (canInsert && !(Methods.existsInDBS(existsDB)))
            {
                Methods.updateOrInsertIntoTable(query);
                sched.start_Time = DateTime.Parse(txtStartTime.Text);
                sched.end_Time = DateTime.Parse(txtEndTime.Text);
            }
            else if (Methods.existsInDBS(existsDB))
                MessageBox.Show("Kjo lende tashme eshte zevendesuar!");
            else
                MessageBox.Show("Nuk mund te zevendesohet kjo lende!");
        }
    }
}
