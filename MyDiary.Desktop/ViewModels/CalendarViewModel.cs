using MyDiary.Desktop.Common;
using MyDiary.Desktop.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
namespace MyDiary.Desktop.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private ObservableCollection<NoteViewModel> notes;
        private NoteViewModel noteToUnlock;
        private int noteToDeleteId;
        private bool hasNotes;
        private bool unlockingNote;
        private string infoText;
        private ICommand confirmCommand;
        private ICommand unlockNoteCommand;
        private DateTime selectedDate;
      //  public UnlockNoteViewModel UnlockNoteViewModel { get; set; }
        public delegate void EventHandler(DateTime date);
        public event EventHandler SelectedDateChanged;

        public CalendarViewModel()
        {
            this.Notes = new List<NoteViewModel>();
            //if (LocalSettingsManager.Instance.IsOffline())
            //{
            //    this.HasNotes = true;
            //}
            //else
            //{
            //    this.HasNotes = false;
            //}
            this.HasNotes = true;
            this.InfoText = string.Empty;
            this.UnlockingNote = false;
            this.SelectedDate = DateTime.UtcNow;
            //this.UnlockNoteViewModel = new UnlockNoteViewModel();
            //this.UnlockNoteViewModel.OnPopupClosed += GetDecryptedNoteText;
            PopulateNotes();
        }

        public ICommand ConfirmCommand
        {
            get
            {
                if (this.confirmCommand == null)
                {
                    this.confirmCommand = new DelegateCommand(execute: this.DeleteNoteAsync);
                }

                return this.confirmCommand;
            }
            set
            {
                this.confirmCommand = value;
            }
        }

        public ICommand UnlockNoteCommand
        {
            get
            {
                if (this.unlockNoteCommand == null)
                {
                    this.unlockNoteCommand = new DelegateCommand(execute: this.GetDecryptedNoteText);
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

        public string InfoText
        {
            get
            {
                return this.infoText;
            }
            set
            {
                this.infoText = value;
                this.OnPropertyChanged("InfoText");
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

        public void OnDateChanged(DateTime date)
        {
            this.SelectedDate = date;
            this.PopulateNotes();
            //EventHandler handler = SelectedDateChanged;
            //if (handler != null)
            //{
            //    handler(this.SelectedDate);
            //}
        }

        public void UnlockNote(object parameter)
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
        }

        private async void DeleteNoteAsync()
        {
            string token = LocalSettingsManager.Instance.GetAccessToken();
            Utils.AppWait();
            var result = await MyDiaryHttpRequester.Instance.DeleteNoteAsync(this.noteToDeleteId, token);
            Utils.AppResume();
            var isSuccessful = result.Item2;
            var problemWithConnection = result.Item1;
            if (problemWithConnection)
            {
                this.InfoText = StringResources.ContentNoConnection;
                return;
            }

            if (isSuccessful)
            {
                this.InfoText = string.Empty;
                var note = this.Notes.FirstOrDefault(n => n.Id == this.noteToDeleteId);
                if (note != null)
                {
                    this.notes.Remove(note);
                }
            }
            else
            {
                var message = result.Item3;
                // JToken tokenJson = JObject.Parse(message);
                //  string msg = (string)tokenJson.SelectToken(Constants.JsonKeyMessage);
                this.InfoText = message;
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

        private async void GetDecryptedNoteText(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string password = string.Empty;
            if (passwordBox != null)
            {
                password = passwordBox.Password;
            }

            this.UnlockingNote = false;
            if (string.IsNullOrEmpty(password))
            {
                return;
            }

            string token = LocalSettingsManager.Instance.GetAccessToken();
            Utils.AppWait();
            var result = await MyDiaryHttpRequester.Instance.GetDecryptedNoteTextAsync(this.noteToUnlock.Id, password, token);
            Utils.AppResume();
            var isSuccessful = result.Item2;
            var problemWithConnection = result.Item1;
            if (problemWithConnection)
            {
                this.InfoText = StringResources.ContentNoConnection;
                return;
            }

            if (isSuccessful)
            {
                this.InfoText = string.Empty;
                var note = this.Notes.FirstOrDefault(n => n.Id == this.noteToUnlock.Id);
                if (note != null)
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
                this.InfoText = msg;
                //MessageDialogManager.ShowConfirmationDialog(StringResources.TitleProblemOccurred, msg);
            }
        }

        private async void GetNotesForDateAsync()
        {
            if (LocalSettingsManager.Instance.IsOffline())
            {
                return;
            }

            string token = LocalSettingsManager.Instance.GetAccessToken();

            // this.ServerResponseReady = false;
            Utils.AppWait();
            var result = await MyDiaryHttpRequester.Instance.GetNotesForDateAsync(this.SelectedDate, token);
            Utils.AppResume();
            //  this.ServerResponseReady = true;
            var problemWithConnection = result.Item1;
            var isSuccessful = result.Item2;

            if (!problemWithConnection)
            {
                if (isSuccessful)
                {
                    this.InfoText = string.Empty;
                    var jsonNotes = result.Item3;
                    this.Notes = JsonConvert.DeserializeObject<ObservableCollection<NoteViewModel>>(jsonNotes);
                    if (this.notes.Count > 0)
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
                    this.InfoText = StringResources.ContentErrorRetrievingNotes;
                }
            }
            else
            {
                this.InfoText = StringResources.ContentNoConnection;
            }
        }

        public bool SaveNotesForDayToFile(string filePath)
        {
            Utils.AppWait();
            var success = FileStore.SaveNotesForDay(this.Notes.ToList(), this.SelectedDate, filePath);
            Utils.AppResume();
            return success;
        }
    }
}
