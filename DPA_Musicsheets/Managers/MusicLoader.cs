
using DPA_Musicsheets.Converters;
using DPA_Musicsheets.Factories;
using DPA_Musicsheets.Models;
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
    /// TODO: Clean this class up.
    /// </summary>
    public class MusicLoader
    {

        public MainViewModel MainViewModel { get; set; }
        public LilypondViewModel LilypondViewModel { get; set; }
        public MidiPlayerViewModel MidiPlayerViewModel { get; set; }
        public StaffsViewModel StaffsViewModel { get; set; }

        /// <summary>
        /// Opens a file.
        /// TODO: Remove the switch cases and delegate.
        /// TODO: Remove the knowledge of filetypes. What if we want to support MusicXML later?
        /// TODO: Remove the calling of the outer viewmodel layer. We want to be able reuse this in an ASP.NET Core application for example.
        /// </summary>
        /// <param name="fileName"></param>
        public void OpenFile(string fileName)
        {
            ICompositionFactory factory = AbstractCompositionFactory.GetFactory(Path.GetExtension(fileName));

            Models.Domain.Composition composition = factory.ReadComposition(fileName);

            LoadCompositionIntoViewModel(composition);

        }

        public void LoadCompositionIntoViewModel(Models.Domain.Composition composition)
        {
            this.LilypondViewModel.SetComposition(composition);
            this.StaffsViewModel.SetComposition(composition);
        }
    }
}
