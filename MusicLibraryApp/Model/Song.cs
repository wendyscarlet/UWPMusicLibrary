using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MusicLibraryApp.Model
{
    public enum Genre
    {
        Pop,
        Rock,
        Classical,
        RnB,
        Jazz,
        HipHop

    }
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string AlbumTitle { get; set; }
        //public Bitmap CoverImage { get; set; }  
        public string CoverImagePath { get; set; }
        public string AudioFilePath { get; set; }
        public Genre Genre { get; set; }

    }
}
