﻿using DPA_Musicsheets.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editing.Commands
{
    class RedoCommand : IEditorCommand
    {
        public bool CanExecute(EditorCommandArgs args) => args.Editor.CareTaker.CanRedo;

        public void Execute(EditorCommandArgs args)
        {
            Composition composition = new Composition();
            composition.Restore(args.Editor.CareTaker.Redo());
            args.Editor.Render(composition);
        }
    }
}
