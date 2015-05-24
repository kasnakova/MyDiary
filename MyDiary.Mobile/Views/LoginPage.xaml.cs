using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

using MyDiary.Mobile.ViewModels;
using MyDiary.Mobile.Common;
using MyDiary.Mobile.Http;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MyDiary.Mobile.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.DataContext = new LoginPageViewModel();
            this.ViewModel.ResponseFromServer += HandleServerResponse;
            Loaded += (s, e) =>
            {
                CheckIfLoggedOrOffline();
            };
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


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack && rootFrame.SourcePageType != typeof(LoginPage))
            {
                rootFrame.GoBack();
                e.Handled = true;
            }
        }

        private async void CheckIfLoggedOrOffline()
        {
            if (LocalSettingsManager.Instance.IsOffline())
            {
                this.Frame.Navigate(typeof(ReminderPage));
                return;
            }

            if(this.ViewModel.IsLogged)
            {
                var isConnected = await MyDiaryHttpRequester.Instance.IsConnected();
                if(isConnected)
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    MessageDialogManager.ShowConfirmationDialog(StringResources.TitleNoConnection, StringResources.ContentNoConnection);
                }
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HelpPage));
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
            this.Frame.Navigate(typeof(RegisterPage));
        }

        private void HandleServerResponse(bool problemWithConnection, bool successful, string message)
        {
            if (!problemWithConnection)
            {
                if (successful)
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    MessageDialogManager.ShowConfirmationDialog(StringResources.TitleProblemWithLogin, message);
                }
            }
            else
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleNoConnection, StringResources.ContentNoConnection);
            }
        }

        private void OfflineButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDialogManager.ShowDialog(StringResources.TitleGoOffline, StringResources.ContentGoOffline, GoOffline);
        }

        private void GoOffline(IUICommand command)
        {
            LocalSettingsManager.Instance.SetOfflineMode(true);
            this.Frame.Navigate(typeof(ReminderPage));
        }
    }
}