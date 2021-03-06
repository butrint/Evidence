﻿using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;


namespace Evidence
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string myConnectionString = "Data Source=localhost;Initial Catalog=vijueshmeria;User ID=root;Password=";
        private static string username;
        private static string password;
        private static string passwordMD5;
        private static string idPerdoruesi;
        private static int idRoli;
        private bool conn = false;

        //string myConnectionString = "Data Source=localhost;Initial Catalog=vijueshmeria;User ID=root;Password=";

        public MainWindow()
        {
            InitializeComponent();
            Username.Focus();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            checkLogin();
        }

        public bool ReadMyData(string myConnString, string user, string pass)
        {
            int rowCounter = 0;
            MySqlConnection myConnection = new MySqlConnection(myConnString);
            MySqlCommand myCommand = (MySqlCommand)myConnection.CreateCommand();

            myCommand.CommandText = "SELECT * FROM users WHERE username='" + user + "' AND password='" + pass + "'";
            try
            {
                myConnection.Open();
                MySqlDataReader myReader = myCommand.ExecuteReader();
                try
                {
                    while (myReader.Read())
                    {
                        rowCounter++;
                        idPerdoruesi = myReader.GetInt32(0).ToString();
                        idRoli = myReader.GetInt32(8);
                    }
                }
                finally
                {
                    myReader.Close();
                    myConnection.Close();
                }
                conn = true;
            }
            catch (MySqlException)
            {
                conn = false;
                MessageBox.Show("Për momentin lidhja dështoi. Provoni më vonë");
            }
            finally
            { }
            if (rowCounter > 0)
                return true;
            else
                return false;

        }
        
        private void Panel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                checkLogin();
            }
        }

        private void checkLogin()
        {
            username = Username.Text.ToString();
            password = Password.Password.ToString();
            passwordMD5 = Methods.Encode(password);
            if (username.Length == 0 || password.Length == 0)
            {
                Error.Content = "Duhet t'i plotësoni të gjitha fushat!";
            }
            else if (username.Length == 0 || username.Length < 4)
            {
                Error.Content = "Kontrolloni username-in, duhet të jetë më shumë se 3 karaktere!";
            }
            else if (password.Length == 0 || password.Length < 6)
            {
                Error.Content = "Kontrolloni fjalëkalimin, duhet të jetë më shumë se 6 karaktere!";
            }
            else
            {
                bool res = ReadMyData(myConnectionString, username, passwordMD5);
                if (res)
                {
                    if (idRoli == 2)
                    {
                        //AdminWindow aW = new AdminWindow(idPerdoruesi);
                        EvidenceWindow aW = new EvidenceWindow(idPerdoruesi);
                        aW.Show();
                    }
                    else if (idRoli == 3)
                    {
                        AdminPage dekaniWindow = new AdminPage(idPerdoruesi);
                        dekaniWindow.Show();
                    }
                    else
                    {
                        //InsertSubstition isW = new InsertSubstition();
                        //isW.Show();
                        //AdminWindow aW = new AdminWindow(idPerdoruesi);
                        //aW.Show();
                        //AdminPage dekaniWindow = new AdminPage(idPerdoruesi);
                        //dekaniWindow.Show();
                        //RaportiWindow rW = new RaportiWindow();
                        //rW.Show();
                    }
                    this.Close();
                }
                else
                    if (conn)
                        Error.Content = "Gabim në të dhëna";
            }
        }
    }
}
