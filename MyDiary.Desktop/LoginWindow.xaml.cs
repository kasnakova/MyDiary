using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MyDiary.Desktop.ViewModels;
using MyDiary.Desktop.Common;
using MyDiary.Desktop.Http;
namespace MyDiary.Desktop
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.DataContext = new LoginPageViewModel();
            this.ViewModel.ResponseFromServer += HandleServerResponse;
            CheckIfLoggedOrOffline();
        }

        public LoginPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as LoginPageViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        private async void CheckIfLoggedOrOffline()
        {

            //if (LocalSettingsManager.Instance.IsOffline())
            //{
            //    this.Frame.Navigate(typeof(ReminderPage));
            //    return;
            //}

            if (this.ViewModel.IsLogged)
            {
                var isConnected = await MyDiaryHttpRequester.Instance.IsConnected();
                if (isConnected)
                {
                    Window mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    this.InfoText.Text = StringResources.ContentNoConnection;
                    //MessageDialogManager.ShowConfirmationDialog(StringResources.TitleNoConnection, StringResources.ContentNoConnection);
                }
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
           // this.Frame.Navigate(typeof(HelpPage));
        }

        private void TextBoxEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxEmail.Text = string.Empty;
        }

        private void TextBoxPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxPassword.Password = string.Empty;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Window registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        private void HandleServerResponse(bool problemWithConnection, bool successful, string message)
        {
            //if (LocalSettingsManager.Instance.IsOffline())
            //{
            //    this.Frame.Navigate(typeof(MainPage));
            //    return;
            //}

            if (!problemWithConnection)
            {
                if (successful || this.ViewModel.IsLogged)
                {
                    Window mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    this.InfoText.Text = message;
                    //MessageDialogManager.ShowConfirmationDialog(StringResources.TitleProblemWithLogin, message);
                }
            }
            else
            {
                this.InfoText.Text = StringResources.ContentNoConnection;
                //MessageDialogManager.ShowConfirmationDialog(StringResources.TitleNoConnection, StringResources.ContentNoConnection);
            }
        }

        private void OfflineButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageDialogManager.ShowDialog(StringResources.TitleGoOffline, StringResources.ContentGoOffline, GoOffline);
        }



        //private void GoOffline(IUICommand command)
        //{
        //    LocalSettingsManager.Instance.SetOfflineMode(true);
        //    this.Frame.Navigate(typeof(MainPage));
        //}
    }
}
