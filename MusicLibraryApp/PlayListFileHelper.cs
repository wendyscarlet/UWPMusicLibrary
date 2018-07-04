using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MusicLibraryApp
{
    class PlayListFileHelper
    {

        public static string FILE_NAME = "AllPlayListFileNames.txt";

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

            //add all song ids in file
            if (songcount > 0)
            {
                for (int i = 0; i < songcount; i++)
                    plData = plData + $"{playlist.PlayListSongIDs[i].ToString()},";

               // plData = plData + $"{playlist.PlayListSongIDs[songcount - 1].ToString()}";
            }
                plData = plData + Environment.NewLine;
            
            //overwite the playlist data if already exist , writes if it is new
            await FileIO.WriteTextAsync(PLFile, plData);

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
                    if (pline.Contains(playlist.PlayListName))
                    {
                        exist = true;
                    }
                }
            }
            if (exist == false)
            {
                var playlistfilename = playlist.PlayListName + Environment.NewLine;
                await FileIO.AppendTextAsync(allPLFile, playlistfilename);
                
            }
        }
    }
}
