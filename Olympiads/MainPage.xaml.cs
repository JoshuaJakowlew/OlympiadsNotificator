using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HtmlAgilityPack;
using Windows.UI;
using Windows.UI.Core;
using Newtonsoft.Json;
using Windows.Storage;
using System.Threading.Tasks;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Olympiads
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<Olympiad> Olympiads = new List<Olympiad>();
        private List<ListViewItem> OlympiadList { get; set; } = new List<ListViewItem>();
        private Olympiad CurrentOlympiad { get; set; } = new Olympiad("", "", 0);

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            Parser parser = new Parser();
            Olympiads = parser.Olympiads;            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitializeOlympiadListView();
        }

        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CurrentOlympiad = (sender as ListViewItem)?.Tag as Olympiad;

            Bindings.Update();
        }

        private void GridView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Bindings.Update();
        }

        private void Link_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (CurrentOlympiad.Link != "")
            {
                Frame.Navigate(typeof(OlympiadWebsitePage), new Uri(CurrentOlympiad.Link));
            }
        }

        private async void ExtrenalLink_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var success = await Windows.System.Launcher.LaunchUriAsync(new Uri(CurrentOlympiad.Link));
        }

        private void InitializeOlympiadListView()
        {      
            bool colorflag = false;
            OlympiadList.Clear();
            foreach (var olympiad in Olympiads)
            {
                var listViewItem = new ListViewItem
                {
                    Content = new TextBlock { Text = olympiad.Name, TextWrapping = TextWrapping.WrapWholeWords },
                    Background = colorflag ? new SolidColorBrush(Colors.LightBlue) : new SolidColorBrush(Colors.Azure),
                    Tag = olympiad
                };

                colorflag = !colorflag;
                listViewItem.Tapped += ListView_Tapped;

                OlympiadList.Add(listViewItem);
            }

            CurrentOlympiad = OlympiadList.First().Tag as Olympiad;
            Bindings.Update();
        }

        private async Task SaveOlympiadsAsync()
        {
            string json = JsonConvert.SerializeObject(Olympiads);

            var storageFolder = ApplicationData.Current.LocalFolder;
            var file = await storageFolder.CreateFileAsync("data.json", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, json);
        }

        private async Task LoadOlympiadsAsync()
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var file = await storageFolder.GetFileAsync("data.json");
            string json = await FileIO.ReadTextAsync(file);

            Olympiads = JsonConvert.DeserializeObject<List<Olympiad>>(json);
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await SaveOlympiadsAsync();
        }

        private async void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            await LoadOlympiadsAsync();
            InitializeOlympiadListView();
        }
    }
}
