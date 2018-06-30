using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MusicLibraryApp
{
    public static class FileHelper
    {
        public static string FILE_NAME = "SongStorage.txt";

        /// <summary>
        /// Gets all songs from a text file in local storage called SongStorage.txt
        /// </summary>
        /// <returns>collection of Songs</returns>
        public static async Task<ICollection<Model.Song>> GetSongsAsync()
        {
            List<Model.Song> songs = new List<Model.Song>();

            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile songFile = await folder.GetFileAsync(FILE_NAME);
            var lines = await FileIO.ReadLinesAsync(songFile);

            foreach (var line in lines)
            {
                var songData = line.Split(',');
                var song = new Model.Song();

                song.Title = songData[0];
                song.Artist = songData[1];
                song.Album = songData[2];
                song.CoverImagePath = songData[3];
                song.AudioFilePath = songData[4];
                song.Genre = (Model.Genre)Enum.Parse(typeof(Model.Genre), songData[5]);

                songs.Add(song);
            }

            return songs;
        }

        public static String convertToString(this Enum eff)
        {
            return Enum.GetName(eff.GetType(), eff);
        }
        /// <summary>
        /// Appends a song to the end of a txt file in local storage storing all
        /// songs in the collection
        /// </summary>
        /// <param name="song">the song you want to save</param>
        public static async void WriteSongToFileAsync(Model.Song song)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile songFile = await localFolder.CreateFileAsync(FILE_NAME, CreationCollisionOption.OpenIfExists);

            var songData = $"{song.Title},{song.Artist},{song.Album},{song.CoverImagePath}{song.AudioFilePath},{song.Genre}" + Environment.NewLine;
            await FileIO.AppendTextAsync(songFile, songData);
        }
    }
}
