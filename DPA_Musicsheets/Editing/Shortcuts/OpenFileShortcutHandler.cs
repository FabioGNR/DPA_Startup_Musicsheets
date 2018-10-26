using DPA_Musicsheets.Managers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Editing.Shortcuts
{
    class OpenFileShortcutHandler : BaseShortcutChainHandler
    {
        private MusicLoader _musicLoader;

        public OpenFileShortcutHandler(MusicLoader musicLoader)
        {
            _musicLoader = musicLoader;
        }

        protected override bool TryHandle(ISet<Key> pressedKeys)
        {
            bool hasModifier = pressedKeys.Contains(Key.LeftCtrl) || pressedKeys.Contains(Key.RightCtrl);
            if (hasModifier && pressedKeys.Contains(Key.O))
            {
                OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly" };
                if (openFileDialog.ShowDialog() == true)
                {
                    _musicLoader.OpenFile(openFileDialog.FileName);
                }
                return true;
            }
            return false;
        }
    }
}
