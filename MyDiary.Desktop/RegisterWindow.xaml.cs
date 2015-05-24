using MyDiary.Desktop.Common;
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
namespace MyDiary.Desktop
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
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
                    this.SuccessText.Visibility = Visibility.Visible;
                    this.LoginButton.Visibility = Visibility.Visible;
                   // this.InfoText.Text = StringResources.ContentSuccessfulRegistering;
                   // MessageDialogManager.ShowConfirmationDialog(StringResources.TitleSuccessfulRegistering, StringResources.ContentSuccessfulRegistering);
                    //Window loginWindow = new LoginWindow();
                    //loginWindow.Show();
                    //this.Close();
                }
                else
                {
                    this.InfoText.Text = message;
                   // MessageDialogManager.ShowConfirmationDialog(StringResources.TitleProblemWithRegister, message);
                }
            }
            else
            {
                this.InfoText.Text = StringResources.ContentNoConnection;
               // MessageDialogManager.ShowConfirmationDialog(StringResources.TitleNoConnection, StringResources.ContentNoConnection);
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
