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
            vm = new MainViewModel();
            vm.CreateDummySongs();
            this.DataContext = vm;
        }

        private void SoundGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //play song item
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            Search.Visibility = MySplitView.IsPaneOpen ? Visibility.Collapsed : Visibility.Visible;

            if (MySplitView.IsPaneOpen)
            {
                Search.Visibility = Visibility.Collapsed;

            }
            else
                Search.Visibility = Visibility.Visible;

        }

        private void SearchAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //text is added in search box
        }

        private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

            vm.SearchSongs(SearchAutoSuggestBox.Text);
            this.SongGridView.ItemsSource = vm.SongsList;
        }

        private void DisplaySongList_Click(object sender, RoutedEventArgs e)
        {
        }

        private void AddSongButton_Click(object sender, RoutedEventArgs e)
        {
            vm.AddDummySong();
        }

        private void SearchAutoSuggestBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = true;
            Search.Visibility = Visibility.Collapsed;
        }
    }
}
