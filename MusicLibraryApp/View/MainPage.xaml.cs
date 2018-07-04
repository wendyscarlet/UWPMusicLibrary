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
using Windows.Storage.Streams;
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
        private MainViewModel vm;
        //private MediaSource _mediaSource;
        bool playing;

        public MainPage()
        {
            this.InitializeComponent();
            vm = new MainViewModel();
            vm.GetAllSongs();
            this.DataContext = vm;
            playing = false;
           

        }

        private void UpdateGreeting()
        {
            var now = DateTime.Now;
            var greeting =
                now.Hour < 12 ? "Good Morning!" :
                now.Hour < 18 ? "Good Afternoon!" :
                /* otherwise */ "Good Evening!";

           // TextGreeting.Text = $"{greeting}";

        }

        private async void SoundGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Song songInContext = (Song)e.ClickedItem;

            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.GetFileAsync(songInContext.SongFileName);
            if (file != null)
            {
                //_mediaSource = MediaSource.CreateFromStorageFile(file);
                //this.mediaPlayer.SetPlaybackSource(_mediaSource);
                //this.mediaPlayer.AutoPlay = true;
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                MyMediaElement.SetSource(stream, file.ContentType);
            }
            if(playing)
            {
                MyMediaElement.AutoPlay = false;
                playing = false;
            }
            else
            {
                playing = true;
                MyMediaElement.AutoPlay = true;

            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            Search.Visibility = MySplitView.IsPaneOpen ? Visibility.Collapsed : Visibility.Visible;
            PlayListNames.Visibility = MySplitView.IsPaneOpen ? Visibility.Visible : Visibility.Collapsed;

        }
         
            private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
            {
                if (SearchAutoSuggestBox.Text.Trim() != "")
                {
                    vm.SearchSongs(SearchAutoSuggestBox.Text);
                    this.SongGridView.ItemsSource = vm.songsList;
                }
                else {
                    vm.GetAllSongs();
                    this.SongGridView.ItemsSource = vm.songsList;
                }
                MySplitView.IsPaneOpen = false;
                Search.Visibility = Visibility.Visible;

            }

            private void DisplaySongList_Click(object sender, RoutedEventArgs e)
            {
                vm.GetAllSongs();
                this.DataContext = vm;
            }

            private async void AddSongButton_Click(object sender, RoutedEventArgs e)
            {
                var dialog = new ContentDialog1();
                await dialog.ShowAsync();
            }

            private void SongGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {

            }

            private void SearchSongButton_Click(object sender, RoutedEventArgs e)
            {
                vm.GetAllSongs();
                MySplitView.IsPaneOpen = true;
                Search.Visibility = Visibility.Collapsed;
                SearchAutoSuggestBox.Text = "";
            }

        private void MySplitView_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            Search.Visibility = Visibility.Visible;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyMediaElement.DefaultPlaybackRate != 1)
            {
                MyMediaElement.DefaultPlaybackRate = 1.0;
            }
            MyMediaElement.Play();
            
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Pause();
            
        }

        private void PlayListsButton_Click(object sender, RoutedEventArgs e)
        {
            vm.DisplayAllPlaylists();
            MySplitView.IsPaneOpen = true;
            PlayListNames.Visibility = Visibility.Visible;
        }

        private async void AddPlayListButton_Click(object sender, RoutedEventArgs e)
        {
            //call addplaylist
            var AddPlayListDialog = new AddPlaylist();
            var result = await AddPlayListDialog.ShowAsync();
            //if add was selected
            if (result == ContentDialogResult.Primary)
            {
                //playlistname inputted by user in textbox
                var plname = AddPlayListDialog.Content;


                //create a playlist object and call AddPlayList from viewmodel
                vm.AddPlayList(new PlayList
                {
                    PlayListName = plname.ToString(),

                    //i need to add code to the list of songIDs here

                });

            }
            else if (result == ContentDialogResult.Secondary) //cancel was selected
            {
                AddPlayListDialog.Hide();
            }
        }

        private void PlayListNames_ItemClick(object sender, ItemClickEventArgs e)
        {
            //writecode to display songs of selected playlist
        }
    }
}

