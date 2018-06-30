using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace MusicLibraryApp.Model
{
    class SongsDAO
    {
        public ObservableCollection<Song> songsList { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public SongsDAO()
        {
            songsList = new ObservableCollection<Song>();
        }

         /// <summary>
        /// Gets all songs from a text file in local storage called SongStorage.txt
        /// </summary>
        /// <returns>collection of Songs</returns>
        public async void GetAllSongs()
        {
            songsList.Clear();
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var allFiles = await folder.GetFilesAsync();
            foreach (var file in allFiles)
            {
                if (file.FileType.Equals(".mp3")) { 
                MusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();
                songsList.Add(new Song
                {
                    Title = musicProperties.Title,
                    Artist = musicProperties.Artist,
                    Album = musicProperties.Album,
                    SongFileName = file.Name

                });
            }
            }
        }


        /// <summary>
        /// Appends a song to the end of a txt file in local storage storing all
        /// songs in the collection
        /// </summary>
        /// <param name="song">the song you want to save</param>
        public static async void addSong(Model.Song song)
        {
            var songFile = song.sourceSongFile;
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                Windows.Storage.StorageFile existingFile = await localFolder.GetFileAsync(songFile.Name);
            }
            catch (FileNotFoundException ex)
            {
                await songFile.CopyAsync(localFolder);
            }

        }
    }
}
