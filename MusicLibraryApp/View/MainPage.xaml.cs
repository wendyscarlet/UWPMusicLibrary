using MusicLibraryApp.Model;
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


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MusicLibraryApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainViewModel vm;

        public MainPage()
        {
            this.InitializeComponent();
            // Update greeting that appears at the top of the screen e.g. "Good morning"
            UpdateGreeting();
            vm = new MainViewModel();
            vm.CreateDummySongs();
            this.DataContext = vm;
        }

        private void SoundGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
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
            if (MySplitView.IsPaneOpen == true)
            {
                SearchAutoSuggestBox.Width = 200;
                SearchAutoSuggestBox.Margin = new Thickness(10, 0, 0, 0);
            }
        }


        private void DisplaySongList_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;
            SearchAutoSuggestBox.Width = 200;
            Search.Visibility = Visibility.Collapsed;
            SearchAutoSuggestBox.Margin = new Thickness(10, 0, 0, 0);
            this.DataContext = vm.SongsList;
        }

        private void AddSongButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;
            SearchAutoSuggestBox.Width = 200;
            SearchAutoSuggestBox.Margin = new Thickness(10, 0, 0, 0);
            Search.Visibility = Visibility.Collapsed;
            //call add Songs
            vm.AddDummySong();
        }

        private void SearchSongButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;
            SearchAutoSuggestBox.Width = 200;
            Search.Visibility = Visibility.Collapsed;
            SearchAutoSuggestBox.Margin = new Thickness(10, 0, 0, 0);

        }

        private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            //call search song 
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
