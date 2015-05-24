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
using System.IO;

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
            this.isHelpOpen = false;
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

        private bool isHelpOpen;
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isHelpOpen)
            {
                this.HelpText.Visibility = Visibility.Visible;
                this.isHelpOpen = true;
            }
            else
            {
                this.HelpText.Visibility = Visibility.Hidden;
                this.isHelpOpen = false;
            }
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

        private void ExportAllButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "My diary notes - " + this.Calendar.SelectedDate.Value.Date.ToString("dd.MM.yyyy"); // Default file name
            dlg.DefaultExt = ".docs"; 
            dlg.Filter = "Word document (.docx)|*.docx|PDF document (.pdf)|*.pdf|Plain text document (.txt)|*.txt|Web page document (.html)|*.html"; // Filter files by extension
            dlg.ValidateNames = true;
            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                if(this.ViewModel.CalendarViewModel.SaveNotesForDayToFile(filename))
                {
                    var msgWin = new ConfirmationBoxWindow("Your notes were successfully saved to file '" + System.IO.Path.GetFileName(filename) + "'", @"/Assets/tick.ico");
                    msgWin.ShowDialog();
                }
                else
                {
                    var msgWin = new ConfirmationBoxWindow("Sorry but we couldn't save your notes to file.", @"/Assets/clear.png");
                    msgWin.ShowDialog();
                }
            }
        }
    }
}
