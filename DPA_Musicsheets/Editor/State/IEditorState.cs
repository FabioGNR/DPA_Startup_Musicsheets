using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editor.State
{
    interface IEditorState
    {
        void Close();
        void TextChanged();
    }
}
