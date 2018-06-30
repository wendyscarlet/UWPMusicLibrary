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
        public ContentDialog1()
        {
            this.InitializeComponent();
            
        }
        private void ContentDialog_AddButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var song = new Model.Song
            {
                Title = Title.Text,
                Artist = Artist.Text,
                Album = "Pop",
                SongFileName = Song.Text
            };
            SongsDAO.addSong(song);

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
                MusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();
                // Application now has read/write access to the picked file
                this.Song.Text = file.Name;
                this.Artist.Text = musicProperties.Artist;
                this.Title.Text = musicProperties.Title;
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                try
                {
                    Windows.Storage.StorageFile existingFile = await localFolder.GetFileAsync(file.Name);
                } catch(FileNotFoundException ex) { 
                    await file.CopyAsync(localFolder);
                }
                   
            }
        }
    }
}
