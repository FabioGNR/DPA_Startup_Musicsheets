using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPA_Musicsheets.Editor.State
{
    abstract class BaseEditorState : IEditorState
    {
        protected IEditor context;

        public BaseEditorState(IEditor context)
        {
            this.context = context;
        }

        virtual public bool CanClose()
        {
            return true;
        }

        virtual public void TextChanged()
        {

        }
    }
}
