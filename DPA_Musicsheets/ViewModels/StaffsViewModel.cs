using DPA_Musicsheets.Converters;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models.Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DPA_Musicsheets.ViewModels
{
    public class StaffsViewModel : ViewModelBase
    {
        // These staffs will be bound to.
        public ObservableCollection<MusicalSymbol> Staffs { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="musicLoader">We need the musicloader so it can set our staffs.</param>
        public StaffsViewModel(MusicLoader musicLoader)
        {
            musicLoader.OnCompositionChanged += MusicLoader_OnCompositionChanged;
            Staffs = new ObservableCollection<MusicalSymbol>();
        }

        private void MusicLoader_OnCompositionChanged(object sender, CompositionChangedArgs args)
        {
            SetComposition(args.NewComposition);
        }

        /// <summary>
        /// SetComposition fills the observablecollection with new symbols. 
        /// We don't want to reset the collection because we don't want other classes to create an observable collection.
        /// </summary>
        /// <param name="composition">The new composition to show.</param>
        public void SetComposition(Composition composition)
        {
            var staffConverter = new ToStaffsVisitor();
            foreach (var token in composition.Tokens)
            {
                token.Accept(staffConverter);
            }
            try
            {
                Staffs.Clear();
                foreach (var symbol in staffConverter.Symbols)
                {
                    Staffs.Add(symbol);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
