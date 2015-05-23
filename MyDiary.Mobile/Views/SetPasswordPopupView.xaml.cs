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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MyDiary.Mobile.Views
{
    public sealed partial class SetPasswordPopupView : UserControl
    {
        public SetPasswordPopupView()
        {
            this.InitializeComponent();
        //    this.DataContext = new SetPasswordViewModel();
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            Password.Password = string.Empty;
        }

        private void ConfirmPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            ConfirmPassword.Password = string.Empty;
        }
    }
}
