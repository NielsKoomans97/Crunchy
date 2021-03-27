using Newtonsoft.Json;

namespace Crunchy.Crunchyroll
{
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
}