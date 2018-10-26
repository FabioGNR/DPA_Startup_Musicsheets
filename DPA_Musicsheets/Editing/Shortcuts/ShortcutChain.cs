using DPA_Musicsheets.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Editing.Shortcuts
{
    public class ShortcutChain
    {
        private IShortcutChainHandler firstHandler = null;
        private ISet<Key> pressedKeys = new HashSet<Key>();


        public ShortcutChain(MusicLoader musicLoader, Editor editor, KeyDispatcher keyDispatcher)
        {
            // subscribe for keys
            keyDispatcher.KeyDown += KeyDispatcher_KeyDown;
            keyDispatcher.KeyUp += KeyDispatcher_KeyUp;
            // build hardcoded chain
            firstHandler = new SaveAsShortcutHandler(editor);
            firstHandler.SetNext(new OpenFileShortcutHandler(musicLoader));
        }

        private void KeyDispatcher_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            pressedKeys.Remove(e.Key);
        }

        private void KeyDispatcher_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            pressedKeys.Add(e.Key);
            if (!e.Handled)
            {
                firstHandler?.Handle(pressedKeys);
            }
        }
    }
}
