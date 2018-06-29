using MusicLibraryApp.AppDialogs;
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
        SongsDAO songDao;

        public MainPage()
        {
            this.InitializeComponent();       
            songDao = new SongsDAO();
            songDao.GetAllSongs();
            this.DataContext = songDao;

        }

        private void SoundGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //play song item
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
            songDao.GetAllSongs();
            this.DataContext = songDao;
        }

        private async void AddSongButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog1();
            await dialog.ShowAsync();
        }

        private void SongGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
