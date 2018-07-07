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
    public sealed partial class AddPlaylist : ContentDialog
    {
       
        public AddPlaylist()
        {
            this.InitializeComponent();
            
        }
        private void ContentDialog_AddButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            /*  var song = new Model.Song
              {
                  sourceSongFile = songFile,

              };

              MainViewModel.addSong(song);
              */
            Content = this.PlayListName.Text;
            
            
        }

        private void ContentDialog_CancelButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void PlayListName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = PlayListName.Text;
            if (input != null && input.Count() > 2)
                IsPrimaryButtonEnabled = true;
        }
    }
}
