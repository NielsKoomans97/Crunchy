using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;

namespace Crunchy.Crunchyroll
{
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
}