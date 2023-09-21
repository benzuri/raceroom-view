using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Windows.Threading;
using System.Diagnostics;

namespace RaceRoom_UI
{
    /// <summary>
    /// Lógica de interacción para PgDrive.xaml
    /// </summary>
    /// 
    public partial class PgDrive : Page
    {
        private dynamic _myTracks, _myPurchases;
        string _url = "https://game.raceroom.com/multiplayer-rating/servers/";
        int num = 0;

        DispatcherTimer _timer;

        public PgDrive(dynamic myTracks, dynamic myPurchases)
        {
            this._myTracks = myTracks;
            this._myPurchases = myPurchases;

            InitializeComponent();

            LoadData();
        }

        private void Countdown(int seconds, Label label, Button btn, StackPanel stack)
        {
            TimeSpan _time;
            _time = TimeSpan.FromSeconds(seconds);

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                label.Content = _time.ToString("c");
                if (_time == TimeSpan.Zero)
                {
                    //_timer.Stop(); //TODO
                    label.Visibility = Visibility.Hidden;
                    btn.IsEnabled = false;
                    stack.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fff"));
                    stack.Background.Opacity = 0.7;
                }
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);

            _timer.Start();
        }

        public async void LoadData()
        {
            //loading.Visibility = Visibility.Visible;

            try
            {
                //https://game.raceroom.com/users/{USER}/purchases?json

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(_url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic myData = serializer.DeserializeObject(responseBody);

                loading.Visibility = Visibility.Hidden;

                AddHeader();

                foreach (dynamic data in myData["result"])
                {
                    AddData(data);
                }
            }
            catch (Exception e)
            {
                // No info
                loading.Visibility = Visibility.Hidden;
            }            
        }

        private void AddHeader()
        {
            var font = 16;

            StackPanel stackHeader = new StackPanel();
            
            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Colors.Black);
            grid.Background.Opacity = 0.6;
            ColumnDefinition c1 = new ColumnDefinition();
            c1.Width = new GridLength(2, GridUnitType.Star);
            grid.ColumnDefinitions.Add(c1);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            RowDefinition row = new RowDefinition();

            Label lbl1 = new Label();
            Label lbl2 = new Label();
            Label lbl3 = new Label();
            Label lbl4 = new Label();
            Label lbl5 = new Label();
            Label lbl6 = new Label();
            Label lbl7 = new Label();

            lbl1.Content = "Name";
            lbl1.FontSize = font;
            lbl1.Foreground = new SolidColorBrush(Colors.White);
            lbl1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            lbl2.Content = "Duration";
            lbl2.FontSize = font;
            lbl2.Foreground = new SolidColorBrush(Colors.White);
            lbl2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            lbl3.Content = "Session";
            lbl3.FontSize = font;
            lbl3.Foreground = new SolidColorBrush(Colors.White);
            lbl3.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            lbl4.Content = "Time left";
            lbl4.FontSize = font;
            lbl4.Foreground = new SolidColorBrush(Colors.White);
            lbl4.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            lbl5.Content = "Drivers";
            lbl5.FontSize = font;
            lbl5.Foreground = new SolidColorBrush(Colors.White);
            lbl5.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            lbl6.Content = "Track";
            lbl6.FontSize = font;
            lbl6.Foreground = new SolidColorBrush(Colors.White);
            lbl6.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            stackHeader.Children.Add(grid);

            Grid.SetColumn(lbl1, 0);
            Grid.SetRow(lbl1, 0);
            grid.Children.Add(lbl1);

            Grid.SetColumn(lbl2, 1);
            Grid.SetRow(lbl2, 0);
            grid.Children.Add(lbl2);

            Grid.SetColumn(lbl3, 2);
            Grid.SetRow(lbl3, 0);
            grid.Children.Add(lbl3);

            Grid.SetColumn(lbl4, 3);
            Grid.SetRow(lbl4, 0);
            grid.Children.Add(lbl4);

            Grid.SetColumn(lbl5, 4);
            Grid.SetRow(lbl5, 0);
            grid.Children.Add(lbl5);

            Grid.SetColumn(lbl6, 5);
            Grid.SetRow(lbl6, 0);
            grid.Children.Add(lbl6);

            Grid.SetColumn(lbl7, 6);
            Grid.SetRow(lbl7, 0);
            grid.Children.Add(lbl7);

            headerGrid.RowDefinitions.Add(new RowDefinition());
            headerGrid.Children.Add(stackHeader);
        }

