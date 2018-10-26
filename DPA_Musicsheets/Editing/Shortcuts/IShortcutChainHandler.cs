using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Editing.Shortcuts
{
    public interface IShortcutChainHandler
    {
        bool Handle(ISet<Key> pressedKeys);

        void SetNext(IShortcutChainHandler nextHandler);
    }
}
