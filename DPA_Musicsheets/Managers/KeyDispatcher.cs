using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Managers
{
    public class KeyDispatcher
    {
        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;

        public void DispatchKeyDown(KeyEventArgs args)
        {
            KeyDown?.Invoke(this, args);
        }
        public void DispatchKeyUp(KeyEventArgs args)
        {
            KeyUp?.Invoke(this, args);
        }
    }
}
