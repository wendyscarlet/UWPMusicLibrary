using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MusicLibraryApp.Model
{
    class SongsDAO
    {
        const string TEXT_FILE_NAME = "SongStorage.txt";

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
            var songFile = await folder.GetFileAsync(TEXT_FILE_NAME);
            var lines = await Windows.Storage.FileIO.ReadLinesAsync(songFile);

            foreach (var line in lines)
            {
                var songData = line.Split(',');
                songsList.Add(new Song
                {
                    Title = songData[0],
                    Artist = songData[1],
                    Album = songData[2]
                });
            }
        }


        /// <summary>
        /// Appends a song to the end of a txt file in local storage storing all
        /// songs in the collection
        /// </summary>
        /// <param name="song">the song you want to save</param>
        public static async void addSong(Model.Song song)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile songFile = await localFolder.CreateFileAsync(TEXT_FILE_NAME, CreationCollisionOption.OpenIfExists);

            var songData = $"{song.Title},{song.Artist},{song.Album},{song.AudioFilePath}";
            await FileIO.AppendTextAsync(songFile, songData);
            await FileIO.AppendTextAsync(songFile, Environment.NewLine);
        }
    }
}
