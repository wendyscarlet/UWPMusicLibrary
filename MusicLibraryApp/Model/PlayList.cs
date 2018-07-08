using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryApp.Model
{
    public class PlayList
    {
        public string PlayListName { get; set; }
        public ObservableCollection<Song> PlayListSongs { get; set; }
        public string PlayListFilePath { get; set; }

        public PlayList()
        {
            this.PlayListSongs = new ObservableCollection<Song>();
        }
        public ObservableCollection<Song> GetSongsListOfPlayList()
        {
            return this.PlayListSongs;
        }
    }


}
