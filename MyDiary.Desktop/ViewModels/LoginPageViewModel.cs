namespace MyDiary.Desktop.ViewModels
{
    using System.Windows.Input;
    using System.Threading.Tasks;

    using MyDiary.Desktop.Common;
    using MyDiary.Desktop.Http;
    using System;
    using Newtonsoft.Json.Linq;
    using System.Windows.Controls;

    public class LoginPageViewModel : ViewModelBase
    {
        private string email;
        private string password;
        private string infoText;
        private bool serverResponseReady;
        private ICommand loginCommand;
        public delegate void EventHandler(bool problemWithConnection, bool successful, string message);
        public event EventHandler ResponseFromServer;

        public LoginPageViewModel()
        {
            this.Email = StringResources.Email;
            this.InfoText = string.Empty;
            this.Password = StringResources.Password;
            this.ServerResponseReady = false;
        }

        public ICommand LoginCommand
        {
            get
            {
                if (this.loginCommand == null)
                {
                    this.loginCommand = new DelegateCommand(execute: this.LoginAsync);
                }

                return this.loginCommand;
            }
            set
            {
                this.loginCommand = value;
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
                this.OnPropertyChanged("Email");
            }
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

        public bool IsLogged
        {
            get
            {
                return !string.IsNullOrEmpty(LocalSettingsManager.Instance.GetAccessToken());
            }
        }

        public async void LoginAsync(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if(passwordBox != null)
            {
                this.Password = passwordBox.Password;
            }

            if (this.IsValid())
            {
                this.ServerResponseReady = false;
                Utils.AppWait();
                var result = await MyDiaryHttpRequester.Instance.LoginAsync(this.Email, this.Password);
                var problemWithConnection = result.Item1;
                var isSuccessStatusCode = result.Item2;
                var pageContent = result.Item3;
                string message = string.Empty;

                try
                {
                    JToken tokenJson = JObject.Parse(pageContent);
                    if (isSuccessStatusCode)
                    {
                        string token = (string)tokenJson.SelectToken(Constants.JsonKeyToken);
                        message = token;
                    }
                    else
                    {
                        string error = (string)tokenJson.SelectToken(Constants.JsonKeyError);
                        if (error != null)
                        {
                            message = (string)tokenJson.SelectToken(Constants.JsonKeyErrorDescription);
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = StringResources.TitleProblemOccurred;
                }

                if (!problemWithConnection && isSuccessStatusCode)
                {
                    LocalSettingsManager.Instance.SaveAccessToken(message);
                }


                Utils.AppResume();
                this.ServerResponseReady = true;
                EventHandler handler = ResponseFromServer;
                if (handler != null)
                {
                    handler(problemWithConnection, isSuccessStatusCode, message);
                }
            }
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
               // MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidEmail);
                this.InfoText = StringResources.ContentInvalidEmail;
                return false;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
               // MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidPassword);
                this.InfoText = StringResources.ContentInvalidPassword;
                return false;
            }

            return true;
        }
    }
}
