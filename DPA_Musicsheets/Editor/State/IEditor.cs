using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggerRenderHandler = DPA_Musicsheets.Managers.MusicLoader.TriggerRenderHandler;

namespace DPA_Musicsheets.Editor.State
{
    interface IEditor
    {
        void SetState(IEditorState state);
        void Render();
    }
}
