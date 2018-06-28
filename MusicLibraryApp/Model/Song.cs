using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicLibraryApp.Model
{
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
       public BitmapImage CoverImage { get; set; }     
        public string AudioFilePath { get; set; }

        //public static ICollection<Song> GetSongList()
        //{

        //    List<Song> SongsList = new List<Song>();

        //    var s1 = new Song
        //    {
        //        Title = "Lemonade",
        //        Artist = "Beyonce",
        //        Album = "her first"
        //    };
        //    var s2 = new Song
        //    {
        //        Title = "Hello",
        //        Artist = "Adele",
        //        Album = "her second"
        //    };
        //    var s3 = new Song
        //    {
        //        Title = "Billie Jean",
        //        Artist = "Michael Jackson",
        //        Album = "Hits"
        //    };
        //    SongsList.Add(s1);
        //    SongsList.Add(s2);
        //    SongsList.Add(s3);

        //    return SongsList;
        //}
    }
}
