using MusicLibraryApp;
using MusicLibraryApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryApp.Model
{
    class MainViewModel
    {
        /// <summary>
        /// Contain the Songs to be shown in the UI
        /// </summary>
         public  ObservableCollection<Song> SongsList { get; private set; }
        /// <summary>
        /// Contains all the Songs
        /// </summary>
         private List<Song> AllSongs {  get;  set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel() {
            SongsList = new ObservableCollection<Song>();
            AllSongs = new List<Song>();
        }

        /// <summary>
        /// Add a new Song to the Collection.
        /// </summary>
        /// <param name="s"></param>
         public void AddSong(Song s) {
            SongsList.Add(s);
         }

        /// <summary>
        /// This is a Method just for testing. To use before the AddSong functionality is done.
        /// </summary>
        public void CreateDummySongs() {   
            
            AllSongs.Add(new Song
            {
                Title = "Lemonade",
                Artist = "Beyonce",
                Album = "her first"
            });
            AllSongs.Add(new Song
            {
                Title = "Hello",
                Artist = "Adele",
                Album = "25"
            });
            AllSongs.Add(new Song
            {
                Title = "Billie Jean",
                Artist = "Michael Jackson",
                Album = "Hits"
            });

            SongsList = new ObservableCollection<Song>(AllSongs);
        }

        public void AddDummySong() {
            var newSong = new Song
            {
                Title = "Thunder",
                Artist = "Imagine Dragons",
                Album = "Evolve"
            };
            AllSongs.Add(newSong);
            SongsList.Add(newSong);
            
        }
        /// <summary>
        /// Search a Song from the File.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        public async void SearchSongsAsync(string str,int pageSize=1,int currentPage =0) {
           var allSongs = await  FileHelper.GetSongsAsync();
            var query = (from Song s in allSongs
                         where s.Title.Contains(str)|| s.Album.Contains(str) || s.Artist.Contains(str)
                         select s).Skip(pageSize * currentPage).Take(pageSize);
            SongsList = (ObservableCollection < Song >) query;

        }
        /// <summary>
        /// Search a Song in the Memory.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        public  void SearchSongs(string str, int pageSize = 1, int currentPage = 0)
        {
            var query = (from Song s in AllSongs
                         where s.Title.Contains(str) || s.Album.Contains(str) || s.Artist.Contains(str)
                         select s).Skip(pageSize * currentPage).Take(pageSize);
            SongsList = new ObservableCollection<Song>(query);
           
        }

        public void AllSongsToDisplay() {
            SongsList = new ObservableCollection<Song>(AllSongs);
        }
    }
}
