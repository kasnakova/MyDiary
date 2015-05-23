namespace MyDiary.Mobile.Common
{
    using MyDiary.Mobile.ViewModels;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using Windows.Storage;

    public class LocalSettingsManager
    {
        private static LocalSettingsManager instance;
        private ApplicationDataContainer localSettings;

        private LocalSettingsManager()
        {
            this.localSettings = ApplicationData.Current.LocalSettings;
        }

        public static LocalSettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LocalSettingsManager();
                }

                return instance;
            }
        }

        public void SaveAccessToken(string accessToken)
        {
            this.localSettings.Values[Constants.LocalSettingsKeyAccessToken] = accessToken;
        }

        public string GetAccessToken()
        {
            //for the emulator not saving the settings after 'restart' - http://stackoverflow.com/questions/25473254/test-app-settings-in-windows-phone-8-1-emulator
            var accessToken = string.Empty;
            object token = localSettings.Values[Constants.LocalSettingsKeyAccessToken];
            if (token != null)
            {
                accessToken = token.ToString();
            }

            return accessToken;
        }

        public void RemoveAccessToken()
        {
            this.localSettings.Values[Constants.LocalSettingsKeyAccessToken] = null;
        }

        public void AddReminder(string guid, string text, DateTime date)
        {
            var reminder = new ReminderViewModel
                {
                    Id = guid,
                    Text = text,
                    Date = date
                };

            var currReminders = this.GetAllReminders();
            currReminders.Add(reminder);
            string output = JsonConvert.SerializeObject(currReminders);
            this.localSettings.Values[Constants.LocalSettingsKeyReminders] = output;
        }

        public void RemoveReminder(string guid)
        {
            var currReminders = this.GetAllReminders();
            for (int i = 0; i < currReminders.Count; i++)
            {
                var reminder = currReminders[i];
                if(reminder.Id == guid)
                {
                    currReminders.Remove(reminder);
                }
            }

            string output = JsonConvert.SerializeObject(currReminders);
            this.localSettings.Values[Constants.LocalSettingsKeyReminders] = output;
        }

        public void RemoveAllReminders()
        {
            this.localSettings.Values[Constants.LocalSettingsKeyReminders] = null;
        }

        public List<ReminderViewModel> GetAllReminders()
        {
            object reminders = localSettings.Values[Constants.LocalSettingsKeyReminders];
            List<ReminderViewModel> deserializedProduct = new List<ReminderViewModel>();
            if (reminders != null)
            {
                deserializedProduct = JsonConvert.DeserializeObject<List<ReminderViewModel>>(reminders.ToString());
            }

            return deserializedProduct;
        }

        public void SetOfflineMode(bool isOffline)
        {
            this.localSettings.Values[Constants.LocalSettingsKeyIsOffline] = isOffline;
        }

        public bool IsOffline()
        {
            var isOffline = false;
            object offline = localSettings.Values[Constants.LocalSettingsKeyIsOffline];
            if (offline != null)
            {
                isOffline = bool.Parse(offline.ToString());
            }

            return isOffline;
        }
    }
}
