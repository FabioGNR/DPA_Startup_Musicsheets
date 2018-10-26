using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editing.State
{
    class IdleEditorState : BaseEditorState
    {
        public IdleEditorState(IEditor context) : base(context)
        {
        }

        public override void TextChanged()
        {
            base.TextChanged();
            IEditorState next = new TypingEditorState(context);
            context.SetState(next);
        }
    }
}
