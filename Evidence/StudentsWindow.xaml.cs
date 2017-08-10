using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for StudentsWindow.xaml
    /// </summary>
    public partial class StudentsWindow : Window
    {
        protected override void OnClosing(CancelEventArgs e)
        {
            if (MessageBox.Show("A jeni te sigurt qe doni ta mbyllni kete faqe?", "Konfirmoni", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }
        public StudentsWindow(int subject_ID, int lush_ID, int group_ID)
        {
            InitializeComponent();
            string getStudOnGroup = "SELECT st.num_id, st.name, st.surname FROM cslgs c INNER JOIN subjects sub ON sub.subject_id=c.subject_id " +
                                "INNER JOIN lush l ON l.lush_id=c.lush_id INNER JOIN groups g ON g.group_id=c.group_id " +
                                "INNER JOIN students st ON st.student_id=c.student_id WHERE c.subject_ID='" + subject_ID + "' " +
                                "AND c.lush_id='" + lush_ID + "' AND c.group_id='" + group_ID + "'";
            Methods.fillDGTextbox(gridStudents, getStudOnGroup);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog Printdlg = new System.Windows.Controls.PrintDialog();
            if ((bool)Printdlg.ShowDialog().GetValueOrDefault())
            {
                Size pageSize = new Size(Printdlg.PrintableAreaWidth, Printdlg.PrintableAreaHeight);
                // sizing of the element.
                gridStudents.Measure(pageSize);
                gridStudents.Arrange(new Rect(5, 5, pageSize.Width, pageSize.Height));
                Printdlg.PrintVisual(gridStudents, Title);
            }
        }
    }
}
