using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editing.State
{
    public interface IEditorState
    {
        void Close();
        void TextChanged();
    }
}
