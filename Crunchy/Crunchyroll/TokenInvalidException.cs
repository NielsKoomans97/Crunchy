using System;

namespace Crunchy.Crunchyroll
{
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
}