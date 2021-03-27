using System;

namespace Crunchy.Crunchyroll
{
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
}