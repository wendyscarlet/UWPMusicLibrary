using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryApp.Model
{
    public class PlayList
    {
        public string PlayListName { get; set; }
        public List<int> PlayListSongIDs { get; set; }
        public string PlayListFilePath { get; set; }
    }
}
