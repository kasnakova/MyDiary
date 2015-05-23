using MyDiary.Mobile.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyDiary.Mobile.Common;
using Windows.UI.Popups;

namespace MyDiary.Mobile.ViewModels
{
    public class RemindersViewModel : ViewModelBase
    {
        private bool addingReminder;
        private bool hasRemidners;
        private string remidnerToDeleteId;
        private ObservableCollection<ReminderViewModel> reminders;
        private ReminderManager reminderManager;
        private ICommand addReminderCommand;
        private ICommand deleteAllCommand;
        public AddReminderViewModel AddReminderViewModel { get; set; }

        public RemindersViewModel()
        {
            this.AddingReminder = false;
            this.HasReminders = false;
            this.Reminders = new List<ReminderViewModel>();
            this.PopulateRemindersList();
            this.AddReminderViewModel = new AddReminderViewModel();
            this.AddReminderViewModel.OnPopupClosed += MakeReminder;
            this.reminderManager = new ReminderManager();
        }

        public IEnumerable<ReminderViewModel> Reminders
        {
            get
            {
                return this.reminders;
            }
            set
            {
                if (this.reminders == null)
                {
                    this.reminders = new ObservableCollection<ReminderViewModel>();
                }

                this.reminders.Clear();
                this.reminders.AddRange<ReminderViewModel>(value);
            }
        }

        public bool AddingReminder
        {
            get
            {
                return this.addingReminder;
            }
            set
            {
                this.addingReminder = value;
                this.OnPropertyChanged("AddingReminder");
            }
        }

        public bool HasReminders
        {
            get
            {
                return this.hasRemidners;
            }
            set
            {
                this.hasRemidners = value;
                this.OnPropertyChanged("HasReminders");
            }
        }

        public ICommand DeleteAllCommand
        {
            get
            {
                if (this.deleteAllCommand == null)
                {
                    this.deleteAllCommand = new DelegateCommand(execute: this.DeleteAll);
                }

                return this.deleteAllCommand;
            }
            set
            {
                this.deleteAllCommand = value;
            }
        }

        public ICommand AddReminderCommand
        {
            get
            {
                if (this.addReminderCommand == null)
                {
                    this.AddReminderCommand = new DelegateCommand(execute: this.AddReminder);
                }

                return this.addReminderCommand;
            }
            set
            {
                this.addReminderCommand = value;
            }
        }

        public void DeleteReminder(string id)
        {
            this.remidnerToDeleteId = id;
            MessageDialogManager.ShowDialog(StringResources.TitleDeleteReminder, StringResources.ContentDeleteReminder, this.DeleteOneReminder);
        }

        private void DeleteAll()
        {
            MessageDialogManager.ShowDialog(StringResources.TitleDeleteAllReminders, StringResources.ContentDeleteAllReminders, this.DeleteAllReminders);
        }

        private void DeleteOneReminder(IUICommand command)
        {
            reminderManager.RemoveReminder(this.remidnerToDeleteId);
            this.PopulateRemindersList();
        }

        private void DeleteAllReminders(IUICommand command)
        {
            reminderManager.RemoveAllReminders();
            this.reminders.Clear();
            this.HasReminders = false;
        }

        private void AddReminder()
        {
            this.AddingReminder = true;
        }

        private void MakeReminder(string text, DateTime date)
        {
            this.AddingReminder = false;
            if(!string.IsNullOrEmpty(text))
            {
                this.reminderManager.AddReminder(text, date);
                this.PopulateRemindersList();
            }
        }

        private void PopulateRemindersList()
        {
            this.Reminders = LocalSettingsManager.Instance.GetAllReminders();
            if(this.reminders.Count == 0)
            {
                this.HasReminders = false;
            }
            else
            {
                this.HasReminders = true;
            }
        }
    }
}
