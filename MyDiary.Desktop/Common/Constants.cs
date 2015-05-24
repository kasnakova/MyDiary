namespace MyDiary.Desktop.Common
{
    public class Constants
    {
        public const int ConnectionTimeout = 10000; //10 seconds
        public const int MinPasswordLength = 6;
        public const string LocalSettingsKeyAccessToken = "AccessToken";
        public const string LocalSettingsKeyReminders = "Reminders";
        public const string LocalSettingsKeyIsOffline = "IsOffline";

        #region Http constants
        public const string UrlBase = "http://localhost:50264/";
        public const string UrlLogin = UrlBase + "Token";
        public const string UrlRegister = UrlBase + "api/Account/Register";
        public const string UrlName = UrlBase + "api/Account/UserName";
        public const string UrlLogout = UrlBase + "api/Account/Logout";
        public const string UrlSaveNote = UrlBase + "api/Notes/SaveNote";
        public const string UrlGetNotesForDate = UrlBase + "api/Notes/GetNotes";
        public const string UrlDeleteNote = UrlBase + "api/Notes/DeleteNote";
        public const string UrlGetDecryptedNoteText = UrlBase + "api/Notes/GetDecryptedNoteText";

        public const string FormatLoginBody = "grant_type=password&username={0}&password={1}";
        public const string FormatRegisterBody = "Email={0}&Password={1}&ConfirmPassword={2}&Name={3}";
        public const string FormatSaveNote = "NoteText={0}&Date={1}";
        public const string FormatDeleteNote = "?id={0}";
        public const string FormatGetNotesForDate = "?date={0}";
        public const string FormatGetDecryptedNoteText = "?id={0}&password={1}";
        public const string FormatAuthorization = "Bearer {0}";

        public const string MediaType = "application/x-www-form-urlencoded";
        public const string Authorization = "Authorization";

        public const string JsonKeyError = "error";
        public const string JsonKeyModelState = "ModelState";
        public const string JsonKeyErrors = "Errors";
        public const string JsonKeyMessage = "Message";
        public const string JsonKeyErrorDescription = "error_description";
        public const string JsonKeyToken = "access_token";
        #endregion
    }
}
