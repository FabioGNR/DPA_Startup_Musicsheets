using DPA_Musicsheets.Editing.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Editing.Shortcuts
{
    public class SaveAsShortcutHandler : BaseEditorShortcutHandler
    {
        private SaveAsCommand _saveAsCommand = new SaveAsCommand();

        public SaveAsShortcutHandler(IEditor editor) : base(editor)
        {
        }

        protected override bool TryHandle(ISet<Key> pressedKeys)
        {
            bool modifierActive = pressedKeys.Contains(Key.LeftCtrl) || pressedKeys.Contains(Key.RightCtrl);
            if (modifierActive && pressedKeys.Contains(Key.S))
            {
                _editor.ExecuteCommand(_saveAsCommand);
                return true;
            }
            return false;
        }
    }
}
