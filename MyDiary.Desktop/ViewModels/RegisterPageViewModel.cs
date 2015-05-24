using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

using MyDiary.Desktop.Common;
using MyDiary.Desktop.Http;

namespace MyDiary.Desktop.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        private string email;
        private string name;
        private string password;
        private string confirmPassword;
        private string infoText;
        private bool serverResponseReady;
        private ICommand registerCommand;
        public delegate void EventHandler(bool problemWithConnection, bool successful, string message);
        public event EventHandler ResponseFromServer;

        public RegisterPageViewModel()
        {
            this.Email = StringResources.Email;
            this.Name = StringResources.Name;
            this.Password = StringResources.Password;
            this.ConfirmPassword = StringResources.Password;
            this.InfoText = string.Empty;
            this.ServerResponseReady = true;
        }

        public ICommand RegisterCommand
        {
            get
            {
                if (this.registerCommand == null)
                {
                    this.registerCommand = new DelegateCommand(execute: this.RegisterAsync);
                }

                return this.registerCommand;
            }
            set
            {
                this.registerCommand = value;
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

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.OnPropertyChanged("Name");
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

        public async void RegisterAsync(object parameter)
        {
            var values = (object[])parameter;
            this.Password = (string)values[0];
            this.ConfirmPassword = (string)values[1];

            if (this.IsValid())
            {
               // this.ServerResponseReady = false;
                Utils.AppWait();
                var result = await MyDiaryHttpRequester.Instance.RegisterAsync(this.Email, this.Name, this.Password);
                var problemWithConnection = result.Item1;
                var isSuccessStatusCode = result.Item2;
                var pageContent = result.Item3;
                string message = string.Empty;
                if (!isSuccessStatusCode && !problemWithConnection)
                {
                    JToken tokenJson = JObject.Parse(pageContent);
                    try
                    {
                        var modelState = (JToken)tokenJson.SelectToken(Constants.JsonKeyModelState);
                        var messages = (JArray)modelState.SelectToken(Constants.JsonKeyErrors);
                        message = messages[0].ToString();
                    }
                    catch (Exception ex)
                    {
                        message = StringResources.TitleProblemOccurred;
                    }
                }

                Utils.AppResume();
              //  this.ServerResponseReady = true;
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
                this.InfoText = StringResources.ContentInvalidEmail;
               // MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidEmail);
                return false;
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                this.InfoText = StringResources.ContentInvalidName;
                //MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidName);
                return false;
            }

            if (string.IsNullOrEmpty(this.Password) || string.IsNullOrEmpty(this.ConfirmPassword))
            {
                this.InfoText = StringResources.ContentInvalidPassword;
               // MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidPassword);
                return false;
            }

            if (this.Password.Length < Constants.MinPasswordLength)
            {
                this.InfoText = StringResources.ContentInvalidPasswordLength;
                //MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidPassword, string.Format(StringResources.ContentInvalidPasswordLength, Constants.MinPasswordLength));
                return false;
            }

            if (this.Password != this.ConfirmPassword)
            {
                this.InfoText = StringResources.ContentPasswordMismatch;
               // MessageDialogManager.ShowConfirmationDialog(StringResources.TitlePasswordMismatch, StringResources.ContentPasswordMismatch);
                return false;
            }

            return true;
        }
    }
}
