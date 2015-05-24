namespace MyDiary.Desktop.ViewModels
{
    using System.Windows.Input;

    using MyDiary.Desktop.Common;
    using MyDiary.Desktop.Http;
    using System.Threading.Tasks;
    //using Windows.Media.SpeechRecognition;
    using System;

    public class MainPageViewModel : ViewModelBase
    {
        private string title;
        private bool isOffline;
        public RecordViewModel RecordViewModel { get; set; }
        public CalendarViewModel CalendarViewModel { get; set; }
        //public delegate void EventHandler();
        //public event EventHandler ProblemWithConnection;

        public MainPageViewModel()
        {
            this.GetNameAsync();
            this.Title = StringResources.OfflineDiary;
            //this.IsOffline = LocalSettingsManager.Instance.IsOffline();
           this.RecordViewModel = new RecordViewModel();
            this.CalendarViewModel = new CalendarViewModel();
            this.RecordViewModel.NoteSaved += this.CalendarViewModel.OnNoteSaved;
            FileStore.Test();
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                this.OnPropertyChanged("Title");
            }
        }

        public bool IsOffline
        {
            get
            {
                return this.isOffline;
            }
            set
            {
                this.isOffline = value;
                this.OnPropertyChanged("IsOffline");
            }
        }

        public async Task LogoutAsync()
        {
            string token = LocalSettingsManager.Instance.GetAccessToken();
            LocalSettingsManager.Instance.RemoveAccessToken();
            var result = await MyDiaryHttpRequester.Instance.LogoutAsync(token);
        }

        private async void GetNameAsync()
        {
            //if (LocalSettingsManager.Instance.IsOffline())
            //{
            //    this.Title = StringResources.OfflineDiary;
            //    return;
            //}

            string token = LocalSettingsManager.Instance.GetAccessToken();
            var result = await MyDiaryHttpRequester.Instance.GetNameAsync(token);
            var isSuccessful = result.Item2;
            if (isSuccessful)
            {
                var name = result.Item3.Substring(1, result.Item3.Length - 2);
                this.Title = name + "'s " + StringResources.Diary;
            }
        }
    }
}
