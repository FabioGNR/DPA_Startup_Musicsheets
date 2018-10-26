using DPA_Musicsheets.Editing.Saving;
using DPA_Musicsheets.Factories;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPA_Musicsheets.Editing.Commands
{
    public class SaveAsCommand : IEditorCommand
    {
        public bool CanExecute(CommandArgs args) => true;

        public void Execute(CommandArgs args)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = AbstractSaver.GetFileTypeFilter() };
            if (saveFileDialog.ShowDialog() == true)
            {
                var composition = new LilypondCompositionFactory().ReadComposition(args.LilypondText);

                var filename = saveFileDialog.FileName;
                try
                {
                    AbstractSaver.SaveToFile(composition, filename);
                    args.Editor.SetLastSavedComp(composition);
                }
                catch (NotSupportedException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
    }
}
