using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MusicLibraryApp.Player
{
    class BackgroundPlayer
    {
        public async static void playSong(String filePath)
        {
            StorageFolder Folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            Folder = await Folder.GetFolderAsync("MyFolder");
            StorageFile sf = await Folder.GetFileAsync("MyFile.mp3");
            var storageFile = await KnownFolders.MusicLibrary.GetFileAsync("line.mp3");
            var stream = await storageFile.OpenAsync(FileAccessMode.Read);
          //  mediaElement.SetSource(stream, storageFile.ContentType);
            //mediaElement.Play();
        }
    }
}
