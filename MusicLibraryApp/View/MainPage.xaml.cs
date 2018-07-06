using MusicLibraryApp.AppDialogs;
using MusicLibraryApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private VoiceCommandObjects.CortanaCommands _pageParameters;

        public int Playlist { get; private set; }

        public MainPage()
        {
            this.InitializeComponent();
            vm = new MainViewModel();
            vm.GetAllSongs();
            this.DataContext = vm;
            playing = false;
           

        }

        #region SplitView
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
                //MyMediaElement.PosterSource = songInContext.CoverImage;
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
                MediaElementImage.Source = songInContext.CoverImage;

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
        #endregion

        #region PlayControl
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
        #endregion

        #region PlayList
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

                        if (!plname.ToString().Contains(","))
                        {


                            //create a playlist object and call AddPlayList from viewmodel
                            vm.AddPlayList(new PlayList
                            {
                                PlayListName = plname.ToString(),



                            });
                        }
                        else
                        {
                            throw new ArgumentException("Commas cannot be used");
                        }
                    }
                    else if (result == ContentDialogResult.Secondary) //cancel was selected
                    {
                        AddPlayListDialog.Hide();
                    }
                }

                private void PlayListNames_ItemClick(object sender, ItemClickEventArgs e)
                {
                    //writecode to display  and play songs of selected playlist
                }

                private void PlayListNames_ItemRightTapped(object sender, RightTappedRoutedEventArgs e)
                {
                    //bring out a menu context to add song to playlist , delete playlist name and edit playlist name
                    MenuFlyout myFlyout = new MenuFlyout();

                    if (PlayListNames.SelectedItems.Count > 0)
                    {
                        PlayList p = (PlayList)PlayListNames.SelectedItem;

                        MenuFlyoutItem addSongsPlaylist = new MenuFlyoutItem { Text = "Add Songs to " + $"{p.PlayListName}" };
                        MenuFlyoutItem deletePlaylist = new MenuFlyoutItem { Text = "Delete " + $"{p.PlayListName}" };

                        myFlyout.Items.Add(addSongsPlaylist);
                        myFlyout.Items.Add(deletePlaylist);

                        addSongsPlaylist.Click += AddSongsToPlayList_Click;
                        deletePlaylist.Click += DeletePlayList_Click;
                        // renamePlaylist.Click += RenamePlaylist_Click;

                        FrameworkElement senderElement = sender as FrameworkElement;

                        //the code can show the flyout in your mouse click 
                        myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
                    }
                }

                private void AddSongsToPlayList_Click(object sender, RoutedEventArgs e)
                {
                    if (PlayListNames.SelectedItems.Count > 0)
                    {
                        PlayList p = (PlayList)PlayListNames.SelectedItem;
                        vm.GetAllSongs();
                        this.DataContext = vm;

                        if (SongGridView.SelectedItems.Count > 0)
                        {
                            for (int i = 0; i < SongGridView.SelectedItems.Count; i++)
                            {
                                Song s = (Song)SongGridView.SelectedItems[i];
                                p.PlayListSongIDs.Add(s.ID);
                            }
                            vm.AddSongtoPlayList(p);
                        }
                    }
                }

                private void DeletePlayList_Click(object sender, RoutedEventArgs e)
                {

                    vm.DisplayAllPlaylists();
                    PlayList dp = (PlayList)PlayListNames.SelectedItem;

                    foreach (var p in vm.playLists)
                    {
                        if (p.PlayListName == dp.PlayListName)
                            vm.DeletePlayList(dp);
                    }

                }

                private void SongGridViewItem_RightTapped(object sender, RightTappedRoutedEventArgs e)
                {
                    MenuFlyout myFlyout = new MenuFlyout();

                    Song s = (Song)this.SongGridView.SelectedItem;

                    PlayList p = (PlayList)this.PlayListNames.SelectedItem;

                    MenuFlyoutItem addSongsPlaylist = new MenuFlyoutItem { Text = "Add this Songs to " }; //+ $"{p.PlayListName}" };

                    myFlyout.Items.Add(addSongsPlaylist);

                    addSongsPlaylist.Click += AddSongsToPlayList_Click;

                    FrameworkElement senderElement = sender as FrameworkElement;

                    //the code can show the flyout in your mouse click 
                    myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
                }





                /* private void RenamePlaylist_Click(object sender, RoutedEventArgs e)
                 {

                 }
                 */
                #endregion

        #region Cortana Commands
        //Cortana Commands
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.Write("OnNavigated to");
            _pageParameters = e.Parameter as VoiceCommandObjects.CortanaCommands;
            if (_pageParameters != null)
            {
                switch (_pageParameters.VoiceCommandName)
                {
                    case "AddSong":
                        var dialog = new ContentDialog1();
                        await dialog.ShowAsync();
                        break;
                    case "OpenPlayList":
                        MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
                        Search.Visibility = MySplitView.IsPaneOpen ? Visibility.Collapsed : Visibility.Visible;
                        PlayListNames.Visibility = MySplitView.IsPaneOpen ? Visibility.Visible : Visibility.Collapsed;
                        vm.DisplayAllPlaylists();
                        this.DataContext = vm;
                        this.PlayListNames.ItemsSource = vm.playLists;

                        if (vm.playLists.Count > 0)
                        {
                            MySplitView.IsPaneOpen = true;
                            PlayListNames.Visibility = Visibility.Visible;
                        }
                        break;
                    default:
                        Debug.Write("Couldn't find command Name");
                        break;
                }
            }

        }
        #endregion

        #region Pivot
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
        #endregion
    }
}

