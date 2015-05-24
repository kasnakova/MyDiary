namespace MyDiary.Desktop.ViewModels
{
    using System;

    public class ReminderViewModel : ViewModelBase
    {
        private string text;
        private DateTime date;

        public string Id { get; set; }

        public DateTime Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = value;
                this.OnPropertyChanged("Date");
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                this.OnPropertyChanged("NoteText");
            }
        }
    }
}
