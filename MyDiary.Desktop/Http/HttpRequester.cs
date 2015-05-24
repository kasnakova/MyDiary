using System;
using System.Threading.Tasks;

using MyDiary.Desktop.Common;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace MyDiary.Desktop.Http
{
    public class HttpRequester
    {
        private HttpClient client;

        public HttpRequester()
        {
            this.client = new HttpClient();
            this.client.Timeout = TimeSpan.FromMilliseconds(Constants.ConnectionTimeout);
           
        }

        //TODO return it to three
        public async Task<Tuple<bool, bool, string>> SendRequestAsync(string url, Method method, string body, string token)
        {
            var uri = new Uri(url);
            if (!string.IsNullOrEmpty(token))
            {
                var headers = this.client.DefaultRequestHeaders;
                headers.Add(Constants.Authorization, string.Format(Constants.FormatAuthorization, token));
            }

            HttpResponseMessage aResponse = null;
            var problemWithConnection = false;
            var isSuccessStatusCode = false;
            String response = String.Empty;
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource(Constants.ConnectionTimeout);
                switch (method)
                {
                    case Method.Get:
                        aResponse = await this.client.GetAsync(uri);
                        break;
                    case Method.Post:
                        HttpContent content = new StringContent(body);
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        //StringContent theContent = new StringContent(body);
                        aResponse = await this.client.PostAsync(uri, content);
                        break;
                    case Method.Delete:
                        aResponse = await this.client.DeleteAsync(uri);
                        break;
                    default:
                        break;
                }

                isSuccessStatusCode = aResponse.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                problemWithConnection = true;
                response = ex.Message;
            }

            if (aResponse != null && !problemWithConnection)
            {
                var pageContent = await aResponse.Content.ReadAsStringAsync();
                response = pageContent;
            }

            Tuple<bool, bool, string> result = new Tuple<bool, bool, string>(problemWithConnection, isSuccessStatusCode, response);
            return result;
        }
    }
}
