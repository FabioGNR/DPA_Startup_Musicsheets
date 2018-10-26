using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Editing.Shortcuts
{
    public abstract class BaseEditorShortcutHandler : BaseShortcutChainHandler
    {
        protected IEditor _editor;
        public BaseEditorShortcutHandler(IEditor editor)
        {
            _editor = editor;
        }
    }
}
