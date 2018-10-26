using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editing.Commands
{
    public interface IEditorCommand
    {
        bool CanExecute(EditorCommandArgs args);
        void Execute(EditorCommandArgs args);
    }
}
