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
    class MainViewModel
    {
        public ObservableCollection<Song> songsList { get; private set; }
        public  ObservableCollection<PlayList> playLists { get; private set; }

        private static int lastSongID = 0;


        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel()
        {
            songsList = new ObservableCollection<Song>();
            playLists = new ObservableCollection<PlayList>();
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
                    ID = ++lastSongID,
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
        public  void SearchSongs(string str, int pageSize = 1, int currentPage = 0)
        {
            str = str.ToLower();
            // GetAllSongs();
            var query = (from Song s in songsList
                         where s.Title.ToLower().Contains(str)
                         || s.Album.ToLower().Contains(str)
                         || s.Artist.ToLower().Contains(str)
                         select s);
                         //.Skip(pageSize * currentPage).Take(pageSize);
            songsList = new ObservableCollection<Song>(query);

        }
        //add playlist to observalbe collection of playlist and creates a playlist file
        public  void AddPlayList(PlayList p)
        {
            playLists.Add(p);
            //also call Write to file function
            PlayListFileHelper.WritePlayListToFileAsync(p);
        }
        public async void DisplayAllPlaylists()
        {
            
           /* playLists.Add(new PlayList
            {
                PlayListName = "Favorites",
               
            });
            playLists.Add(new PlayList
            {
                PlayListName = "Happy Songs",

            });
            playLists.Add(new PlayList
            {
                PlayListName = "Sad Songs",

            });
            */
            playLists = new ObservableCollection<PlayList>( await PlayListFileHelper.GetAllPlayListsAsync());

        }

    }
}

