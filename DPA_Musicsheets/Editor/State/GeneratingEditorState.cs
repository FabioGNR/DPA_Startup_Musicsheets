using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editor.State
{
    class GeneratingEditorState : BaseEditorState
    {
        private bool hasTextChanged = false;

        public GeneratingEditorState(IEditor context) : base(context)
        {
            musicLoader.
        }

        public override void TextChanged()
        {
            hasTextChanged = true;
        }

        private void OnRenderComplete()
        {
            IEditorState next;
            if (hasTextChanged)
            {
                next = new TypingEditorState(context);
            }
            else
            {
                next = new IdleEditorState(context);
            }
            context.SetState(next);
        }
    }
}
