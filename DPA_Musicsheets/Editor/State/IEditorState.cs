using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editor.State
{
    public interface IEditorState
    {
        bool CanClose();
        void TextChanged();
    }
}
