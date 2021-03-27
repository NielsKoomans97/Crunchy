using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crunchy
{
    public partial class Form1 : Form
    {
        private Crunchyroll Crunchyroll;
        private dynamic CurrentData;

        public Form1()
        {
            InitializeComponent();

            Crunchyroll = new Crunchyroll();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await Crunchyroll.InitializeAsync();
        }

        private void PopulateListview<T>(string[] Columns, T[] Items)
        {
            listView1.Clear();

            foreach (var column in Columns)
                listView1.Columns.Add(column, (int)(column.Length + 180.50));

            foreach (var item in Items)
            {
                var type = item.GetType();
                var properties = type.GetProperties();
                var lvi = new ListViewItem();

                var property = properties.First(a => Columns[0].Contains(a.Name)).GetValue(item).ToString();
                lvi.Text = property;

                for (int i = 1; i < Columns.Length; i++)
                {
                    lvi.SubItems.Add(properties.First(a => Columns[i].Contains(a.Name)).GetValue(item).ToString());
                }

                listView1.Items.Add(lvi);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            CurrentData = await Crunchyroll.SearchAsync(textBox2.Text);
            PopulateListview(new[] { "Name", "Id" }, CurrentData);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count < 0)
                return;

            try
            {
                textBox2.Text = listView1.Items
                    .Cast<ListViewItem>()
                    .First(a => a.Selected == true)
                    .SubItems[1].Text;
            }
            catch
            {
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            CurrentData = await Crunchyroll.GetStreamInfoAsync(Convert.ToInt64(textBox2.Text));
            PopulateListview(new[] { "Quality", "Url" }, CurrentData.Streams);
        }

        private async void button7_Click(object sender, EventArgs e)
        {
            await Crunchyroll.LoginAsync("niels.koomans@gmail.com", "N!fi19g7");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (CurrentData is SeasonInfo)
            {
            }

            if (CurrentData is SeriesInfo)
            {
            }
        }
    }

    public class Crunchyroll : IDisposable
    {
        public TokenInfo Tokens { get; private set; }

        private HttpClient DataClient;

        public Crunchyroll()
        {
            DataClient = new HttpClient();
        }

        internal Crunchyroll(string sessionId, DateTime expires)
        {
            Tokens.SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
            Tokens.SessionExpires = expires;
        }

        public async Task InitializeAsync()
        {
            var session = await DataClient.GetAsync<dynamic>("https://api.crunchyroll.com",
                "start_session",
                ("device_id", "00000000-4d19-2528-ffff-ffff99d603a9"),
                ("device_type", "com.crunchyroll.windows.desktop"),
                ("access_token", "LNDJgOit5yaRIWN"));

            Tokens = new TokenInfo((string)session.data.session_id, DateTime.Now.AddHours(4), string.Empty, DateTime.Now, default(string));
        }

        #region GetEpisodes

        public async Task<EpisodeInfo[]> ListEpisodesAsync(long series_id = 0, long collection_id = 0)
        {
            if (series_id == 0 && collection_id == 0)
                throw new Exception("Both parameters were empty. Please give a valid id number to either one of the parameters");

            var data = await DataClient.GetAsync<dynamic>("https://api.crunchyroll.com", "list_media",
                series_id != 0 ? ("series_id", series_id) : ("collection_id", collection_id),
                ("session_id", Tokens.SessionId),
            ("auth_token", Tokens.AuthToken));

            var dataItem = (JArray)data.data;

            return dataItem.ToObject<EpisodeInfo[]>();
        }

        public async Task<EpisodeInfo[]> ListEpisodesAsync(SeriesInfo seriesInfo)
            => await ListEpisodesAsync(series_id: seriesInfo.Id);

        public async Task<EpisodeInfo[]> ListEpisodesAsync(SeasonInfo seasonInfo)
            => await ListEpisodesAsync(collection_id: seasonInfo.Id);

        #endregion GetEpisodes

        #region GetSeasons

        public async Task<SeasonInfo[]> ListSeasonsAsync(long series_id)
        {
            if (series_id == 0)
                throw new Exception("No valid id number was given");

            var data = await DataClient.GetAsync<dynamic>("https://api.crunchyroll.com", "list_collections",
                ("series_id", series_id),
                ("session_id", Tokens.SessionId),
            ("auth_token", Tokens.AuthToken));

            var dataItem = (JArray)data.data;

            return dataItem.ToObject<SeasonInfo[]>();
        }

        public async Task<SeasonInfo[]> ListSeasonsAsync(SeriesInfo seriesInfo)
            => await ListSeasonsAsync(seriesInfo.Id);

        #endregion GetSeasons

        #region GetSeries

        public async Task<SeriesInfo[]> ListSeriesAsync(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                throw new Exception("No valid filter query was given");

            var data = await DataClient.GetAsync<dynamic>("https://api.crunchyroll.com", "list_series",
                ("media_type", "anime"),
                ("filter", $"prefix:{filter}"),
                ("session_id", Tokens.SessionId),
            ("auth_token", Tokens.AuthToken));

            var dataItem = (JArray)data.data;

            return dataItem.ToObject<SeriesInfo[]>();
        }

        #endregion GetSeries

        #region GetInfo

        public async Task<T> GetInfoAsync<T>(long media_id = 0, long collection_id = 0, long series_id = 0)
        {
            if (media_id == 0 && series_id == 0 && collection_id == 0)
                throw new Exception("All parameters were empty. Please give a valid id number to either one of the parameters");

            var data = await DataClient.GetAsync<dynamic>("https://api.crunchyroll.com",
                "info",
                media_id != 0
                ? ("media_id", media_id)
                : collection_id != 0
                ? ("collection_id", collection_id)
                : ("series_id", series_id),
                ("session_id", Tokens.SessionId),
            ("auth_token", Tokens.AuthToken));

            var dataItem = (JObject)data.data;

            return dataItem.ToObject<T>();
        }

        #endregion GetInfo

        #region GetStreamData

        public async Task<StreamInfo> GetStreamInfoAsync(long media_id)
        {
            var data = await DataClient.GetAsync<dynamic>("https://api.crunchyroll.com",
                "info",
                ("media_id", media_id),
                ("fields", "media.stream_data"),
                ("session_id", Tokens.SessionId),
            ("auth_token", Tokens.AuthToken));

            var dataItem = (JObject)data.data.stream_data;

            return dataItem.ToObject<StreamInfo>();
        }

        public async Task<StreamInfo> GetStreamInfoAsync(EpisodeInfo episodeInfo)
            => await GetStreamInfoAsync(episodeInfo.Id);

        #endregion GetStreamData

        #region Search

        public async Task<SeriesInfo[]> SearchAsync(string query)
        {
            var data = await DataClient.GetAsync<dynamic>("https://api.crunchyroll.com",
                "search",
                ("q", query),
                ("media_types", "anime"),
                ("classes", "series"),
                ("session_id", Tokens.SessionId),
                ("auth_token", Tokens.AuthToken));

            var dataItem = (JArray)data.data;

            return dataItem.ToObject<SeriesInfo[]>();
        }

        #endregion Search

        #region Login

        public async Task LoginAsync(string username, string password)
        {
            var postcontent = new LoginInfo(username, password);
            var data = await DataClient.PostAsync<dynamic>(postcontent,
                "https://api.crunchyroll.com",
                "login",
                ("account", username),
                ("password", password),
                ("session_id", Tokens.SessionId));

            Tokens.AuthToken = data.data.auth;
            Tokens.AuthExpires = DateTime.Now
                .AddMonths(1)
                .AddHours(1);
            Tokens.Account = data.data.user;
        }

        #endregion Login

        #region Save Tokens and Credentials

        public void SaveTokens(string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(Tokens));
        }

        public void LoadTokens(string path)
        {
            var text = File.ReadAllText(path);
            Tokens = JsonConvert.DeserializeObject<TokenInfo>
                 (text);

            if (DateTime.Now > Tokens.SessionExpires)
                throw new TokenInvalidException("This saved session isn't valid anymore", 0);

            if (DateTime.Now > Tokens.AuthExpires)
                throw new TokenInvalidException("This saved authorization code isn't valid anymore", 1);
        }

        #endregion Save Tokens and Credentials

        public void Dispose()
        {
            ((IDisposable)DataClient).Dispose();
        }
    }

    public class TokenInvalidException : Exception
    {
        public string ErrorMessage { get; }
        public int ErrorCode { get; }

        public TokenInvalidException(string errorMessage, int errorCode)
        {
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
            ErrorCode = errorCode;
        }
    }

    public class TokenInfo
    {
        public TokenInfo(string sessionId, DateTime sessionExpires, string authToken, DateTime authExpires, dynamic account)
        {
            SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
            SessionExpires = sessionExpires;
            AuthToken = authToken ?? throw new ArgumentNullException(nameof(authToken));
            AuthExpires = authExpires;
            Account = account;
        }

        public string SessionId { get; set; }
        public DateTime SessionExpires { get; set; }
        public string AuthToken { get; set; }
        public DateTime AuthExpires { get; set; }
        public dynamic Account { get; set; }
    }

    public class LoginInfo
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public LoginInfo(string account, string password)
        {
            Account = account;
            Password = password;
        }
    }

    public class EpisodeInfo
    {
        [JsonProperty("media_id")]
        public long Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("episode_number")]
        public string EpisodeIndex { get; private set; }

        [JsonProperty("available")]
        public bool IsAvailable { get; private set; }

        [JsonProperty("available_time")]
        public string AvailableTime { get; private set; }

        [JsonProperty("duration")]
        public long Duration { get; private set; }

        [JsonProperty("premium_only")]
        public bool IsPremiumOnly { get; private set; }
    }

    public class SeasonInfo
    {
        [JsonProperty("collection_id")]
        public long Id { get; private set; }

        [JsonProperty("media_type")]
        public string Type { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("landscape_image")]
        public JObject LandscapeImage { get; private set; }

        [JsonProperty("portrait_image")]
        public JObject PortraitImage { get; private set; }

        [JsonProperty("availability_notes")]
        public string AvailabilityNotes { get; private set; }

        [JsonProperty("complete")]
        public bool IsComplete { get; private set; }

        [JsonProperty("created")]
        public string CreatedAt { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("season")]
        public int SeasonIndex { get; private set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public class SeriesInfo
    {
        [JsonProperty("series_id")]
        public long Id { get; private set; }

        [JsonProperty("media_type")]
        public string Type { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("landscape_image")]
        public JObject LandscapeImage { get; private set; }

        [JsonProperty("portrait_image")]
        public JObject PortraitImage { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }
    }

    public class StreamInfo

    {
        public class Stream
        {
            [JsonProperty("quality")]
            public string Quality { get; private set; }

            [JsonProperty("expires")]
            public DateTimeOffset Expires { get; private set; }

            [JsonProperty("url")]
            public string Url { get; private set; }

            public async Task<(string ProgramUrl, Size Quality)[]> GetStreamProgramsAsync()
            {
                string text = string.Empty;
                var result = new List<(string ProgramUrl, Size Quality)>();

                using (var wc = new WebClient())
                    text = await wc.DownloadStringTaskAsync(Url);

                var lines = text.Split('\n');
                for (int i = 1; i < lines.Length - 1; i = i + 2)
                {
                    var program = lines[i].Split(',');
                    var url = lines[i + 1];

                    var size = program[2].Split('=')[1].Split('x');

                    result.Add((url, new Size(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]))));
                }

                return result.ToArray();
            }
        }

        [JsonProperty("streams")]
        public Stream[] Streams { get; private set; }

        [JsonProperty("hardsub_lang")]
        public string HardsubLanguage { get; private set; }

        [JsonProperty("audio_lang")]
        public string AudioLanguage { get; private set; }
    }

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