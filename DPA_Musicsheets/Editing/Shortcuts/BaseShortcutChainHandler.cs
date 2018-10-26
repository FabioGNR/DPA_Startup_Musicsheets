using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Editing.Shortcuts
{
    public abstract class BaseShortcutChainHandler : IShortcutChainHandler
    {
        private IShortcutChainHandler _nextHandler = null;


        public bool Handle(ISet<Key> pressedKeys)
        {
            bool handled = TryHandle(pressedKeys);
            if (!handled && _nextHandler != null)
            {
                return _nextHandler.Handle(pressedKeys);
            }
            return handled;
        }

        public void SetNext(IShortcutChainHandler nextHandler)
        {
            _nextHandler = nextHandler;
        }

        protected abstract bool TryHandle(ISet<Key> pressedKeys);
    }
}
