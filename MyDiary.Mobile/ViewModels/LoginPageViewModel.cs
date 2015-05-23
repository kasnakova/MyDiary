namespace MyDiary.Mobile.ViewModels
{
    using System.Windows.Input;
    using System.Threading.Tasks;

    using MyDiary.Mobile.Common;
    using MyDiary.Mobile.Http;
    using System;
    using Newtonsoft.Json.Linq;

    public class LoginPageViewModel : ViewModelBase
    {
        private string email;
        private string password;
        private bool serverResponseReady;
        private ICommand loginCommand;
        public delegate void EventHandler(bool problemWithConnection, bool successful, string message);
        public event EventHandler ResponseFromServer;

        public LoginPageViewModel()
        {
            this.Email = StringResources.Email;
            this.Password = StringResources.Password;
            this.ServerResponseReady = false;
            this.LoginAsync();
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

        //protected virtual void OnThresholdReached(EventArgs e)
        //{
        //    EventHandler handler = ResponseFromServer;
        //    if (handler != null)
        //    {
        //        handler();
        //    }
        //}

        public async void LoginAsync()
        {
            if (this.IsValid())
            {
                this.ServerResponseReady = false;
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
            if(string.IsNullOrEmpty(this.Email))
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidEmail);
                return false;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidPassword);
                return false;
            }

            return true;
        }
    }
}
