using MyDiary.Mobile.Common;
using System.Windows.Input;
using Windows.Media.SpeechRecognition;
using System.Threading.Tasks;
using System;
using MyDiary.Mobile.Http;
namespace MyDiary.Mobile.ViewModels
{
    public class RecordViewModel : ViewModelBase
    {
        private string noteText;
        private bool settingPassword;
        private bool serverResponseReady;
        private string password;
        private DateTime selectedDate;
        private ICommand setPasswordCommand;
        private ICommand speechRecognitionCommand;
        private ICommand saveNoteCommand;
        public SetPasswordViewModel SetPasswordViewModel { get; set; }
        public delegate void EventHandler();
        public event EventHandler NoteSaved;

        public RecordViewModel()
        {
            this.SettingPassword = false;
            this.SetPasswordViewModel = new SetPasswordViewModel();
            this.SelectedDate = DateTime.Now;
            this.SetPasswordViewModel.OnPopupClosed += OnSetPasswordReady;
        }

        public string NoteText
        {
            get
            {
                return this.noteText;
            }
            set
            {
                this.noteText = value;
                this.OnPropertyChanged("NoteText");
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

        public bool SettingPassword
        {
            get
            {
                return this.settingPassword;
            }
            set
            {
                this.settingPassword = value;
                this.OnPropertyChanged("SettingPassword");
            }
        }

        public bool ServerResponseReady
        {
            get
            {
                return this.serverResponseReady;
            }
            set
            {
                this.serverResponseReady = value;
                this.OnPropertyChanged("ServerResponseReady");
            }
        }

        public ICommand SaveNoteCommand
        {
            get
            {
                if (this.saveNoteCommand == null)
                {
                    this.saveNoteCommand = new DelegateCommand(execute: this.SaveNote);
                }

                return this.saveNoteCommand;
            }
            set
            {
                this.saveNoteCommand = value;
            }
        }

        public ICommand SpeechRecognitionCommand
        {
            get
            {
                if (this.speechRecognitionCommand == null)
                {
                    this.speechRecognitionCommand = new DelegateCommand(execute: this.SpeechRecognition);
                }

                return this.speechRecognitionCommand;
            }
            set
            {
                this.speechRecognitionCommand = value;
            }
        }

        public ICommand SetPasswordCommand
        {
            get
            {
                if (this.setPasswordCommand == null)
                {
                    this.setPasswordCommand = new DelegateCommand(execute: this.SetPassword);
                }

                return this.setPasswordCommand;
            }
            set
            {
                this.setPasswordCommand = value;
            }
        }

      
        public void SelectedDateChanged(DateTime date)
        {
            this.SelectedDate = date;
        }

        private void SetPassword()
        {
            if (LocalSettingsManager.Instance.IsOffline())
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInOfflineMode, StringResources.ContentInOfflineMode);
                return;
            }

            if (!this.SettingPassword)
            {
                this.SettingPassword = true;
            }
        }

        private void OnSetPasswordReady(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                this.password = password;
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleSetPassword, password);
            }

            this.SettingPassword = false;
        }


        private async void SaveNote()
        {
            if (LocalSettingsManager.Instance.IsOffline())
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInOfflineMode, StringResources.ContentInOfflineMode);
                return;
            }

            if (this.IsValid())
            {
                string token = LocalSettingsManager.Instance.GetAccessToken();
                //TODO: see about the time - in the ui and calendar
                this.ServerResponseReady = false;
                var result = await MyDiaryHttpRequester.Instance.SaveNoteAsync(this.NoteText, this.password, this.SelectedDate, token);
                this.ServerResponseReady = true;
                var problemWithConnection = result.Item1;
                var isSuccessful = result.Item2;

                if (!problemWithConnection)
                {
                    if (isSuccessful)
                    {
                        EventHandler handler = NoteSaved;
                        if (handler != null)
                        {
                            handler();
                        }

                        MessageDialogManager.ShowConfirmationDialog(StringResources.TitleSuccessSavingNote, StringResources.ContentSuccessSavingNote);
                        this.password = string.Empty;
                    }
                    else
                    {
                        MessageDialogManager.ShowConfirmationDialog(StringResources.TitleProblemOccurred, StringResources.ContentErrorSavingNote);
                    }
                }
                else
                {
                    MessageDialogManager.ShowConfirmationDialog(StringResources.TitleNoConnection, StringResources.ContentNoConnection);
                }
            }
        }

        private async void SpeechRecognition()
        {
            SpeechRecognizer speechRecognizer = new SpeechRecognizer();

            //add dictation grammar to the recognizer
            SpeechRecognitionTopicConstraint topicConstraint =
                new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation,
            "dictation");

            speechRecognizer.Constraints.Add(topicConstraint);

            //compile constraints before performing speech recognition
            await speechRecognizer.CompileConstraintsAsync();
            SpeechRecognitionResult recognitionResult = await speechRecognizer.RecognizeWithUIAsync();
            this.NoteText = recognitionResult.Text;
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(this.NoteText))
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidNote);
                return false;
            }

            return true;
        }
    }
}
