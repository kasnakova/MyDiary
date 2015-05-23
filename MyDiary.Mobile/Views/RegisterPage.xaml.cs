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

using MyDiary.Mobile.ViewModels;
using MyDiary.Mobile.Common;
using Windows.UI.ViewManagement;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MyDiary.Mobile.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
            this.DataContext = new RegisterPageViewModel();
            this.ViewModel.ResponseFromServer += HandleServerResponse;
        }

        public RegisterPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as RegisterPageViewModel;
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

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HelpPage));
        }

        private void TextBoxPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxPassword.Password = string.Empty;
        }

        private void TextBoxConfirmPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxConfirmPassword.Password = string.Empty;
        }

        private void TextBoxEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxEmail.Text = string.Empty;
        }

        private void TextBoxName_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxName.Text = string.Empty;
        }

        private void HandleServerResponse(bool problemWithConnection, bool successful, string message)
        {
            if (!problemWithConnection)
            {
                if (successful)
                {
                    MessageDialog msgDialog = new MessageDialog(StringResources.ContentSuccessfulRegistering, StringResources.TitleSuccessfulRegistering);
                    msgDialog.Commands.Add(new UICommand(StringResources.Ok));
                    msgDialog.ShowAsync();
                    this.Frame.Navigate(typeof(LoginPage));
                }
                else
                {
                    MessageDialogManager.ShowConfirmationDialog(StringResources.TitleProblemWithRegister, message);
                }
            }
            else
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleNoConnection, StringResources.ContentNoConnection);
            }
        }
    }
}
