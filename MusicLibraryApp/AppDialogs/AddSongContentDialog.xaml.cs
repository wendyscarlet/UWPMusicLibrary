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
        IReadOnlyList<StorageFile> pickedFileList;
        private List<StorageFile> songFile;
        public ContentDialog1()
        {
            this.InitializeComponent();
            
        }
        private void ContentDialog_AddButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            foreach (StorageFile songFile in pickedFileList)
            {

                var song = new Model.Song
                {
                                    
                sourceSongFile = songFile,

                };

            MainViewModel.addSong(song);
        }

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
            picker.FileTypeFilter.Add(".wma");
            picker.FileTypeFilter.Add(".wav");
            pickedFileList = await picker.PickMultipleFilesAsync();
            if (pickedFileList != null && pickedFileList.Count>0)
            {
                foreach (StorageFile file in pickedFileList)
                    Song.Text+=file.Name;
                Song.TextWrapping = TextWrapping.Wrap;

                    IsPrimaryButtonEnabled = true;
               
            }
        }
        
    }
}
