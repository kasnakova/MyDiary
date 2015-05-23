using MyDiary.Mobile.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyDiary.Mobile.ViewModels
{
    public class AddReminderViewModel : ViewModelBase
    {
        private string reminderText;
        private DateTime selectedDate;
        private DateTime selectedTime;
        private ICommand doneCommand;
        private ICommand cancelCommand;
        public delegate void EventHandler(string text, DateTime date);
        public event EventHandler OnPopupClosed;

        public AddReminderViewModel()
        {
            this.ReminderText = string.Empty;
            this.SelectedDate = DateTime.Now.Date;
            this.SelectedTime = DateTime.Now;
        }

        public DateTime Date 
        { 
            get 
            {
                return new DateTime(this.SelectedDate.Year, this.SelectedDate.Month, this.SelectedDate.Day, this.SelectedTime.Hour, this.SelectedTime.Minute, this.SelectedTime.Second);
            } 
        }

        public string ReminderText
        {
            get
            {
                return this.reminderText;
            }
            set
            {
                this.reminderText = value;
                this.OnPropertyChanged("ReminderText");
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return this.selectedDate;
            }
            set
            {
                this.selectedDate = value;
                this.OnPropertyChanged("SelectedDate");
            }
        }

        public DateTime SelectedTime
        {
            get
            {
                return this.selectedTime;
            }
            set
            {
                this.selectedTime = value;
                this.OnPropertyChanged("SelectedTime");
            }
        }

        public ICommand DoneCommand
        {
            get
            {
                if (this.doneCommand == null)
                {
                    this.doneCommand = new DelegateCommand(execute: this.AddReminder);
                }

                return this.doneCommand;
            }
            set
            {
                this.doneCommand = value;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if (this.cancelCommand == null)
                {
                    this.cancelCommand = new DelegateCommand(execute: this.Cancel);
                }

                return this.cancelCommand;
            }
            set
            {
                this.cancelCommand = value;
            }
        }

        private void AddReminder()
        {
            if (this.IsValid())
            {
                EventHandler handler = OnPopupClosed;
                if (handler != null)
                {
                    handler(this.ReminderText, this.Date);
                }
            }

            this.ReminderText = string.Empty;
        }

        private void Cancel()
        {
            EventHandler handler = OnPopupClosed;
            if (handler != null)
            {
                handler(null, DateTime.Now);
            }

            this.ReminderText = string.Empty;
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(this.ReminderText))
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidReminder);
                return false;
            }

            if(this.Date < DateTime.Now)
            {
                MessageDialogManager.ShowConfirmationDialog(StringResources.TitleInvalidInput, StringResources.ContentInvalidDateForReminder);
                return false;
            }

            return true;
        }
    }
}
