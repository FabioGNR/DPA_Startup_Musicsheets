using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPA_Musicsheets.Editing.State
{
    abstract class BaseEditorState : IEditorState
    {
        protected IEditor context;
        protected bool isSaved;
        protected bool needsSaving;

        public BaseEditorState(IEditor context)
        {
            this.context = context;
        }

        virtual public void Close()
        {
            Application.Current.Shutdown();
        }

        virtual public void TextChanged()
        {

        }
    }
}
