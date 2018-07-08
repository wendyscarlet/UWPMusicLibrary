using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicLibraryApp.Model
{
    class MainViewModel
    {
        /// <summary>
        /// Contain the Songs to be shown in the UI
        /// </summary>
        public ObservableCollection<Song> songsList { get; private set; }
        public ObservableCollection<PlayList> playLists { get; private set; }

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
                if (file.FileType.Equals(".mp3"))
                {
                    MusicProperties musicProperties = await file.Properties.GetMusicPropertiesAsync();



                    StorageItemThumbnail storageItemThumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView,
                         200, ThumbnailOptions.UseCurrentScale);
                    var AlbumCover = new BitmapImage();
                    AlbumCover.SetSource(storageItemThumbnail);

                    Song s = new Song
                    {

                        Title = ((musicProperties.Title == null || musicProperties.Title == "") ? "Unknown" : musicProperties.Title),
                        Artist = ((musicProperties.Artist == null || musicProperties.Artist == "") ? "Unknown" : musicProperties.Artist),
                        Album = ((musicProperties.Album == null || musicProperties.Album == "") ? "Unknown" : musicProperties.Album),
                        SongFileName = file.Name,
                        CoverImage = AlbumCover

                    };
                    //get songid from file
                    var songid = await PlayListFileHelper.GetSongIDFromFileAsync(s);
                    //add it to song
                    s.ID = songid;
                    songsList.Add(s);
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
            MusicProperties musicProperties = await songFile.Properties.GetMusicPropertiesAsync();
            try
            {

                if (musicProperties.Title == null || musicProperties.Title == "")
                {

                    throw new ArgumentException("Cannot add music file without Title");

                }


                try
                {
                    Windows.Storage.StorageFile existingFile = await localFolder.GetFileAsync(songFile.Name);


                }
                catch (FileNotFoundException ex)
                {
                    await songFile.CopyAsync(localFolder);
                }

                //add song id and title and artist
                song.Title = musicProperties.Title;
                if (musicProperties.Artist == null || musicProperties.Artist == "")
                    song.Artist = "Unknown";
                else
                    song.Artist = musicProperties.Artist;

                if (musicProperties.Album == null || musicProperties.Album == "")
                    song.Album = "Unknown";
                else
                    song.Album = musicProperties.Album;

                song.ID = ++lastSongID;
                PlayListFileHelper.WriteSongToFileAsync(song);

            }
            catch (ArgumentException ex)
            {
                var messageDialog = new MessageDialog(ex.Message);
                // Show the message dialog
                await messageDialog.ShowAsync();
                return;
            }


        }

        public void SearchSongs(string str, int pageSize = 1, int currentPage = 0)
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

        //add playlist to observable collection of playlist and creates a playlist file
        public async void AddPlayList(PlayList p)
        {
            if (await PlayListFileHelper.IsDuplicateNewPlaylistAsync(p) == false)
            {
                playLists.Add(p);
                //also call Write to file function
                PlayListFileHelper.WritePlayListToFileAsync(p);
            }
            else
            {
                var messageDialog = new MessageDialog("The playlist name already exist.Please chose another one");
                // Show the message dialog
                await messageDialog.ShowAsync();
                return;
            }
        }

        public void DeletePlayList(PlayList p)
        {

            // delete playlist from file
            PlayListFileHelper.DeletePlayListAsync(p);

        }

        public async void DisplayAllPlaylists()
        {

            ObservableCollection<PlayList> tempplayLists = new ObservableCollection<PlayList>(await PlayListFileHelper.GetAllPlayListsAsync());

            playLists.Clear();

            for (int i = 0; i < tempplayLists.Count; i++)
            {
                playLists.Add(tempplayLists[i]);
            }

        }

        public void AddSongtoPlayList(PlayList p, Song song)
        {
            p.PlayListSongs.Add(song);
        }




        #region Pivot

        //private IOrderedEnumerable<IGrouping<string, Song>> albums;
        //private IOrderedEnumerable<IGrouping<string, Song>> artists;

        public IEnumerable<IGrouping<string, Song>> Albums
        {
            get
            {
                var queryGroupByAlbum =
                    from song in songsList
                    group song by song.Album into albums
                    orderby albums.Key
                    select albums;

                return queryGroupByAlbum;

            }
        }

        public IEnumerable<IGrouping<string, Song>> Artists
        {
            get
            {
                var queryGroupByArtist =
                    from song in songsList
                    group song by song.Artist into artists
                    orderby artists.Key
                    select artists;

                return queryGroupByArtist;

            }
        }
        #endregion
        public int Itemscount()
        {
            int count = playLists.ElementAt(0).PlayListSongs.Count;
            return count;
        }

    }
}

