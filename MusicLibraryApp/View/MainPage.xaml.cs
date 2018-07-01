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
        MainViewModel vm;
       // MediaPlayer MyMediaPlayer;
        bool playing;

        public MainPage()
        {
            this.InitializeComponent();
            
            vm = new MainViewModel();
            vm.CreateDummySongs();
            this.DataContext = vm.Songs; ;

            //MediaPlayer
            //MyMediaPlayer = new MediaPlayer();
            playing = false;

        }

        private async void SoundGridView_ItemClick(object sender, ItemClickEventArgs e)
        {

            //getting song from e
            Song Clickedsong =(Song) e.ClickedItem;
            StorageFile ClickedSongFile = Clickedsong.AudioFilePath;
            if (ClickedSongFile != null)
            {
                IRandomAccessStream stream = await ClickedSongFile.OpenAsync(FileAccessMode.Read);
                MyMediaElement.SetSource(stream, ClickedSongFile.ContentType);
            }

            //play song item
            //MyMediaElement.AutoPlay = false;
            //MyMediaElement.Source = MediaSource.CreateFromStorageFile(ClickedSongFile);

            if (playing)
            {
                playing = false;
                //MyMediaElement.Source = null;
                MyMediaElement.AutoPlay = false;
                PlayButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
                PauseButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                
                playing = true;
                MyMediaElement.AutoPlay = true;
                //Visibility visibility = Windows.UI.Xaml.Visibility.Collapsed;
                PlayButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                PauseButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            
        }

        private void SearchAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //text is added in search box
        }

        private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //find song 
        }

        private void DisplaySongList_Click(object sender, RoutedEventArgs e)
        {
            //list all songs
        }

        private void AddSongButton_Click(object sender, RoutedEventArgs e)
        {
            //vm.AddDummySong();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if(MyMediaElement.DefaultPlaybackRate != 1)
            {
                MyMediaElement.DefaultPlaybackRate = 1.0;
            }
            MyMediaElement.Play();
            PlayButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            PauseButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            MyMediaElement.Pause();
            PlayButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            PauseButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            if (slider != null)
            {
                MyMediaElement.Volume = slider.Value;
            }
        }

        private void BackwardButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
