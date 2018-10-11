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
    public class LilypondViewModel : ViewModelBase
    {
        private MusicLoader _musicLoader;
        private StaffsViewModel _staffsViewModel { get; set; }

        private string _text;
        private string _previousText;
        private string _nextText;
        private Editor.Editor context;

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
        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private bool _waitingForRender = false;

        public LilypondViewModel(StaffsViewModel staffsViewModel, MusicLoader musicLoader)
        {
            _staffsViewModel = staffsViewModel;
            musicLoader.OnCompositionChanged += MusicLoader_OnCompositionChanged;
            _musicLoader = musicLoader;

            _text = "Your lilypond text will appear here.";
            context = new Editor.Editor();
            context.RenderTriggered += Context_RenderTriggered;
            context.SetState(new IdleEditorState(context));
        }

        private void MusicLoader_OnCompositionChanged(object sender, Composition composition)
        {
            SetComposition(composition);
        }

        private void Context_RenderTriggered()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                _staffsViewModel.SetComposition(new LilyPondCompositionFactory().ToComposition(LilypondText))));
        }

        public void SetComposition(Composition composition)
        {
            var lilypondText = LilypondFactory.GetLilypond(composition);
            LilypondTextLoaded(lilypondText);
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
            context.TextChanged();
        });

        #region Commands for buttons like Undo, Redo and SaveAs
        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            _nextText = LilypondText;
            LilypondText = _previousText;
            _previousText = null;
        }, () => _previousText != null && _previousText != LilypondText);

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            _previousText = LilypondText;
            LilypondText = _nextText;
            _nextText = null;
            RedoCommand.RaiseCanExecuteChanged();
        }, () => _nextText != null && _nextText != LilypondText);

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            // TODO: In the application a lot of classes know which filetypes are supported. Lots and lots of repeated code here...
            // Can this be done better?
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    //_musicLoader.SaveToMidi(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".ly"))
                {
                    //_musicLoader.SaveToLilypond(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".pdf"))
                {
                    //_musicLoader.SaveToPDF(saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
            }
        });
        #endregion Commands for buttons like Undo, Redo and SaveAs
    }
}
