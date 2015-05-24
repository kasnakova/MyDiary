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

using MyDiary.Desktop.Http;
using MyDiary.Desktop.Common;
using MyDiary.Desktop.ViewModels;
using System.Threading;

namespace MyDiary.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.DataContext = new MainPageViewModel();
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

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (true)//!LocalSettingsManager.Instance.IsOffline())
            {
                await this.ViewModel.LogoutAsync();
            }
            else
            {
               // LocalSettingsManager.Instance.SetOfflineMode(false);
            }

            Window loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void RemindersButton_Click(object sender, RoutedEventArgs e)
        {
           // this.Frame.Navigate(typeof(ReminderPage));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(UserDetailsPage));
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
           // this.Frame.Navigate(typeof(HelpPage));
        }

        private void ListView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.OriginalSource;
            if (element.DataContext != null && element.DataContext is NoteViewModel)
            {
                NoteViewModel selectedOne = (NoteViewModel)element.DataContext;
                var id = selectedOne.Id;
                this.ViewModel.CalendarViewModel.DeleteNote(id);
                Window msgBoxWindow = new MessageBoxWindow(StringResources.TitleDeleteNote, StringResources.ContentDeleteNote);
                msgBoxWindow.DataContext = this.ViewModel.CalendarViewModel;
                msgBoxWindow.ShowDialog();
                e.Handled = true;
            }
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = DateTime.Parse(sender.ToString());
            this.ViewModel.CalendarViewModel.OnDateChanged(date);
            this.ViewModel.RecordViewModel.SelectedDate = date;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.TextBoxNoteText.Text = string.Empty;
            this.RecordInfoText.Text = string.Empty;
            this.ViewModel.RecordViewModel.Password = string.Empty;
            this.ViewModel.RecordViewModel.NoteText = string.Empty;
        }

        private void Calendar_Loaded(object sender, RoutedEventArgs e)
        {
            this.Calendar.SelectedDate = DateTime.Now;
        }

        private void ListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.OriginalSource;
            if (element.DataContext != null && element.DataContext is NoteViewModel)
            {
                NoteViewModel selectedOne = (NoteViewModel)element.DataContext;
                if (selectedOne.HasPassword)
                {
                    this.ViewModel.CalendarViewModel.UnlockNote(selectedOne);
                    Window msgBoxWindow = new UnlockNoteWindow();
                    msgBoxWindow.DataContext = this.ViewModel.CalendarViewModel;
                    msgBoxWindow.ShowDialog();
                }
                e.Handled = true;
            }
        }

        private void SetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.ViewModel.RecordViewModel.SetPassword())
            {
                Window msgBoxWindow = new SetPasswordWindow();
                msgBoxWindow.DataContext = this.ViewModel.RecordViewModel;
                msgBoxWindow.ShowDialog();
            }
        }

        //private void ListView_Holding(object sender, HoldingRoutedEventArgs e)
        //{
        //    FrameworkElement element = (FrameworkElement)e.OriginalSource;
        //    if (element.DataContext != null && element.DataContext is NoteViewModel && e.HoldingState == Windows.UI.Input.HoldingState.Started)
        //    {
        //        NoteViewModel selectedOne = (NoteViewModel)element.DataContext;
        //        var id = selectedOne.Id;
        //        this.ViewModel.CalendarViewModel.DeleteNote(id);
        //        e.Handled = true;
        //    }
        //}
    }
}
