using System;
using System.Runtime.Serialization;
using System.ComponentModel;
using Windows.UI.Input.Inking;
using MusicLibrary;

namespace MusicLibrary
{

    public enum GenreEnumType {Pop =1,Jazz=2,Blues=3, Rock =4, Country =5, Alternative =6, Techno=7}
    
    /// <summary>
    /// Provides the business object for the Song data.
    /// </summary>
    [DataContract]
    public class Song : BindableBase
    {       
        /// <summary>
        /// Initializes a new instance of a <see cref="Song"/> with 
        /// the specified <see cref="Person"/> object.
        /// </summary>
        /// <param name="person">The person the song is for.</param>
        public Song(Person person)
        {
            this.SongIsFor = person;            
        }

        /// <summary>
        /// Gets or sets the <see cref="Person"/> instance that's associated 
        /// with the current song.
        /// </summary>
        [DataMember]
        public Person SongIsFor { get; set; }

        [DataMember]
        public String Title { get; set; }

        [DataMember]
        public String Artist { get; set; }

        [DataMember]
        public String Album { get; set; }

        [DataMember]
        public GenreEnumType Genre { get; set; }



        #region MyNotes


        /// <summary>
        /// Initializes a new instance of a <see cref="Song"/> with 
        /// the specified tag string.
        /// </summary>
        /// <param name="nametag">Who the song is for.</param>
        public Song(string nametag)
        {
            WhoNoteIsFor = nametag;
        }
        /// <summary>
        /// Gets or sets the text in the current note.
        /// </summary>
        [DataMember]
        public string NoteText
        {
            get
            {
                return _noteText;
            }

            set
            {
                SetProperty(ref _noteText, value);
            }
        }
        private string _noteText;


        /// <summary>
        /// Gets or sets the placeholder for the <see cref="Note"/> control
        /// that's associated with the current <see cref="Song"/> object.
        /// </summary>
        public string NotePlaceholderText
        {
            get
            {
                return _notePlaceholderText;
            }

            set
            {
                SetProperty(ref _notePlaceholderText, value);
            }
        }
        private string _notePlaceholderText;

        /// <summary>
        /// Gets or sets the ink stroke data for the <see cref="Note"/> control
        /// that's associated with the current <see cref="Song"/> object.
        /// </summary>
        [IgnoreDataMember] // can't serialize InkStrokeContainers
        public InkStrokeContainer Ink
        {
            get
            {
                return _ink;
            }

            set
            {
                SetProperty(ref _ink, value);
            }
        }

        [IgnoreDataMember]
        private InkStrokeContainer _ink = new InkStrokeContainer();

        /// <summary>
        /// Gets or sets the name of the <see cref="Person"/> that
        /// the current <see cref="Song"/> is for.
        /// </summary>
        [DataMember]
        public string WhoNoteIsFor { get; set; }

        /// <summary>
        /// Gets or sets the input mode that the user created the
        /// note's content with.
        /// </summary>
        [DataMember]
        public NoteInputMode NoteMode
        {
            get
            {
                return _noteMode;
            }
            set
            {
                SetProperty(ref _noteMode, value);
            }
        }
        private NoteInputMode _noteMode;

    }
    #endregion


}