        private void AddData(dynamic data)
        {
            var font = 14;
            StackPanel stackContent = new StackPanel();
            StackPanel stackDuration = new StackPanel();
            StackPanel stackPlayers = new StackPanel();

            Grid grid = new Grid();
            ColumnDefinition c1 = new ColumnDefinition();
            c1.Width = new GridLength(2, GridUnitType.Star);
            grid.ColumnDefinitions.Add(c1);
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());

            RowDefinition row = new RowDefinition();

            Label lbl1 = new Label();
            Label lbl2 = new Label();
            Label lbl3 = new Label();
            Label lbl4 = new Label();
            Label lbl5 = new Label();
            Label lbl6 = new Label();
            Label lbl7 = new Label();
            Label lbl8 = new Label();
            Button btn1 = new Button();

            stackContent.Children.Add(grid);
            stackDuration.Children.Add(lbl2);
            stackDuration.Children.Add(lbl3);
            stackDuration.Children.Add(lbl4);
            stackPlayers.Children.Add(lbl7);
            //stackPlayers.Children.Add(lbl8);

            Grid.SetColumn(lbl1, 0);
            Grid.SetRow(lbl1, 0);
            grid.Children.Add(lbl1);

            Grid.SetColumn(stackDuration, 1);
            Grid.SetRow(stackDuration, 0);
            grid.Children.Add(stackDuration);

            Grid.SetColumn(lbl5, 3);
            Grid.SetRow(lbl5, 0);
            grid.Children.Add(lbl5);

            Grid.SetColumn(lbl6, 2);
            Grid.SetRow(lbl6, 0);
            grid.Children.Add(lbl6);

            Grid.SetColumn(lbl8, 5);
            Grid.SetRow(lbl8, 0);
            grid.Children.Add(lbl8);

            Grid.SetColumn(btn1, 6);
            Grid.SetRow(btn1, 0);
            grid.Children.Add(btn1);

            Grid.SetColumn(stackPlayers, 4);
            Grid.SetRow(stackPlayers, 0);
            grid.Children.Add(stackPlayers);

            #region StackPanel Properties
            stackContent.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fff"));
            stackContent.Background.Opacity = 0.85;
            stackDuration.Orientation = Orientation.Horizontal;
            stackPlayers.Orientation = Orientation.Horizontal;

            if (data["Server"]["CurrentSession"] != 0)
            {
                btn1.IsEnabled = false;
                stackContent.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fff"));
                stackContent.Background.Opacity = 0.7;
            }
            else
            {
                //lbl1.FontWeight = FontWeights.Bold;
            }
            #endregion

            #region DockPanel Content Properties
            lbl1.Content = data["Server"]["Settings"]["ServerName"];
            lbl1.FontSize = font;
            lbl1.Foreground = new SolidColorBrush(Colors.Black);
            lbl1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbl1.VerticalAlignment = VerticalAlignment.Center;

            lbl2.Content = data["Server"]["Settings"]["PracticeDuration"];
            lbl2.ToolTip = "Practice duration";
            lbl2.FontSize = font;
            lbl2.Foreground = new SolidColorBrush(Colors.DarkGray);
            lbl2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbl2.VerticalAlignment = VerticalAlignment.Center;

            lbl3.Content = data["Server"]["Settings"]["QualifyDuration"];
            lbl3.ToolTip = "Qualify duration";
            lbl3.FontSize = font;
            lbl3.Foreground = new SolidColorBrush(Colors.DarkGray);
            lbl3.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbl3.VerticalAlignment = VerticalAlignment.Center;

            lbl4.Content = data["Server"]["Settings"]["Race1Duration"];
            lbl4.ToolTip = "Race duration";
            lbl4.FontSize = font;
            lbl4.Foreground = new SolidColorBrush(Colors.DarkGray);
            lbl4.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbl4.VerticalAlignment = VerticalAlignment.Center;

