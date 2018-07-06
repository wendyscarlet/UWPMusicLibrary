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
        public int ID { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public BitmapImage CoverImage { get; set; }
        public string SongFileName { get; set; }
        public Windows.Storage.StorageFile sourceSongFile { get; set; }
        public string ShortArtistName { get {
                if (Artist.Length > 31)
                    return Artist.Substring(0, 30);
                else return Artist;
            } }
    }
}
