using MyDiary.Desktop.Common;
using System.Windows.Input;
using System.Threading.Tasks;
using System;
using MyDiary.Desktop.Http;
using System.Windows.Controls;
namespace MyDiary.Desktop.ViewModels
{
    public class RecordViewModel : ViewModelBase
    {
        private string noteText;
        private bool settingPassword;
        private bool serverResponseReady;
        private bool isSuccessful;
        private string infoText;
        private DateTime selectedDate;
        private ICommand setPasswordCommand;
        private ICommand saveNoteCommand;
      //  public SetPasswordViewModel SetPasswordViewModel { get; set; }
        public string Password { get; set; }
        public delegate void EventHandler();
        public event EventHandler NoteSaved;

        public RecordViewModel()
        {
            this.SettingPassword = false;
            this.IsSuccessful = false;
            this.InfoText = string.Empty;
          //  this.SetPasswordViewModel = new SetPasswordViewModel();
            this.SelectedDate = DateTime.Now;
         //   this.SetPasswordViewModel.OnPopupClosed += OnSetPasswordReady;
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

        public bool IsSuccessful
        {
            get
            {
                return this.isSuccessful;
            }
            set
            {
                this.isSuccessful = value;
                this.OnPropertyChanged("IsSuccessful");
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

        public ICommand SetPasswordCommand
        {
            get
            {
                if (this.setPasswordCommand == null)
                {
                    this.setPasswordCommand = new DelegateCommand(execute: this.OnSetPasswordReady);
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

        public bool SetPassword()
        {
            if (LocalSettingsManager.Instance.IsOffline())
            {
                this.IsSuccessful = false;
                this.InfoText = StringResources.ContentInOfflineMode;
                return false;
            }

            if(string.IsNullOrEmpty(this.NoteText))
            {
                this.IsSuccessful = false;
                this.InfoText = StringResources.ContentInvalidNote;
                return false;
            }

            return true;
        }

        private void OnSetPasswordReady(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            string pass = string.Empty;
            if (passwordBox != null)
            {
                pass = passwordBox.Password;
            }

            if (!string.IsNullOrEmpty(pass))
            {
                this.Password = pass;
                this.IsSuccessful = true;
                this.InfoText = StringResources.TitleSetPassword;
            }

           // this.SettingPassword = false;
        }


        private async void SaveNote()
        {
            if (LocalSettingsManager.Instance.IsOffline())
            {
                this.IsSuccessful = false;
                this.InfoText = StringResources.ContentInOfflineMode;
                return;
            }

            if (this.IsValid())
            {
                string token = LocalSettingsManager.Instance.GetAccessToken();
                Utils.AppWait();
                var result = await MyDiaryHttpRequester.Instance.SaveNoteAsync(this.NoteText, this.Password, this.SelectedDate, token);
                Utils.AppResume();
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

                        this.IsSuccessful = true;
                        this.InfoText = StringResources.ContentSuccessSavingNote;
                        this.Password = string.Empty;
                    }
                    else
                    {
                        this.IsSuccessful = false;
                        this.InfoText = StringResources.ContentErrorSavingNote;
                    }
                }
                else
                {
                    this.IsSuccessful = false;
                    this.InfoText = StringResources.ContentNoConnection;
                }
            }
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(this.NoteText))
            {
                this.IsSuccessful = false;
                this.InfoText = StringResources.ContentInvalidNote;
                return false;
            }

            return true;
        }
    }
}
