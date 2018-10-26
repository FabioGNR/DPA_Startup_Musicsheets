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

        private readonly MusicLoader _musicLoader;


        private string _text;

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
                _text = value;
                RaisePropertyChanged(() => LilypondText);
            }
        }
        public int SelectionStart { get; private set; }

        public int SelectionLength { get; private set; }

        private bool _textChangedByLoad = false;

        private readonly UndoCommand _undoCommand = new UndoCommand();
        private readonly RedoCommand _redoCommand = new RedoCommand();
        private readonly SaveAsCommand _saveAsCommand = new SaveAsCommand();

        private IEditor _editor = new DummyEditor();

        public LilypondViewModel(MusicLoader musicLoader, Editor editor)
        {
            musicLoader.OnCompositionChanged += MusicLoader_OnCompositionChanged;

            _musicLoader = musicLoader;
            editor.SetLilypondViewModel(this);
            _editor = editor;

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
            LilypondText = text;
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

        /// <summary>
        /// This occurs when the selection in the textbox has changed.
        /// </summary>
        public ICommand SelectionChangedCommand => new RelayCommand<RoutedEventArgs>((args) =>
        {
            if(args.OriginalSource is TextBox textBox)
            {
                SelectionStart = textBox.SelectionStart;
                SelectionLength = textBox.SelectionLength;
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
