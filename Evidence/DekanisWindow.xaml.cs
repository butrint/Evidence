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
    /// Interaction logic for DekanisWindow.xaml
    /// </summary>
    public partial class DekanisWindow : Window
    {
        private static TimeSpan startTimeSpan = TimeSpan.Zero;
        private static TimeSpan periodTimeSpan = TimeSpan.FromMinutes(1);
        private static DateTime now = new DateTime();
        private static ObservableCollection<TodayAllData> todayAllData = new ObservableCollection<TodayAllData>();
        private static bool canCheckSubs = false;

        public DekanisWindow(string idDekani)
        {
            InitializeComponent();
            string query = "SELECT * FROM `scheduler` WHERE DATE_FORMAT(start_time, '%Y-%m-%d') = CURRENT_DATE();";
            todayAllData = Methods.getTodaySubsProfs(query);
            now = Methods.datetimeInMysql();
            gridTodaySubs.ItemsSource = todayAllData;
        }

        // Ekzekutohet cdo 5 minuta
        System.Threading.Timer timer = new System.Threading.Timer((e) =>
        {
            if (canCheckSubs)
                Methods.checkIfSubjectHasStarted(todayAllData);
            canCheckSubs = true;
        }, null, startTimeSpan, periodTimeSpan);


    }
}
