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
            idleTimer.Start();
        }

        private void IdleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            context.RenderAfterChange();
            context.SetState(new IdleEditorState(context));
        }

        public override void TextChanged()
        {
            idleTimer.Stop();
            idleTimer.Start();
            base.TextChanged();
        }

        public override bool CanClose()
        {
            return false;
        }
    }
}
