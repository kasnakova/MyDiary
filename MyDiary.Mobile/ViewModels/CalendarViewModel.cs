using MyDiary.Mobile.Common;
using MyDiary.Mobile.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using Windows.UI.Popups;
namespace MyDiary.Mobile.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private ObservableCollection<NoteViewModel> notes;
        private NoteViewModel noteToUnlock;
        private int noteToDeleteId;
        private bool hasNotes;
        private bool unlockingNote;
        private ICommand dateChangedCommand;
        private ICommand unlockNoteCommand;
        private DateTime selectedDate;
        public UnlockNoteViewModel UnlockNoteViewModel { get; set; }
        public delegate void EventHandler(DateTime date);
        public event EventHandler SelectedDateChanged;

        public CalendarViewModel()
        {
            this.Notes = new List<NoteViewModel>();
            if(LocalSettingsManager.Instance.IsOffline())
            {
                this.HasNotes = true;
            }
            else
            {
                this.HasNotes = false;
            }

            this.UnlockingNote = false;
            this.SelectedDate = DateTime.Now;
            this.UnlockNoteViewModel = new UnlockNoteViewModel();
            this.UnlockNoteViewModel.OnPopupClosed += GetDecryptedNoteText;
            PopulateNotes();
        }

        public ICommand DateChangedCommand
        {
            get
            {
                if (this.dateChangedCommand == null)
                {
                    this.dateChangedCommand = new DelegateCommand(execute: this.OnDateChanged);
                }

                return this.dateChangedCommand;
            }
            set
            {
                this.dateChangedCommand = value;
            }
        }

        public ICommand UnlockNoteCommand
        {
            get
            {
                if (this.unlockNoteCommand == null)
                {
                    this.unlockNoteCommand = new DelegateCommand(execute: this.UnlockNote);
                }

                return this.unlockNoteCommand;
            }
            set
            {
                this.unlockNoteCommand = value;
            }
        }

        public IEnumerable<NoteViewModel> Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                if (this.notes == null)
                {
                    this.notes = new ObservableCollection<NoteViewModel>();
                }

                this.notes.Clear();
                this.notes.AddRange<NoteViewModel>(value);
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return this.selectedDate;
            }
            set
            {
                this.selectedDate = value;
                this.OnPropertyChanged("SelectedDate");
            }
        }

        public bool UnlockingNote
        {
            get
            {
                return this.unlockingNote;
            }
            set
            {
                this.unlockingNote = value;
                this.OnPropertyChanged("UnlockingNote");
            }
        }

        public bool HasNotes
        {
            get
            {
                return this.hasNotes;
            }
            set
            {
                this.hasNotes = value;
                this.OnPropertyChanged("HasNotes");
            }
        }

        private void OnDateChanged(object parameter)
        {
            this.SelectedDate = (DateTime)parameter;
            this.PopulateNotes();
            EventHandler handler = SelectedDateChanged;
            if (handler != null)
            {
                handler(this.SelectedDate);
            }
        }

        private void UnlockNote(object parameter)
        {
            this.noteToUnlock = parameter as NoteViewModel;
           if (this.noteToUnlock != null && this.noteToUnlock.HasPassword)
            {
                this.UnlockingNote = true;
            }
        }

        public void DeleteNote(int id)
        {
            this.noteToDeleteId = id;
            MessageDialogManager.ShowDialog(StringResources.TitleDeleteNote, StringResources.ContentDeleteNote, this.DeleteNoteAsync);
        }

        private async void DeleteNoteAsync(IUICommand command)
        {
            string token = LocalSettingsManager.Instance.GetAccessToken();
            var result = await MyDiaryHttpRequester.Instance.DeleteNoteAsync(this.noteToDeleteId, token);
            var isSuccessful = result.Item2;
            var problemWithConnection = result.Item1;
            if (problemWithConnection)
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleNoConnection, StringResources.ContentNoConnection);
                return;
            }

            if (isSuccessful)
            {
                var note = this.Notes.FirstOrDefault(n => n.Id == this.noteToDeleteId);
                if (note != null)
                {
                    this.notes.Remove(note);
                //    this.Notes = this.notes;
                }
            }
            else
            {
                var message = result.Item3;
               // JToken tokenJson = JObject.Parse(message);
              //  string msg = (string)tokenJson.SelectToken(Constants.JsonKeyMessage);
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleProblemOccurred, message);
            }
        }

        public void PopulateNotes()
        {
            this.GetNotesForDateAsync();
        }

        public void OnNoteSaved()
        {
            PopulateNotes();
        }

        private async void GetDecryptedNoteText(string password)
        {
            this.UnlockingNote = false;
            if(string.IsNullOrEmpty(password))
            {
                return;
            }

            string token = LocalSettingsManager.Instance.GetAccessToken();
            var result = await MyDiaryHttpRequester.Instance.GetDecryptedNoteTextAsync(this.noteToUnlock.Id, password, token);
            var isSuccessful = result.Item2;
            var problemWithConnection = result.Item1;
            if(problemWithConnection)
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleNoConnection, StringResources.ContentNoConnection);
                return;
            }

            if (isSuccessful)
            {
                var note = this.Notes.FirstOrDefault(n => n.Id == this.noteToUnlock.Id);
                if(note != null)
                {
                    var noteText = result.Item3.Substring(1, result.Item3.Length - 2);
                    note.NoteText = noteText;
                    note.HasPassword = false;
                }
            }
            else
            {
                var message = result.Item3;
                JToken tokenJson = JObject.Parse(message);
                string msg = (string)tokenJson.SelectToken(Constants.JsonKeyMessage);
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleProblemOccurred, msg);
            }
        }

        private async void GetNotesForDateAsync()
        {
            if(LocalSettingsManager.Instance.IsOffline())
            {
                return;
            }

            string token = LocalSettingsManager.Instance.GetAccessToken();
           
           // this.ServerResponseReady = false;
            var result = await MyDiaryHttpRequester.Instance.GetNotesForDateAsync(this.SelectedDate, token);
          //  this.ServerResponseReady = true;
            var problemWithConnection = result.Item1;
            var isSuccessful = result.Item2;

            if (!problemWithConnection)
            {
                if (isSuccessful)
                {
                    var jsonNotes = result.Item3;
                    this.Notes = JsonConvert.DeserializeObject<ObservableCollection<NoteViewModel>>(jsonNotes);
                    if(this.notes.Count > 0)
                    {
                        this.HasNotes = true;
                    }
                    else
                    {
                        this.HasNotes = false;
                    }
                }
                else
                {
                    MessageDialogManager.ShowConfirmationDialog(StringResources.TitleProblemOccurred, StringResources.ContentErrorRetrievingNotes);
                }
            }
            else
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleNoConnection, StringResources.ContentNoConnection);
            }
        }
    }
}
