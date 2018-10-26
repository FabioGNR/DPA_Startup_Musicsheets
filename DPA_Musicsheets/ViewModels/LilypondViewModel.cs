﻿using DPA_Musicsheets.Editor.Memento;
using DPA_Musicsheets.Editor.Saving;
using DPA_Musicsheets.Editor.State;
using DPA_Musicsheets.Factories;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Models.Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TriggerRenderHandler = DPA_Musicsheets.Managers.MusicLoader.TriggerRenderHandler;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase, IEditor
    {
        private MusicLoader _musicLoader;

        private string _text;
        private string _previousText;
        private string _nextText;

        private IEditorState currentState;
        EditorCaretaker careTaker = new EditorCaretaker();

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

        private bool _textChangedByCommand = false;

        public LilypondViewModel(MusicLoader musicLoader)
        {
            musicLoader.OnCompositionChanged += MusicLoader_OnCompositionChanged;
            _musicLoader = musicLoader;

            _text = "Your lilypond text will appear here.";
            SetState(new IdleEditorState(this));
        }

        private void MusicLoader_OnCompositionChanged(object sender, Composition composition, bool isFresh)
        {
            if (isFresh)
            {
                careTaker = new EditorCaretaker();
            }
            SetComposition(composition);
        }

        public void SetComposition(Composition composition)
        {
            if (!_textChangedByCommand)
            {
                SaveMemento(composition);
            }
            var lilypondText = LilypondFactory.GetLilypond(composition);
            LilypondTextLoaded(lilypondText);

            UndoCommand.RaiseCanExecuteChanged();
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
            _textChangedByCommand = false;
            if (!_textChangedByLoad)
            {
                currentState.TextChanged();
            }
        });

        public void SetState(IEditorState state)
        {
            currentState = state;
        }

        public void RenderAfterChange()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Composition composition = new LilypondCompositionFactory().ReadComposition(LilypondText);
                    Render(composition);
                }));
        }

        private Composition CreateComposition()
        {
            return new LilypondCompositionFactory().ReadComposition(LilypondText);
        }

        #region Commands for buttons like Undo, Redo and SaveAs
        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            _textChangedByCommand = true;
            Composition composition = new Composition();
            composition.Restore(careTaker.Undo());
            Render(composition);
            RedoCommand.RaiseCanExecuteChanged();
        }, () => careTaker.CanUndo);

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            _textChangedByCommand = true;
            Composition composition = new Composition();
            composition.Restore(careTaker.Redo());
            Render(composition);
            UndoCommand.RaiseCanExecuteChanged();
        }, () => careTaker.CanRedo);

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = AbstractSaver.GetFileTypeFilter() };
            if (saveFileDialog.ShowDialog() == true)
            {
                var composition = CreateComposition();
                var filename = saveFileDialog.FileName;
                try
                {
                    AbstractSaver.SaveToFile(composition, filename);
                }
                catch (NotSupportedException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        });

        private Composition SaveMemento(Composition composition)
        {
            careTaker.Save(new CompositionMemento(composition));
            return composition;
        }

        private void Render(Composition composition)
        {
            _musicLoader.SetComposition(composition);
        }
        #endregion Commands for buttons like Undo, Redo and SaveAs
    }
}
