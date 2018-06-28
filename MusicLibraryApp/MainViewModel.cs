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

<<<<<<< HEAD
=======
 

>>>>>>> 8e4d5e0ff9de69d19e9e05c1964b7df303c37d7d
         public  ObservableCollection<Song> SongsList { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        public MainViewModel() {
            SongsList = new ObservableCollection<Song>();
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
            
            SongsList.Add(new Song
            {
                Title = "Lemonade",
                Artist = "Beyonce",
                Album = "her first"
            });
            SongsList.Add(new Song
            {
                Title = "Hello",
                Artist = "Adele",
                Album = "25"
            });
            SongsList.Add(new Song
            {
                Title = "Billie Jean",
                Artist = "Michael Jackson",
                Album = "Hits"
            });


<<<<<<< HEAD

=======
>>>>>>> 8e4d5e0ff9de69d19e9e05c1964b7df303c37d7d
        }

        public void AddDummySong() {
            SongsList.Add(new Song
            {
                Title = "Thunder",
                Artist = "Imagine Dragons",
                Album = "Evolve"
            });
        }
<<<<<<< HEAD

=======
>>>>>>> 8e4d5e0ff9de69d19e9e05c1964b7df303c37d7d
    }
}
