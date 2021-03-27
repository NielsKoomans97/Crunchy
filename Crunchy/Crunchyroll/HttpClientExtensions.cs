using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Crunchy.Crunchyroll
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetAsync<T>(this HttpClient httpClient, string host, string path, params (string Key, object Value)[] parameters)

        {
            var uri = CreateUri(host, path, parameters);
            var content = await httpClient.GetStringAsync(uri);

            if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
                throw new NullReferenceException("No textual content was found");

            return JsonConvert.DeserializeObject<T>(content);
        }

        public static async Task<T> PostAsync<T>(this HttpClient httpClient, object content, string host, string path, params (string Key, object Value)[] parameters)
        {
            var uri = CreateUri(host, path, parameters);
            var scontent = new StringContent(JsonConvert.SerializeObject(content));
            var message = await httpClient.PostAsync(uri, scontent);
            var rcontent = message.EnsureSuccessStatusCode().Content;
            var text = await rcontent.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(text);
        }

        private static string CreateUri(string host, string path, (string Key, object Value)[] parameters)
        {
            var builder = new StringBuilder($"{host}/{path}.0.json");

            if (parameters.Length >= 0)
            {
                var param = parameters[0];
                builder.Append($"?{param.Key}={param.Value}");

                for (int i = 1; i < parameters.Length; i++)
                {
                    param = parameters[i];
                    builder.Append($"&{param.Key}={param.Value}");
                }
            }

            return builder.ToString();
        }
    }
}