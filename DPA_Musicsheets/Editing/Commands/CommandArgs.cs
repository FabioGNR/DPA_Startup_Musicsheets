using DPA_Musicsheets.Editing.Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editing.Commands
{
    public class CommandArgs
    {
        /// <summary>
        /// The editor context to store mementos and rerender
        /// </summary>
        public Editor Editor { get; private set; }
        /// <summary>
        /// The entire current lilypond text
        /// </summary>
        public string LilypondText { get; set; }
        /// <summary>
        /// Index at which the user selected text starts
        /// </summary>
        public int SelectionStart { get; set; }
        /// <summary>
        /// Index at which the user selected text stops
        /// </summary>
        public int SelectionEnd { get; set; }

        public CommandArgs(Editor editor)
        {
            this.Editor = editor;
        }
    }
}
