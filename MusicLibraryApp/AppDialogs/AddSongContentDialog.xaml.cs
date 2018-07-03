using MusicLibraryApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MusicLibraryApp.AppDialogs
{
    public sealed partial class ContentDialog1 : ContentDialog
    {
        private Windows.Storage.StorageFile songFile;
        public ContentDialog1()
        {
            this.InitializeComponent();
            
        }
        private void ContentDialog_AddButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var song = new Model.Song
            {
                sourceSongFile = songFile,
                
            };

            MainViewModel.addSong(song);

        }

        private void ContentDialog_CancelButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
        private async void ContentDialog_TakeImage_Clicked(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();

            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add(".mp3");
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                songFile = file;
                Song.Text = file.Path;
            }
        }

        private void Song_TextChanged(object sender, TextChangedEventArgs e)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            if(songFile!=null)
            IsPrimaryButtonEnabled = true;
=======

>>>>>>> parent of 29f76cf... Merge branch 'master' of https://kalacademy.visualstudio.com/SoftwareDevC1Team5/_git/SoftwareDevC1Team5
=======

>>>>>>> f7cd3a60d561eef7f4599b2f1ec2329e5aa49e24
        }
    }
}
