using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace MusicLibraryApp
{
    class PlayListFileHelper
    {

        public static string FILE_NAME = "AllPlayListFileNames.txt";
        public static string FILE_NAME2 = "SongStorage.txt";
        public static string FILE_NAME3 = "tempStorage.txt";
        /// <summary>
        /// Gets all playlist file names from a text file in local storage called AllPlayListFileNames.txt
        /// Goes to each file path and get playlistname and all its songids 
        /// put it in a playlist
        /// </summary>
        /// <returns>collection of PlayList</returns>
        public static async Task<ICollection<Model.PlayList>> GetAllPlayListsAsync()
        {
            List<Model.PlayList> playlists = new List<Model.PlayList>();
            StorageFolder allPLfolder = ApplicationData.Current.LocalFolder;
            StorageFile allPLFile = await allPLfolder.CreateFileAsync(FILE_NAME, CreationCollisionOption.OpenIfExists);

            var allplines = await FileIO.ReadLinesAsync(allPLFile);

            foreach (var pline in allplines) {
                if (pline != "")
                {
                    StorageFolder PLfolder = ApplicationData.Current.LocalFolder;
                    StorageFile PLFile = await PLfolder.GetFileAsync(pline);

                    var line = await FileIO.ReadTextAsync(PLFile);

                    var plData = line.Split(',');
                    var playlist = new Model.PlayList();

                    //read data from file and put in a playlist
                    playlist.PlayListName = plData[0];

                    for (int i = 1; i < plData.Length; i++)
                    {
                        if (plData[i] != "\r\n")
                            playlist.PlayListSongIDs.Add(Convert.ToInt32(plData[i]));
                    }
                    playlist.PlayListFilePath = pline;

                    playlists.Add(playlist);
                }
            }

            return playlists;
        }

        public static async void DeletePlayListAsync(Model.PlayList p)
        {
            
            StorageFolder allPLfolder = ApplicationData.Current.LocalFolder;
            StorageFile allPLFile = await allPLfolder.CreateFileAsync(FILE_NAME, CreationCollisionOption.OpenIfExists);
            StorageFolder tempfolder = ApplicationData.Current.LocalFolder;
            StorageFile tempFile = await tempfolder.CreateFileAsync(FILE_NAME3, CreationCollisionOption.OpenIfExists);

            var allplines = await FileIO.ReadLinesAsync(allPLFile);
           
            foreach (var pline in allplines)
            {
                if (pline != "")
                {
                    
                    if (!pline.Contains(p.PlayListName))
                    {

                        var tline = pline + Environment.NewLine;
                        await FileIO.AppendTextAsync(tempFile, tline);
                    }

                    
                }
            }

            await tempFile.CopyAndReplaceAsync(allPLFile);
            await tempFile.DeleteAsync();
            
        }

        /// <summary>
        /// Write a playlist with its song is to a txt file in local storage 
        /// write playlist name in text file that has a collection of playlists
        /// checks to see if playlist exist before adding in this collection
        /// </summary>
        /// <param name="song">the song you want to save</param>
        public static async void WritePlayListToFileAsync(Model.PlayList playlist)
        {
            var songcount = 0;

            StorageFolder PLlocalFolder = ApplicationData.Current.LocalFolder;
            StorageFile PLFile = await PLlocalFolder.CreateFileAsync(playlist.PlayListName, CreationCollisionOption.OpenIfExists);

            var plData = $"{playlist.PlayListName},";


            //check if playlist does not have songs

            if (playlist.PlayListSongIDs == null)
                songcount = 0;
            else
                songcount = playlist.PlayListSongIDs.Count;
            //add all song ids in file
            if (songcount > 0)
            {
                for (int i = 0; i < songcount; i++)
                    plData = plData + $"{playlist.PlayListSongIDs[i].ToString()},";

               // plData = plData + $"{playlist.PlayListSongIDs[songcount - 1].ToString()}";
            }
                plData = plData + Environment.NewLine;

           

            var oldplData = await FileIO.ReadTextAsync(PLFile);

            bool rewrite = false;

            if (oldplData != "")
            {
                var oldplDatasongs = oldplData.Split(',');
                
                //not including playlistname and newline
                var oldplDatacount = oldplDatasongs.Length - 2;
                if(songcount > oldplDatacount)
                {
                    rewrite = true;
                }

            }
            //overwite the playlist data if already exist , writes if it is new
            //await FileIO.WriteTextAsync(PLFile, plData);

            //save the playlist name in AllPlayListFileNames.txt"

            StorageFolder allPLfolder = ApplicationData.Current.LocalFolder;
            StorageFile allPLFile = await allPLfolder.CreateFileAsync(FILE_NAME, CreationCollisionOption.OpenIfExists);

            var allplines = await FileIO.ReadLinesAsync(allPLFile);

            //checks if playlist name is already in allplaylist file
            bool exist = false;
            foreach (var pline in allplines)
            {
                if (pline != null)
                {
                    if (pline == playlist.PlayListName)
                    {
                        exist = true;
                    }
                }
            }
            //playlist does not exist and should be added
            if (exist == false)
            {


                //writes create new playlist
                await FileIO.WriteTextAsync(PLFile, plData);

                var playlistfilename = playlist.PlayListName + Environment.NewLine;
                await FileIO.AppendTextAsync(allPLFile, playlistfilename);

            }
            else
            {
                if (rewrite == false)
                {
                    var messageDialog = new MessageDialog("The playlist name already exist.Please chose another one");
                    // Show the message dialog
                    await messageDialog.ShowAsync();
                    return;
                }
                else
                {
                    //overwite the playlist data if already exist
                    await FileIO.WriteTextAsync(PLFile, plData);

                }
            }
        }

        public static async Task<bool> IsDuplicateNewPlaylistAsync(Model.PlayList playlist)
        {
            var isduplicate = false;
            var songcount = 0;

            StorageFolder PLlocalFolder = ApplicationData.Current.LocalFolder;
            StorageFile PLFile = await PLlocalFolder.CreateFileAsync(playlist.PlayListName, CreationCollisionOption.OpenIfExists);

            var plData = $"{playlist.PlayListName},";


            //check if playlist does not have songs

            if (playlist.PlayListSongIDs == null)
                songcount = 0;
            else
                songcount = playlist.PlayListSongIDs.Count;
            
            var oldplData = await FileIO.ReadTextAsync(PLFile);

            bool rewrite = false;

            if (oldplData != "")
            {
                var oldplDatasongs = oldplData.Split(',');

                //not including playlistname and newline
                var oldplDatacount = oldplDatasongs.Length - 2;
                if (songcount > oldplDatacount)
                {
                    rewrite = true;
                }

            }
            //check if playlist exist

            StorageFolder allPLfolder = ApplicationData.Current.LocalFolder;
            StorageFile allPLFile = await allPLfolder.CreateFileAsync(FILE_NAME, CreationCollisionOption.OpenIfExists);

            var allplines = await FileIO.ReadLinesAsync(allPLFile);

            //checks if playlist name is already in allplaylist file
            bool exist = false;
            foreach (var pline in allplines)
            {
                if (pline != null)
                {
                    if (pline == playlist.PlayListName)
                    {
                        exist = true;
                    }
                }
            }
            //playlist does not exist and should be added
            if (exist == true)
            {


                if (rewrite == false)
                {
                    
                    isduplicate = true; ;
                }
                
            }
            return isduplicate;
        }

        /// <summary>
        /// Write a song with its song title , artist and assigned ID into txt file in local storage 
        /// checks to see if song exist before adding in this collection
        /// </summary>
        /// <param name="song">the song you want to save</param>
        public static async void WriteSongToFileAsync(Model.Song song)
        {
          
            StorageFolder sFolder = ApplicationData.Current.LocalFolder;
            StorageFile sFile = await sFolder.CreateFileAsync(FILE_NAME2, CreationCollisionOption.OpenIfExists);

            //checks if song title and artist is already in SongStorage.txt file
            var slines = await FileIO.ReadLinesAsync(sFile);
            bool exist = false;
            foreach (var sline in slines)
            {
                if (sline != "")
                {
                    var check = sline.Split(',');
                    if (check[0] == song.Title)
                    {
                        exist = true;
                    }
                }
            }
            if (exist == false)
            {
                // create song data  and add into SongStorage.txt file
                var sData = $"{song.Title},";
                sData = sData + $"{song.ID.ToString()},";
                sData = sData + Environment.NewLine;
                await FileIO.AppendTextAsync(sFile, sData);

            }
        }

        /// <summary>
        /// Passes in all songs in songlist 
        /// checks file for all song id mapped to song title
        /// Gets all song ids with its song title and puts it in a dictionary 
        /// returns the dictionary
        /// faster performance less unique, for example 
        /// Hello by Adele and Hello by Beatles will cause a problem
        /// unless we sort our songlist before we call  the function
        /// </summary>
        /// 
        public static async Task<Dictionary<string, int>> GetAllSongIDsFromFileAsync(List<Model.Song> allsongs)
        {
            var songIDs = new Dictionary<string, int>();
            StorageFolder sFolder = ApplicationData.Current.LocalFolder;
            StorageFile sFile = await sFolder.CreateFileAsync(FILE_NAME2, CreationCollisionOption.OpenIfExists);

            //checks if song title and artist is already in SongStorage.txt file
            var slines = await FileIO.ReadLinesAsync(sFile);
           
            //searches for songs, if it find it add it into a dictionary

            for (int i = 0; i < allsongs.Count; i++)
            {
                foreach (var sline in slines)
                {
                    if (sline != "") 
                    {
                        var check = sline.Split(',');
                        if (check[0] == allsongs[i].Title)
                        {
                            songIDs.Add(allsongs[i].Title, Convert.ToInt32(check[1]));
                        }
                    }
                }
            }
            //when called in main view model must check if it dictionary is empty, means song does not have id
            return songIDs;
        }

        /// <summary>
        /// Passes in a song
        /// checks file for it song id mapped to song title and artist 
        /// returns the songid
        /// more unique selection poor performance
        /// </summary>
        /// 
        public static async Task<int> GetSongIDFromFileAsync(Model.Song song)
        {
            int songID=-1;

            StorageFolder sFolder = ApplicationData.Current.LocalFolder;
            StorageFile sFile = await sFolder.CreateFileAsync(FILE_NAME2, CreationCollisionOption.OpenIfExists);

            //checks if song title and artist is already in SongStorage.txt file
            var slines = await FileIO.ReadLinesAsync(sFile);

            //searches for songs, if it find it add it into a dictionary

           
                foreach (var sline in slines)
                {
                    if (sline != "")
                    {
                        var check = sline.Split(',');
                        if (check[0] == song.Title)
                        {
                            songID= Convert.ToInt32(check[1]);
                        }
                    }
                }
            //when called in main view model must check if it is -1, means song does not have id
            return songID;
        }

    }
}
