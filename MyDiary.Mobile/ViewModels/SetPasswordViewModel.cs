using MyDiary.Mobile.Common;
using System.Windows.Input;
namespace MyDiary.Mobile.ViewModels
{
    public class SetPasswordViewModel : ViewModelBase
    {
        private string password;
        private string confirmPassword;
        private ICommand setPasswordCommand;
        private ICommand cancelCommand;
        public delegate void EventHandler(string password);
        public event EventHandler OnPopupClosed;

        public SetPasswordViewModel()
        {
            this.Password = StringResources.Password;
            this.ConfirmPassword = StringResources.Password;
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

        public string ConfirmPassword
        {
            get
            {
                return this.confirmPassword;
            }
            set
            {
                this.confirmPassword = value;
                this.OnPropertyChanged("ConfirmPassword");
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

        private void SetPassword()
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
            if (string.IsNullOrEmpty(this.Password) || string.IsNullOrEmpty(this.ConfirmPassword))
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidPassword);
                return false;
            }

            if (this.Password.Length < Constants.MinPasswordLength)
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidPassword, string.Format(StringResources.ContentInvalidPasswordLength, Constants.MinPasswordLength));
                return false;
            }

            if (this.Password != this.ConfirmPassword)
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitlePasswordMismatch, StringResources.ContentPasswordMismatch);
                return false;
            }

            return true;
        }
    }
}
