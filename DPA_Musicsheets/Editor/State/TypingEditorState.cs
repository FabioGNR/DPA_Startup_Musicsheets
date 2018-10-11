using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DPA_Musicsheets.Editor.State
{
    class TypingEditorState : BaseEditorState
    {

        Timer idleTimer;
        public TypingEditorState(IEditor context) : base(context)
        {
            idleTimer = new Timer(1500);
            idleTimer.AutoReset = false;
            idleTimer.Elapsed += IdleTimer_Elapsed;
        }

        private void IdleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            IEditorState next = new GeneratingEditorState(context);
            context.SetState(next);
        }

        public override void TextChanged()
        {
            isSaved = false;
            needsSaving = true;
            idleTimer.Stop();
            idleTimer.Start();
            base.TextChanged();
        }
    }
}
