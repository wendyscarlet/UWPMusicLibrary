using MusicLibraryApp.AppDialogs;
using MusicLibraryApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MusicLibraryApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SongsDAO songDao;
        private MediaSource _mediaSource;

        public MainPage()
        {

            this.InitializeComponent();
            UpdateGreeting();
            songDao = new SongsDAO();
            songDao.GetAllSongs();
            this.DataContext = songDao;

        }

        private async void SoundGridView_ItemClick(object sender, ItemClickEventArgs e)
        {

            Song songInContext = (Song)e.ClickedItem;
            
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.GetFileAsync(songInContext.SongFileName);
            if (file != null)
            {
                _mediaSource = MediaSource.CreateFromStorageFile(file);
                this.mediaPlayer.SetPlaybackSource(_mediaSource);
                this.mediaPlayer.AutoPlay = true;
            }

           // PauseButton.Visibility = Visibility.Visible;
            //play song item

        }
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
          //  PauseButton.Visibility = Visibility.Collapsed;
          //  PlayButton.Visibility = Visibility.Visible;

        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            //  PauseButton.Visibility = Visibility.Collapsed;
            //  PlayButton.Visibility = Visibility.Visible;

        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            Search.Visibility = MySplitView.IsPaneOpen ? Visibility.Collapsed : Visibility.Visible;
            //if (MySplitView.IsPaneOpen == true)
            //{
            //    SearchAutoSuggestBox.Width = 200;
            //    SearchAutoSuggestBox.Margin = new Thickness(10, 0, 0, 0);
            //}
        }


        private void DisplaySongList_Click(object sender, RoutedEventArgs e)
        {
           // MySplitView.IsPaneOpen = true;
           // SearchAutoSuggestBox.Width = 200;
           // Search.Visibility = Visibility.Collapsed;
           // SearchAutoSuggestBox.Margin = new Thickness(10, 0, 0, 0);
           // this.DataContext = vm.SongsList;
        }

     

        private void SearchSongButton_Click(object sender, RoutedEventArgs e)
        {

            MySplitView.IsPaneOpen = true;
            Search.Visibility = Visibility.Collapsed;
        }

        private async void AddSongButton_Click(object sender, RoutedEventArgs e)
        {
        //    MySplitView.IsPaneOpen = true;
        //    SearchAutoSuggestBox.Width = 200;
        //    SearchAutoSuggestBox.Margin = new Thickness(10, 0, 0, 0);
        //    Search.Visibility = Visibility.Collapsed;
            var dialog = new ContentDialog1();
            await dialog.ShowAsync();
        }

        private void SongGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            //MySplitView.IsPaneOpen = true;
            //SearchAutoSuggestBox.Width = 200;
            //Search.Visibility = Visibility.Collapsed;
            //SearchAutoSuggestBox.Margin = new Thickness(10, 0, 0, 0);

        }

        private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

            if (SearchAutoSuggestBox.Text.Trim() != "")
            {
                songDao.SearchSongs(SearchAutoSuggestBox.Text);
                this.SongGridView.ItemsSource = songDao.songsList;
            }
            else
            {
                songDao.GetAllSongs();
                this.SongGridView.ItemsSource = songDao.songsList;
            }
            MySplitView.IsPaneOpen = false;
            Search.Visibility = Visibility.Visible;
        }


        private void UpdateGreeting()
        {
            var now = DateTime.Now;
            var greeting =
                now.Hour < 12 ? "Good Morning!" :
                now.Hour < 18 ? "Good Afternoon!" :
                /* otherwise */ "Good Evening!";

            TextGreeting.Text = $"{greeting}";

        }

    }
}
