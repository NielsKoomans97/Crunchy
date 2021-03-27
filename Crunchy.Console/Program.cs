using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Crunchy.CUI
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            while (true) ;

            if (args.Length < 0)
                throw new Exception("No valid arguments were passed");

            /// Arguments
            var url = (string)IsNull(args[0]);
            var path = (string)IsNull(args[1]);
            var quality = (string)IsNull(args[2]);
            var username = (string)IsNull(args[3]);
            var password = (string)IsNull(args[4]);
            var series = new SeriesInfo();

            /// Functional code
            var crunchyroll = new Crunchyroll();
            await crunchyroll.InitializeAsync();
            await crunchyroll.LoginAsync(username, password);

            var results = await crunchyroll.SearchAsync(url.Substring(url.LastIndexOf('/'), url.Length));

            if (results.Length < 0)
                throw new Exception("No search results were found for this url");

            if (results.Length > 0)
            {
                foreach (var result in results)
                {
                    Console.WriteLine($"[{result.Id}]   {result.Name}");
                }

                Console.WriteLine();
                Console.Write("Enter the index number of your selected item >");

                var index = Convert.ToInt32(Console.ReadLine());
                series = results[index];
            }
            else
            {
                series = results[0];
            }

            if (Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private static object IsNull(object? obj)
        {
            if (obj == null)
                throw new Exception($"{obj} was null");

            return obj;
        }
    }
}

public class Crunchyroll : IDisposable
{
    private string SessionId;
    private DateTime Expires;

    private string AuthToken;
    private DateTime AuthExpires;

    private HttpClient DataClient;

    public Crunchyroll()
    {
        DataClient = new HttpClient();
    }

    internal Crunchyroll(string sessionId, DateTime expires)
    {
        SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
        Expires = expires;
    }

    public async Task InitializeAsync()
    {
        var session = await DataClient.GetAsync<dynamic>("https://api.crunchyroll.com",
            "start_session",
            ("device_id", "00000000-4d19-2528-ffff-ffff99d603a9"),
            ("device_type", "com.crunchyroll.windows.desktop"),
            ("access_token", "LNDJgOit5yaRIWN"));

        SessionId = session.data.session_id;
        Expires = DateTime.Now.AddHours(4);
    }

    #region GetEpisodes

    public async Task<EpisodeInfo[]> ListEpisodesAsync(long series_id = 0, long collection_id = 0)
    {
        if (series_id == 0 && collection_id == 0)
            throw new Exception("Both parameters were empty. Please give a valid id number to either one of the parameters");

        var data = await DataClient.GetAsync<dynamic>("https://api.crunchyroll.com", "list_media",
            series_id != 0 ? ("series_id", series_id) : ("collection_id", collection_id),
            ("session_id", SessionId));

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
            ("session_id", SessionId));

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
            ("session_id", SessionId));

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
            ("session_id", SessionId));

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
            ("session_id", SessionId));

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
            ("session_id", SessionId));

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
            ("session_id", SessionId));

        AuthToken = data.data.auth;
        AuthExpires = DateTime.Now
            .AddMonths(1)
            .AddHours(1);
    }

    #endregion Login

    #region Save Tokens and Credentials

    public void SaveTokens(string path)
    {
        var tokenInfo = new TokenInfo(
            (SessionId, Expires),
            (AuthToken, AuthExpires));

        File.WriteAllText(path, JsonConvert.SerializeObject(tokenInfo));
    }

    public void LoadTokens(string path)
    {
        var text = File.ReadAllText(path);
        var tokenInfo = JsonConvert.DeserializeObject<TokenInfo>
            (text);

        if (DateTime.Now > tokenInfo.Session.Expires)
            throw new Exception("This saved session isn't valid anymore");

        if (DateTime.Now > tokenInfo.Auth.Expires)
            throw new Exception("This saved authorization code isn't valid anymore");
    }

    #endregion Save Tokens and Credentials

    public void Dispose()
    {
        ((IDisposable)DataClient).Dispose();
    }
}

public class TokenInfo
{
    public TokenInfo((string Id, DateTime Expires) session, (string Auth, DateTime Expires) auth)
    {
        Session = session;
        Auth = auth;
    }

    public (string Id, DateTime Expires) Session { get; private set; }
    public (string Auth, DateTime Expires) Auth { get; private set; }
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