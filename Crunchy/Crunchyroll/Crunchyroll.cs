using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crunchy.Crunchyroll
{
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
}