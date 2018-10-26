using DPA_Musicsheets.Editing.Commands;
using DPA_Musicsheets.Editing.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editing
{
    interface IEditor
    {
        void SetState(IEditorState state);
        void RenderAfterChange();
        bool CanCommandExecute(IEditorCommand command);
        void ExecuteCommand(IEditorCommand command);
    }
}
