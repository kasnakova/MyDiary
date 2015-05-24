namespace MyDiary.Desktop.Http
{
    using System;
    using System.Threading.Tasks;
    using System.Threading;

    using Newtonsoft.Json.Linq;

    using MyDiary.Desktop.Http;
    using MyDiary.Desktop.Common;

    public class MyDiaryHttpRequester
    {
        private static MyDiaryHttpRequester instance;

        private MyDiaryHttpRequester()
        {
        }

        public static MyDiaryHttpRequester Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MyDiaryHttpRequester();
                }

                return instance;
            }
        }

        public async Task<Tuple<bool, bool, string>> LoginAsync(string email, string password)
        {
            var httpRequester = new HttpRequester();
            var body = string.Format(Constants.FormatLoginBody, email, password);
            return await httpRequester.SendRequestAsync(Constants.UrlLogin, Method.Post, body, null);
        }

        public async Task<Tuple<bool, bool, string>> RegisterAsync(string email, string name, string password)
        {
            var httpRequester = new HttpRequester();
            var body = string.Format(Constants.FormatRegisterBody, email, password, password, name);
            return await httpRequester.SendRequestAsync(Constants.UrlRegister, Method.Post, body, null);
        }

        public async Task<Tuple<bool, bool, string>> LogoutAsync(string token)
        {
            var httpRequester = new HttpRequester();
            return await httpRequester.SendRequestAsync(Constants.UrlLogout, Method.Post, "", token);
        }

        public async Task<Tuple<bool, bool, string>> GetNameAsync(string token)
        {
            var httpRequester = new HttpRequester();
            return await httpRequester.SendRequestAsync(Constants.UrlName, Method.Get, null, token);
        }

        public async Task<Tuple<bool, bool, string>> SaveNoteAsync(string noteText, string password, DateTime date, string token)
        {
            var httpRequester = new HttpRequester();
            var body = string.Format(Constants.FormatSaveNote, noteText, date);
            if (!string.IsNullOrEmpty(password))
            {
                body = body + "&Password=" + password;
            }

            return await httpRequester.SendRequestAsync(Constants.UrlSaveNote, Method.Post, body, token);
        }

        public async Task<Tuple<bool, bool, string>> GetNotesForDateAsync(DateTime date, string token)
        {
            var httpRequester = new HttpRequester();
            var strDate = date.ToString();
            var url = Constants.UrlGetNotesForDate + string.Format(Constants.FormatGetNotesForDate, strDate);
            return await httpRequester.SendRequestAsync(url, Method.Get, null, token);
        }

        public async Task<Tuple<bool, bool, string>> DeleteNoteAsync(int id, string token)
        {
            var httpRequester = new HttpRequester();
            var url = Constants.UrlDeleteNote + string.Format(Constants.FormatDeleteNote, id);
            return await httpRequester.SendRequestAsync(url, Method.Delete, null, token);
        }

        public async Task<Tuple<bool, bool, string>> GetDecryptedNoteTextAsync(int id, string password, string token)
        {
            var httpRequester = new HttpRequester();
            var url = Constants.UrlGetDecryptedNoteText + string.Format(Constants.FormatGetDecryptedNoteText, id, password);
            return await httpRequester.SendRequestAsync(url, Method.Get, null, token);
        }

        public async Task<bool> IsConnected()
        {
            var httpRequester = new HttpRequester();
            var result = await httpRequester.SendRequestAsync(Constants.UrlName, Method.Get, null, string.Empty);
            return !result.Item1;
        }
    }
}
