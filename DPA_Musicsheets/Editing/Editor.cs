using DPA_Musicsheets.Editing.Commands;
using DPA_Musicsheets.Editing.Memento;
using DPA_Musicsheets.Editing.State;
using DPA_Musicsheets.Factories;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models.Domain;
using DPA_Musicsheets.ViewModels;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPA_Musicsheets.Editing
{
    public class Editor : IEditor
    {
        private MusicLoader _musicLoader;
        private KeyDispatcher _keyDipatcher;
        private LilypondViewModel _lilypondViewModel;
        private MainViewModel _mainViewModel;
        private IEditorState currentState;
        private SaveAsCommand saveAsCommand = new SaveAsCommand();
        private bool _compositionChangedByCommand = false;

        public EditorCaretaker CareTaker { get; private set; }
        private Composition lastSavedComp;

        public Editor(MusicLoader musicLoader, KeyDispatcher keyDispatcher,
            LilypondViewModel lilypondViewModel)
        {
            _musicLoader = musicLoader;
            _keyDipatcher = keyDispatcher;
            _lilypondViewModel = lilypondViewModel;
            _mainViewModel = ServiceLocator.Current.GetInstance<MainViewModel>(); ;
            SetState(new IdleEditorState(this));
            _musicLoader.OnCompositionChanged += _musicLoader_OnCompositionChanged;
            _lilypondViewModel.TextChanged += _lilypondViewModel_TextChanged;
            _mainViewModel.OnWindowClosing += _mainViewModel_OnWindowClosing;
        }

        private void _lilypondViewModel_TextChanged(object sender, string lilypondText)
        {
            currentState.TextChanged();
        }

        private void _musicLoader_OnCompositionChanged(object sender, CompositionChangedArgs args)
        {
            if (args.IsFresh)
            {
                CareTaker = new EditorCaretaker();
            }
            if (!_compositionChangedByCommand)
            {
                SaveMemento(args.NewComposition);
            }
        }

        private void _mainViewModel_OnWindowClosing(CancelEventArgs args)
        {
            if (currentState.CanClose())
            {
                if (!lastSavedComp?.Equals(CareTaker.CurrentItem) ?? false)
                {
                    var result = MessageBox.Show("You have not saved. Do you want to save before closing?", "Save?", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Yes)
                    {
                        args.Cancel = true;

                        ExecuteCommand(saveAsCommand);
                    }
                    else if (result == MessageBoxResult.Cancel) args.Cancel = true;
                }
            }
            else
            {
                args.Cancel = true;
            }

        }

        public void SetState(IEditorState state)
        {
            currentState = state;
        }



        public void RenderAfterChange()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Composition composition = new LilypondCompositionFactory().ReadComposition(_lilypondViewModel.LilypondText);
                Render(composition);
            }));
        }

        public bool CanCommandExecute(IEditorCommand command)
        {
            var args = new CommandArgs(this)
            {
                LilypondText = _lilypondViewModel.LilypondText,
                SelectionEnd = 0, // _lilypondViewModel.
                SelectionStart = 0 //
            };
            return command.CanExecute(args);
        }

        public void ExecuteCommand(IEditorCommand command)
        {
            _compositionChangedByCommand = true;
            var args = new CommandArgs(this)
            {
                LilypondText = _lilypondViewModel.LilypondText,
                SelectionEnd = 0, // _lilypondViewModel.
                SelectionStart = 0 //
            };
            if (command.CanExecute(args))
            {
                command.Execute(args);
            }
            _compositionChangedByCommand = false;
        }

        public void Render(Composition composition)
        {
            _musicLoader.SetComposition(composition);
        }

        private Composition SaveMemento(Composition composition)
        {
            CareTaker.Save(new CompositionMemento(composition));
            return composition;
        }

        private Composition CreateComposition(string lilypondText)
        {
            return new LilypondCompositionFactory().ReadComposition(lilypondText);
        }

        public void setLastSavedComp(Composition comp)
        {
            lastSavedComp = comp;
        }
    }
}
