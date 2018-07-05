using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryApp.VoiceCommandObjects
{
    class CortanaCommands
    {
        /// <summary>
        /// The voice command (Name attribute of Command element in VoiceCommands.xml).
        /// </summary>
        public string VoiceCommandName { get; set; }

        /// <summary>
        /// The command mode (voice or text activation).
        /// </summary>
        public string CommandMode { get; set; }

        /// <summary>
        /// The raw voice command text.
        /// </summary>
        public string TextSpoken { get; set; }

    }
}