            //lbl5.Content = data["Server"]["TimeLeft"];
            int seconds = data["Server"]["TimeLeft"] / 1000;
            if (seconds > 0)
            {
                Countdown(seconds, lbl5, btn1, stackContent);
                lbl5.ToolTip = "Time left";
                lbl5.FontSize = font;
                lbl5.Foreground = new SolidColorBrush(Colors.DarkGray);
                lbl5.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lbl5.VerticalAlignment = VerticalAlignment.Center;
            } else
            {
                lbl5.Visibility = Visibility.Hidden;
            }

            string session = "Practice";
            if (data["Server"]["CurrentSession"] == 256) { session = "Qualify"; }
            if (data["Server"]["CurrentSession"] == 768) { session = "Race"; }
            lbl6.Content = session;
            lbl6.ToolTip = "Current session";
            lbl6.FontSize = font;
            lbl6.Foreground = new SolidColorBrush(Colors.DarkGray);
            lbl6.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbl6.VerticalAlignment = VerticalAlignment.Center;

            lbl7.Content = data["Server"]["PlayersOnServer"] + "/" + data["Server"]["Settings"]["MaxNumberOfPlayers"];
            lbl7.ToolTip = "Drivers on server";
            lbl7.FontSize = font;
            lbl7.Foreground = new SolidColorBrush(Colors.DarkGray);
            lbl7.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbl7.VerticalAlignment = VerticalAlignment.Center;

            // TODO TrackLayoutId
            //Console.WriteLine(data["Server"]["Settings"]["TrackLayoutId"][0]);
            //Console.WriteLine(_myTracks["context"]["c"]["sections"][0]["items"][0]["id"]);
            GetTrackNameAsync(lbl8,(int)data["Server"]["Settings"]["TrackLayoutId"][0]);
            lbl8.ToolTip = "Track";
            lbl8.FontSize = font;
            lbl8.Foreground = new SolidColorBrush(Colors.DarkGray);
            lbl8.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            lbl8.VerticalAlignment = VerticalAlignment.Center;

            btn1.Content = "Join Server";
            btn1.Style = (Style)FindResource("RaceroomButton");
            btn1.Height = 36;
            btn1.Width = 100;
            btn1.FontSize = 16;
            btn1.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            btn1.SetValue(DockPanel.DockProperty, Dock.Right);

            string ip = data["Server"]["ServerIp"];
            int port = data["Server"]["Port"];

            List<dynamic> serverList = new List<dynamic> { ip, port };
            btn1.Click += (object sender, RoutedEventArgs e) => { Btn1_JoinServer(sender, e, serverList); };

            #endregion

            contentGrid.RowDefinitions.Add(new RowDefinition());
            contentGrid.Children.Add(stackContent);
            stackContent.SetValue(Grid.RowProperty, num);
            num++;
        }

        void Btn1_JoinServer(object sender, RoutedEventArgs e, List<dynamic> serverList)
        {
            string ip = serverList[0];
            string port = serverList[1].ToString();
            Process.Start("rrre://multiplayer/join?data=%7B%22MultiplayerJoin%22%3A%7B%22Address%22%3A%22" + ip + "%3A" + port + "%22%7D%7D&");
        }

        public async Task GetTrackNameAsync(Label lbl,int trackLayoutId)
        {
            string trackName = "";
            foreach (dynamic track in _myTracks["context"]["c"]["sections"][0]["items"])
            {
                int id1 = trackLayoutId - 1;
                int id2 = (int)track["cid"];

                if (id1 == id2)
                {
                    trackName = track["name"];
                }
            }

            if (trackName == "")
            {
                //Console.WriteLine(trackLayoutId);
                try
                {
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync("https://game.raceroom.com/leaderboard/?track=" + trackLayoutId + "&json");
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic myTrack = serializer.DeserializeObject(responseBody);

                    trackName = myTrack["context"]["c"]["current_combination"]["track"]["name"];
                }
                catch (Exception e)
                {
                    Console.WriteLine("No track name");
                }

            }
            Console.WriteLine(trackName);
            lbl.Content = trackName;
        }
    }
}
