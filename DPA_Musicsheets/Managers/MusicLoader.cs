
using DPA_Musicsheets.Converters;
using DPA_Musicsheets.Factories;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.Domain;
using DPA_Musicsheets.ViewModels;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    /// <summary>
    /// This is the one and only god class in the application.
    /// It knows all about all file types, knows every viewmodel and contains all logic.
    /// </summary>
    public class MusicLoader
    {
        public delegate void TriggerRenderHandler();
        public delegate void CompositionChangedHandler(object sender, Composition composition);
        public event CompositionChangedHandler OnCompositionChanged;

        /// <summary>
        /// Opens a file.
        /// </summary>
        /// <param name="fileName"></param>
        public void OpenFile(string fileName)
        {
            ICompositionFactory factory = AbstractCompositionFactory.GetFactory(Path.GetExtension(fileName));

            Composition composition = factory.ReadComposition(fileName);

            LoadCompositionIntoViewModel(composition);
        }

        public void LoadCompositionIntoViewModel(Models.Domain.Composition composition)
        {
            OnCompositionChanged?.Invoke(this, composition);
        }
    }
}
