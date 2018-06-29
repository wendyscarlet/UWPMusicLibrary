using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicLibraryApp.Model
{
    public class Song
    {
        public int id { get; set; }
        public String Title { get; set; }
        public String Artist { get; set; }
        public String Album { get; set; }
        //public StorageFile SongFile { get; set; }
        public StorageFile AudioFilePath { get; set; }

        public BitmapImage AlbumCover;


        /********************
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
      // public BitmapImage CoverImage { get; set; }     
        public string AudioFilePath { get; set; }
        *******************/
        
    }
}
