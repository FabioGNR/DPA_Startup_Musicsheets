﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.Editing.Shortcuts
{
    class ShortcutPattern
    {
        public Dictionary<Key, bool> RequiredKeyStates = new Dictionary<Key, bool>();
    }
}
