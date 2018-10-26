using DPA_Musicsheets.Editing;
using DPA_Musicsheets.Editing.Commands;
using DPA_Musicsheets.Editing.Memento;
using DPA_Musicsheets.Editing.Saving;
using DPA_Musicsheets.Editing.State;
using DPA_Musicsheets.Factories;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models.Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        public delegate void LilypondTextChangedHandler(object sender, string lilypondText);
        public event LilypondTextChangedHandler TextChanged;

        private MusicLoader _musicLoader;
        private MainViewModel _mainViewModel;

        private Composition lastSavedComp;
        private bool needsSaving;

        private string _text;
        private string _previousText;
        private string _nextText;

        /// <summary>
        /// This text will be in the textbox.
        /// It can be filled either by typing or loading a file so we only want to set previoustext when it's caused by typing.
        /// </summary>
        public string LilypondText
        {
            get
            {
                return _text;
            }
            set
            {
                if (!_waitingForRender && !_textChangedByLoad)
                {
                    _previousText = _text;
                }
                _text = value;
                RaisePropertyChanged(() => LilypondText);
            }
        }

        private bool _textChangedByLoad = false;
        private bool _waitingForRender = false;

        private UndoCommand _undoCommand = new UndoCommand();
        private RedoCommand _redoCommand = new RedoCommand();
        private SaveAsCommand _saveAsCommand = new SaveAsCommand();

        private Editor _editor;

        public LilypondViewModel(MusicLoader musicLoader, KeyDispatcher keyDispatcher)
        {
            musicLoader.OnCompositionChanged += MusicLoader_OnCompositionChanged;

            _musicLoader = musicLoader;
            _editor = new Editor(musicLoader, keyDispatcher, this);

            _text = "Your lilypond text will appear here.";
        }

        private void MusicLoader_OnCompositionChanged(object sender, CompositionChangedArgs args)
        {
            SetComposition(args.NewComposition);
        }

        public void SetComposition(Composition composition)
        {
            var lilypondText = LilypondFactory.GetLilypond(composition);
            LilypondTextLoaded(lilypondText);

            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
        }

        private void LilypondTextLoaded(string text)
        {
            _textChangedByLoad = true;
            LilypondText = _previousText = text;
            _textChangedByLoad = false;
        }

        /// <summary>
        /// This occurs when the text in the textbox has changed. This can either be by loading or typing.
        /// </summary>
        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            if (!_textChangedByLoad)
            {
                TextChanged?.Invoke(this, LilypondText);
            }
        });

        #region Commands for buttons like Undo, Redo and SaveAs
        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            _editor.ExecuteCommand(_undoCommand);
            RedoCommand.RaiseCanExecuteChanged();
        }, () => _editor.CanCommandExecute(_undoCommand));

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            _editor.ExecuteCommand(_redoCommand);
            UndoCommand.RaiseCanExecuteChanged();
        }, () => _editor.CanCommandExecute(_redoCommand));

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            _editor.ExecuteCommand(_saveAsCommand);
        }, () => _editor.CanCommandExecute(_saveAsCommand));

        #endregion Commands for buttons like Undo, Redo and SaveAs
    }
}
