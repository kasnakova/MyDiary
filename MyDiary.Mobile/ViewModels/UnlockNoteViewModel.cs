namespace MyDiary.Mobile.ViewModels
{
    using MyDiary.Mobile.Common;
    using MyDiary.Mobile.Http;
    using System.Windows.Input;

    public class UnlockNoteViewModel : ViewModelBase
    {
        private string password;
        private ICommand doneCommand;
        private ICommand cancelCommand;
        public delegate void EventHandler(string noteText);
        public event EventHandler OnPopupClosed;

        public UnlockNoteViewModel()
        {
            this.Password = StringResources.Password;
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
                this.OnPropertyChanged("Password");
            }
        }

        public ICommand DoneCommand
        {
            get
            {
                if (this.doneCommand == null)
                {
                    this.doneCommand = new DelegateCommand(execute: this.GetDecryptedNoteText);
                }

                return this.doneCommand;
            }
            set
            {
                this.doneCommand = value;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (this.cancelCommand == null)
                {
                    this.cancelCommand = new DelegateCommand(execute: this.Cancel);
                }

                return this.cancelCommand;
            }
            set
            {
                this.cancelCommand = value;
            }
        }

        private void GetDecryptedNoteText()
        {
            if(this.IsValid())
            {
                EventHandler handler = OnPopupClosed;
                if (handler != null)
                {
                    handler(this.Password);
                }
            }
        }

        private void Cancel()
        {
            EventHandler handler = OnPopupClosed;
            if (handler != null)
            {
                handler(null);
            }
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(this.Password))
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidPassword);
                return false;
            }

            if (this.Password.Length < Constants.MinPasswordLength)
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidPassword, string.Format(StringResources.ContentInvalidPasswordLength, Constants.MinPasswordLength));
                return false;
            }

            return true;
        }
    }
}
