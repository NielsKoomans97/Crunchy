using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Crunchy.Crunchyroll
{
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
}