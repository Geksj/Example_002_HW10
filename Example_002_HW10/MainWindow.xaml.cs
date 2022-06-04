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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;

namespace Example_002_HW10
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataWeather dataWeather = new DataWeather();
        ObservableCollection<DataWeather> dw = new ObservableCollection<DataWeather>();
        readonly string key = "04e82c8ee17396e8b0c8bf0417ddfe8a";
        public MainWindow()
        {
            InitializeComponent();
            listBoxDb.ItemsSource = dw;
        }

        
        
        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (inputSearch.Text != null)
            {
                string url = $@"https://api.openweathermap.org/data/2.5/weather?q={inputSearch.Text}&appid={key}&units=metric";
                DownLoadString(url);
                
            }
        }

        private void DownLoadString(string url)
        {
            WebClient wc = new WebClient();
            string reply = wc.DownloadString(url);
            using (StreamWriter sw = new StreamWriter("_fileInfo.json"))
            {
                sw.WriteLine(reply);
            }
            Deserialize();
        }

        private void Deserialize()
        {
            string json = File.ReadAllText("_fileInfo.json");
            var weather = JObject.Parse(json)["weather"].ToArray();

            dataWeather.Temperature = JObject.Parse(json)["main"]["temp"].ToString();
            dataWeather.Humidity = JObject.Parse(json)["main"]["humidity"].ToString();

            foreach (var item in weather)
            {
                dataWeather.Description = item["main"].ToString();
            }
            dw.Add(new DataWeather()
            {
                NameCity = inputSearch.Text,
                Description = dataWeather.Description,
                Temperature = dataWeather.Temperature,
                Humidity = dataWeather.Humidity
            });
        }

    }
}
