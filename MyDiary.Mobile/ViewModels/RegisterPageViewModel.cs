using MyDiary.Mobile.Common;
using MyDiary.Mobile.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDiary.Mobile.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        private string email;
        private string name;
        private string password;
        private string confirmPassword;
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

        public async void RegisterAsync()
        {
            if (this.IsValid())
            {
                this.ServerResponseReady = false;
                var result = await MyDiaryHttpRequester.Instance.RegisterAsync(this.Email, this.Name, this.Password);
                var problemWithConnection = result.Item1;
                var isSuccessStatusCode = result.Item2;
                var pageContent = result.Item3;
                string message = string.Empty;
                if (!isSuccessStatusCode)
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
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidEmail);
                return false;
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidName);
                return false;
            }

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
