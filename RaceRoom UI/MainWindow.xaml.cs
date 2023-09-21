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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace RaceRoom_UI
{
    public partial class MainWindow : Window
    {
        private dynamic _myCareer, _myPurchases, _myTracks;
        private string _urlStore = "https://game.raceroom.com/store";
        private string _urlCareer = "https://game.raceroom.com/users/"+ Properties.Settings.Default["Username"] + "/career";
        //private string _urlRanking = "https://game.raceroom.com/multiplayer-rating/";
        private string _urlRaceroom = "steam://run/211500";
        private string _urlTwitter = "https://twitter.com/raceroom";
        private string _urlTwitch = "https://www.twitch.tv/directory/game/RaceRoom%20Racing%20Experience";
        private string _urlYoutube = "https://www.youtube.com/channel/UCjv1g76ZIWerlVyZt_zDGoA";
        private string _urlFacebook = "https://www.facebook.com/raceroom";

        public MainWindow(dynamic myCareer, dynamic myPurchases, dynamic myTracks)
        {
            this._myCareer = myCareer;
            this._myPurchases = myPurchases;
            this._myTracks = myTracks;

            InitializeComponent();

            //SetWallpaper();
            
            SetHeaderData();

            PgHome home = new PgHome();
            content.Navigate(home);
        }

        private void SetWallpaper()
        {
            ImageBrush myBrush = new ImageBrush();
            Image image = new Image();
            image.Source = new BitmapImage(
                new Uri("https://prod.r3eassets.com/assets/content/track/monza-circuit-1670-signature-original.png"));
            myBrush.ImageSource = image.Source;
            myBrush.Stretch = Stretch.UniformToFill;

            //body.Opacity = 0; //transition animation
            body.Background = myBrush;

            /*
            //transition animation
            var animation = new DoubleAnimation
            {
                To = 0.4,
                BeginTime = TimeSpan.FromSeconds(0.3),
                Duration = TimeSpan.FromSeconds(3),
                FillBehavior = FillBehavior.Stop
            };

            animation.Completed += (s, a) => body.Opacity = 0.4;

            body.BeginAnimation(UIElement.OpacityProperty, animation);
            */
        }

        private void SetHeaderData()
        {
            name.Header = _myCareer["context"]["c"]["name"];

            rank.Content = _myCareer["context"]["c"]["competition_rank"];
            
            //avatar
            var fullFilePath = _myCareer["context"]["c"]["avatar"];
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();
            avatar.Source = bitmap;
            avatar.ToolTip = _myCareer["context"]["c"]["name"];
            
            int _totalEntries = _myCareer["context"]["c"]["raceList"]["GetUserMpRatingProgressResult"]["TotalEntries"]-1;
            if (_totalEntries >= 0)
            {
                int _i = _totalEntries > 100 ? 99 : _totalEntries; //only have 100 last races
                rating.Content = _myCareer["context"]["c"]["raceList"]["GetUserMpRatingProgressResult"]["Entries"][_i]["RatingAfter"];
                reputation.Content = _myCareer["context"]["c"]["raceList"]["GetUserMpRatingProgressResult"]["Entries"][_i]["ReputationAfter"];
                races.Content = _myCareer["context"]["c"]["raceList"]["GetUserMpRatingProgressResult"]["TotalEntries"];
            }
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            // reset username
            Properties.Settings.Default.Reset();
            // open new window and close actual (login)
            InitWindow _init = new InitWindow();
            _init.Show();
            this.Close();
        }

        private void GoToPage(object sender, RoutedEventArgs e)
        {
            PgHome home = new PgHome();
            switch (((Button)sender).Tag)
            {
                case "home":
                    content.Navigate(home);
                    break;
                case "drive":
                    PgDrive drive = new PgDrive(_myTracks,_myPurchases);
                    content.Navigate(drive);
                    break;
                case "ranking":
                    //PgWebview ranking = new PgWebview(_urlRanking);
                    PgRanking ranking = new PgRanking();
                    content.Navigate(ranking);
                    break;
                case "career":
                    PgWebview career = new PgWebview(_urlCareer);
                    content.Navigate(career);
                    break;
                case "store":
                    PgWebview store = new PgWebview(_urlStore);
                    content.Navigate(store);
                    break;
                case "twitch":
                    PgWebview twitch = new PgWebview(_urlTwitch);
                    content.Navigate(twitch);
                    break;
                    
                default:
                    content.Navigate(home);
                    break;
            }
        }

        private void GoToUrl(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Tag)
            {
                case "raceroom":
                    Process.Start(_urlRaceroom);
                    break;
                case "twitter":
                    Process.Start(_urlTwitter);
                    break;
                case "twitch":
                    Process.Start(_urlTwitch);
                    break;
                case "youtube":
                    Process.Start(_urlYoutube);
                    break;
                case "facebook":
                    Process.Start(_urlFacebook);
                    break;
                default:
                    Process.Start(_urlTwitch);
                    break;
            }
        }
    }
}
