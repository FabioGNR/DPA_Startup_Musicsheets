using DPA_Musicsheets.Editor.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editor
{
    class Editor : IEditor
    {
        private IEditorState currentState;
        public void SetState(IEditorState state)
        {
            currentState = state;
        }

        public void TextChanged()
        {
            currentState.TextChanged();
        }
    }
}
