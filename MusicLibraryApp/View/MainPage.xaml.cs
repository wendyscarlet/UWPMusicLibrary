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
        private MainViewModel vm;
        private MediaSource _mediaSource;
        bool playing;

        public int Playlist { get; private set; }

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
                _mediaSource = MediaSource.CreateFromStorageFile(file);
                this.mediaPlayer.SetPlaybackSource(_mediaSource);
                //this.mediaPlayer.AutoPlay = true;
            }
            if(playing)
            {
                mediaPlayer.AutoPlay = false;
                playing = false;
            }
            else
            {
                playing = true;
                mediaPlayer.AutoPlay = true;

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
            PlayListNames.Visibility = Visibility.Collapsed;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.DefaultPlaybackRate != 1)
            {
                mediaPlayer.DefaultPlaybackRate = 1.0;
            }
            mediaPlayer.Play();
            
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
            
        }

        private void PlayListsButton_Click(object sender, RoutedEventArgs e)
        {
            vm.DisplayAllPlaylists();
            this.DataContext = vm;
            this.PlayListNames.ItemsSource = vm.playLists;
            //display if there is a playlist in the playlist collection
            if (vm.playLists.Count > 0)
            {
                MySplitView.IsPaneOpen = true;
                PlayListNames.Visibility = Visibility.Visible;
            }
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

        private void PlayListNames_ItemRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            //bring out a menu context to add song to playlist , delete playlist name and edit playlist name
            MenuFlyout myFlyout = new MenuFlyout();
            
            MenuFlyoutItem addSongsPlaylist = new MenuFlyoutItem { Text = "Add Songs to playlist" };
            MenuFlyoutItem deletePlaylist = new MenuFlyoutItem { Text = "Delete playlist" };
            MenuFlyoutItem renamePlaylist = new MenuFlyoutItem { Text = "Rename playlist" };
            myFlyout.Items.Add(addSongsPlaylist);
            myFlyout.Items.Add(deletePlaylist);
            myFlyout.Items.Add(renamePlaylist);
            //if you only want to show in left or buttom 
            //myFlyout.Placement = FlyoutPlacementMode.Left;

            FrameworkElement senderElement = sender as FrameworkElement;

            //the code can show the flyout in your mouse click 
            myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }

        private void rootPivot_PivotItemLoading(Pivot sender, PivotItemEventArgs args)
        {

        }

        private void rootPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (rootPivot.SelectedIndex == 1)
                artistsCVS.Source = vm.Artists;
            if (rootPivot.SelectedIndex == 2)
                 albumsCVS.Source = vm.Albums;
        }
    }
}

