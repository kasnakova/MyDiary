using System;
namespace MyDiary.Desktop.ViewModels
{
    public class NoteViewModel : ViewModelBase
    {
        private DateTime date;
        private string time;
        private string noteText;
        private bool hasPassword;
        public int Id { get; set; }


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

        public string Time
        {
            get 
            {
                return this.date.ToString("HH:mm:ss");
            }
        }

        public string NoteText
        {
            get
            {
                return this.noteText;
            }
            set
            {
                this.noteText = value;
                this.OnPropertyChanged("NoteText");
            }
        }

        public bool HasPassword
        {
            get
            {
                return this.hasPassword;
            }
            set
            {
                this.hasPassword = value;
                this.OnPropertyChanged("HasPassword");
            }
        }
    }
}
