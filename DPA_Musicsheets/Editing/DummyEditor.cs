using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Editing.Commands;
using DPA_Musicsheets.Editing.State;

namespace DPA_Musicsheets.Editing
{
    class DummyEditor : IEditor
    {
        public bool CanCommandExecute(IEditorCommand command) => false;

        public void ExecuteCommand(IEditorCommand command)
        {
            // dummy
        }

        public void RenderAfterChange()
        {
            // dummy
        }

        public void SetState(IEditorState state)
        {
            // dummy
        }
    }
}
