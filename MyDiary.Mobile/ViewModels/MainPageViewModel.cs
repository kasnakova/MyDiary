namespace MyDiary.Mobile.ViewModels
{
    using System.Windows.Input;

    using MyDiary.Mobile.Common;
    using MyDiary.Mobile.Http;
    using System.Threading.Tasks;
    using Windows.Media.SpeechRecognition;
    using System;

    public class MainPageViewModel : ViewModelBase
    {
        private string hubHeader;
        private bool isOffline;
        public RecordViewModel RecordViewModel { get; set; }
        public CalendarViewModel CalendarViewModel { get; set; }
        //public delegate void EventHandler();
        //public event EventHandler ProblemWithConnection;

        public MainPageViewModel()
        {
            this.GetNameAsync();
            this.IsOffline = LocalSettingsManager.Instance.IsOffline();
            this.HubHeader = StringResources.OfflineDiary;
            this.RecordViewModel = new RecordViewModel();
            this.CalendarViewModel = new CalendarViewModel();
            this.CalendarViewModel.SelectedDateChanged += this.RecordViewModel.SelectedDateChanged;
            this.RecordViewModel.NoteSaved += this.CalendarViewModel.OnNoteSaved;
        }

        public string HubHeader
        {
            get
            {
                return this.hubHeader;
            }
            set
            {
                this.hubHeader = value;
                this.OnPropertyChanged("HubHeader");
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
            var result = await MyDiaryHttpRequester.Instance.LogoutAsync(token);
            LocalSettingsManager.Instance.RemoveAccessToken();
        }

        private async void GetNameAsync()
        {
            if (LocalSettingsManager.Instance.IsOffline())
            {
                this.HubHeader = StringResources.OfflineDiary;
                return;
            }

            string token = LocalSettingsManager.Instance.GetAccessToken();
            var result = await MyDiaryHttpRequester.Instance.GetNameAsync(token);
            var isSuccessful = result.Item2;
            if(isSuccessful)
            {
                var name = result.Item3.Substring(1, result.Item3.Length - 2);
                this.HubHeader = name + "'s " + StringResources.Diary;
            }
        }
    }
}
