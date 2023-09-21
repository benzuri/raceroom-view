using System;
using System.Windows;
using System.Windows.Input;
using System.Reflection;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace RaceRoom_UI
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class InitWindow : Window
    {
        public InitWindow()
        {
            InitializeComponent();

            // set version in text
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string ver = fileVersionInfo.ProductVersion;
            version.Text = "Raceroom View ";

            // load grid
            if (Properties.Settings.Default.Username != "")
                GoToMain();
            else
                ShowLogin();
        }

        private void ShowLogin()
        {
            login.Visibility = Visibility.Visible;
            username.Focus();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Acept(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default["Username"] = username.Text;
            Properties.Settings.Default.Save();

            loading.Visibility = Visibility.Visible;
            login.Visibility = Visibility.Hidden;

            GoToMain();
        }

        private void Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Properties.Settings.Default["Username"] = username.Text;
                Properties.Settings.Default.Save();

                loading.Visibility = Visibility.Visible;
                login.Visibility = Visibility.Hidden;

                GoToMain();
            }
        }

        public async void GoToMain()
        {
            loading.Visibility = Visibility.Visible;
            login.Visibility = Visibility.Hidden;

            int statusCode = 200;
            string status;
            bool statusUser = true;

            try
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage responseC = await client.GetAsync("https://game.raceroom.com/users/"+ Properties.Settings.Default["Username"] + "/career?json");

                statusCode = (int)responseC.StatusCode;
                status = responseC.StatusCode.ToString();
                responseC.EnsureSuccessStatusCode();

                HttpResponseMessage responseP = await client.GetAsync("https://game.raceroom.com/users/" + Properties.Settings.Default["Username"] + "/purchases?json");
                statusCode = (int)responseP.StatusCode;
                status = responseP.StatusCode.ToString();
                responseP.EnsureSuccessStatusCode();

                HttpResponseMessage responseT = await client.GetAsync("https://game.raceroom.com/store/tracks/?json");
                statusCode = (int)responseT.StatusCode;
                status = responseT.StatusCode.ToString();
                responseT.EnsureSuccessStatusCode();

                string responseCareer = await responseC.Content.ReadAsStringAsync();
                string responsePurchases = await responseP.Content.ReadAsStringAsync();
                string responseTracks = await responseT.Content.ReadAsStringAsync();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue; //important
                dynamic myCareer = serializer.DeserializeObject(responseCareer);
                dynamic myPurchases = serializer.DeserializeObject(responsePurchases);
                dynamic myTracks = serializer.DeserializeObject(responseTracks);

                statusUser = false;

                // open new window and close actual (pass user data)
                MainWindow _main = new MainWindow(myCareer, myPurchases, myTracks);
                _main.Show();
                this.Close();
            }
            catch (Exception e)
            {
                info.Content = "This username is not valid";
                loading.Visibility = Visibility.Hidden;
                login.Visibility = Visibility.Visible;
                username.Focus();
            }

            //Console.WriteLine(statusCode);
            if (!(statusCode >= 200 && statusCode < 300) && statusUser ==  true)
            {
                info.Content = "Service Temporarily Unavailable";
            }
            if (statusCode == 404)
            {
                info.Content = "This username is not valid";
            }
           
        }

    }
}
