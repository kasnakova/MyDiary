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
using MyDiary.Desktop.Common;
namespace MyDiary.Desktop
{
    /// <summary>
    /// Interaction logic for SetPasswordWindow.xaml
    /// </summary>
    public partial class SetPasswordWindow : Window
    {
        public SetPasswordWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TextBoxPassword.Password) || string.IsNullOrEmpty(this.TextBoxConfirmPassword.Password))
            {
                this.ErrorMessage.Text = StringResources.ContentInvalidPassword;
                this.ErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            if (this.TextBoxPassword.Password.Length < Constants.MinPasswordLength)
            {
                this.ErrorMessage.Text = string.Format(StringResources.ContentInvalidPasswordLength, Constants.MinPasswordLength);
                this.ErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            if (this.TextBoxPassword.Password != this.TextBoxConfirmPassword.Password)
            {
                this.ErrorMessage.Text = StringResources.ContentPasswordMismatch;
                this.ErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            this.Close();
        }
    }
}
