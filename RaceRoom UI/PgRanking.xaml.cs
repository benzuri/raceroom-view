using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RaceRoom_UI
{
    /// <summary>
    /// Lógica de interacción para PgRanking.xaml
    /// </summary>
    public partial class PgRanking : Page
    {
        string _url = "https://game.raceroom.com/multiplayer-rating/";
        int num = 0;
        public PgRanking()
        {
            InitializeComponent();

            LoadData();
        }

        public async void LoadData()
        {
            loading.Visibility = Visibility.Visible;
            rankingList.Visibility = Visibility.Hidden;

            try
            {
                List<string> drivers = new List<string>();

                HtmlDocument doc = await new HtmlWeb().LoadFromWebAsync(_url+ "1.html");
                HtmlDocument doc2 = await new HtmlWeb().LoadFromWebAsync(_url+ "2.html");

                var rows = doc.DocumentNode
                  .SelectNodes("//tbody//tr")
                  .Select((z, i) => new
                  {
                      RowNumber = i,
                      Cells = z.ChildNodes.Where(c => c.NodeType == HtmlNodeType.Element)
                  })
                  .ToList();
                var rows2 = doc2.DocumentNode
                  .SelectNodes("//tbody//tr")
                  .Select((z, i) => new
                  {
                      RowNumber = i,
                      Cells = z.ChildNodes.Where(c => c.NodeType == HtmlNodeType.Element)
                  })
                  .ToList();
                rows.AddRange(rows2);

                rows.ForEach(row => AddData(row.Cells.Select(z => z.InnerText.Trim())));

                loading.Visibility = Visibility.Hidden;
                rankingList.Visibility = Visibility.Visible;
            }
            catch (Exception e)
            {
                loading.Visibility = Visibility.Hidden;
            }
        }

        public async void SearchData(string text)
        {
            loading.Visibility = Visibility.Visible;
            rankingList.Visibility = Visibility.Hidden;

            try
            {
                HtmlDocument doc = await new HtmlWeb().LoadFromWebAsync(_url + "1.html");

                var rows = doc.DocumentNode
                  .SelectNodes("//tbody//tr")
                  .Select((z, i) => new
                  {
                      RowNumber = i,
                      Cells = z.ChildNodes.Where(c => c.NodeType == HtmlNodeType.Element)
                  })
                  .ToList();

                for (int i = 2; i < 14; i++)
                {
                    HtmlDocument doci = await new HtmlWeb().LoadFromWebAsync(_url + i + ".html");

                    var rowsi = doci.DocumentNode
                      .SelectNodes("//tbody//tr")
                      .Select((z, x) => new
                      {
                          RowNumber = x,
                          Cells = z.ChildNodes.Where(c => c.NodeType == HtmlNodeType.Element)
                      })
                      .ToList();

                    rows.AddRange(rowsi);
                }                

                rows.ForEach(row => AddData(row.Cells.Select(z => z.InnerText.Trim())));

                loading.Visibility = Visibility.Hidden;
                rankingList.Visibility = Visibility.Visible;
            }
            catch (Exception e)
            {
                loading.Visibility = Visibility.Hidden;
            }
        }

        private void AddData(IEnumerable<string> enumerable)
        {
            //Console.WriteLine(drivers.Count());
            StackPanel stackContent = new StackPanel();

            stackContent.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fff"));
            stackContent.Background.Opacity = 0.85;
            stackContent.Orientation = Orientation.Horizontal;

            List<string> drivers = enumerable.ToList();
            
            Label lbl1 = new Label();
            Label lbl2 = new Label();
            Label lbl3 = new Label();
            Label lbl4 = new Label();
            Label lbl5 = new Label();

            lbl1.Content = drivers[2].Trim().Replace("#", "");
            lbl1.FontSize = 12;
            lbl1.Foreground = new SolidColorBrush(Colors.Black);

            lbl2.Content = drivers[3].ToString();
            lbl2.FontSize = 12;
            lbl2.Foreground = new SolidColorBrush(Colors.Black);

            lbl3.Content = drivers[5].ToString();
            lbl3.FontSize = 12;
            lbl3.Foreground = new SolidColorBrush(Colors.Black);

            lbl4.Content = drivers[6].ToString();
            lbl4.FontSize = 12;
            lbl4.Foreground = new SolidColorBrush(Colors.Black);

            lbl5.Content = drivers[7].ToString();
            lbl5.FontSize = 12;
            lbl5.Foreground = new SolidColorBrush(Colors.Black);

            stackContent.Children.Add(lbl1);
            stackContent.Children.Add(lbl2);
            stackContent.Children.Add(lbl3);
            stackContent.Children.Add(lbl4);
            stackContent.Children.Add(lbl5);

            if (drivers[3].ToString().ToLower().Contains(filterText.Text.ToLower()) )
            {
                contentGrid.RowDefinitions.Add(new RowDefinition());
                contentGrid.Children.Add(stackContent);
                stackContent.SetValue(Grid.RowProperty, num);
                num++;
            }
        }
        private void FilterDriver(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(filterText.Text);
            contentGrid.Children.Clear();

            SearchData(filterText.Text);

            if (filterText.Text == "")
            {
                LoadData();
            }                
        }
        private void Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                FilterDriver(null, null);
            }
        }

    }
}
