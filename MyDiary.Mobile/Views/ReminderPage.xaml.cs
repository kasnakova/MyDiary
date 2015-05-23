using MyDiary.Mobile.ViewModels;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MyDiary.Mobile.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReminderPage : Page
    {
        public ReminderPage()
        {
            this.InitializeComponent();
            this.DataContext = new RemindersViewModel();
        }

        public RemindersViewModel ViewModel
        {
            get
            {
                return this.DataContext as RemindersViewModel;
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

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HelpPage));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CancelAllButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ListView_Holding(object sender, HoldingRoutedEventArgs e)
        {
             FrameworkElement element = (FrameworkElement)e.OriginalSource;
             if (element.DataContext != null && element.DataContext is ReminderViewModel && e.HoldingState == Windows.UI.Input.HoldingState.Started)
            {
                ReminderViewModel selectedOne = (ReminderViewModel)element.DataContext;
                var id = selectedOne.Id;
                this.ViewModel.DeleteReminder(id);
                e.Handled = true;
            }
        }
    }
}
