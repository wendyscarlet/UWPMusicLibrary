using MusicLibraryApp;
using MusicLibraryApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicLibraryApp.Model
{
    class MainViewModel
    {
        private ObservableCollection<Song> SongsList;
        private ObservableCollection<StorageFile> AllSongStorageFiles;
        public MainViewModel()
        {
            SongsList = new ObservableCollection<Song>();
            AllSongStorageFiles = new ObservableCollection<StorageFile>();
        }
        public ObservableCollection<Song> Songs
        {
            get
            {
                return SongsList;
            }
        }
        public async Task<ObservableCollection<StorageFile>> GetAllSongsFromFolderAsync()
        {

            StorageFolder SongsFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation
                .GetFolderAsync(@"Assets");
            //StorageFolder assets = await appInstalledFolder.GetFolderAsync(@"/Assets/SongFiles");
            // var testFiles = await assets.GetFilesAsync();
            //test=testFiles.Count.ToString();
                      
            foreach (var songFile in await SongsFolder.GetFilesAsync())
            {
                AllSongStorageFiles.Add(songFile);
            }
            // AllSongStorageFiles.Add(storageFile);
            return AllSongStorageFiles;
        }

        public async void GetAllSongsCollection()
        {
            var allFiles = await GetAllSongsFromFolderAsync();
            int id = 1;
            foreach (var songFile in AllSongStorageFiles)
            {
                if (songFile.FileType.Equals(".mp3"))
                {
                    MusicProperties songProperties = await songFile.Properties.GetMusicPropertiesAsync();
                    //get album cover
                    StorageItemThumbnail storageItemThumbnail = await songFile.GetThumbnailAsync(ThumbnailMode.MusicView, 200,
                        ThumbnailOptions.UseCurrentScale);

                    var AlbumCover = new BitmapImage();
                    AlbumCover.SetSource(storageItemThumbnail);

                    var song = new Song();
                    song.id = id;
                    song.Title = songProperties.Title;
                    song.Artist = songProperties.Artist;
                    song.Album = songProperties.Album;
                    song.AlbumCover = AlbumCover;
                    song.AudioFilePath = songFile;
                    //song.SongFile = songFile;

                    SongsList.Add(song);
                    id++;
                }

            }
        }

        public void CreateDummySongs()
        {
            GetAllSongsCollection();
        }

        /*********************
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


        }
     
        public void AddDummySong() {
            SongsList.Add(new Song
            {
                Title = "Thunder",
                Artist = "Imagine Dragons",
                Album = "Evolve"
            });
        }
    
    ************************/
    }
}
