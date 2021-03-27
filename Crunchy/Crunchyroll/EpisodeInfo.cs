using Newtonsoft.Json;

namespace Crunchy.Crunchyroll
{
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
}