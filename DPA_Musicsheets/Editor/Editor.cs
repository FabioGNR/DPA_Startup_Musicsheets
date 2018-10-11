using DPA_Musicsheets.Editor.State;
using DPA_Musicsheets.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggerRenderHandler = DPA_Musicsheets.Managers.MusicLoader.TriggerRenderHandler;

namespace DPA_Musicsheets.Editor
{
    class Editor : IEditor
    {
        private IEditorState currentState;

        public event TriggerRenderHandler RenderTriggered;

        public void Render()
        {
            RenderTriggered?.Invoke();
        }

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
