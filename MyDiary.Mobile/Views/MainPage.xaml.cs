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
using Windows.Phone.UI.Input;
using System.Threading.Tasks;
using MyDiary.Mobile.Common;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace MyDiary.Mobile.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = new MainPageViewModel();
            //this.ViewModel.ProblemWithConnection += RedirectToLogin;
        }

        public MainPageViewModel ViewModel
        {
            get
            {
                return this.DataContext as MainPageViewModel;
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
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void Calendar_DisplayDateChanged(object sender, WinRTXamlToolkit.Controls.CalendarDateChangedEventArgs e)
        {
            var b = 9;
        }

        private void RemindersButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReminderPage));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(UserDetailsPage));
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HelpPage));
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if(!LocalSettingsManager.Instance.IsOffline())
            {
                await this.ViewModel.LogoutAsync();
            }
            else
            {
                LocalSettingsManager.Instance.SetOfflineMode(false);
            }

            this.Frame.Navigate(typeof(LoginPage));
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNoteText != null)
            {
                TextBoxNoteText.Text = string.Empty;
            }
        }

        //Because you can't access names of elements inside the hub
        private TextBox TextBoxNoteText { get; set; }

        private void TextBoxNoteText_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxNoteText = sender as TextBox;
        }

        private void ListView_Holding(object sender, HoldingRoutedEventArgs e)
        {
             FrameworkElement element = (FrameworkElement)e.OriginalSource;
             if (element.DataContext != null && element.DataContext is NoteViewModel && e.HoldingState == Windows.UI.Input.HoldingState.Started)
            {
                NoteViewModel selectedOne = (NoteViewModel)element.DataContext;
                var id = selectedOne.Id;
                this.ViewModel.CalendarViewModel.DeleteNote(id);
                e.Handled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
