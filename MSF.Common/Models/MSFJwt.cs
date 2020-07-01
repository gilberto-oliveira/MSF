using System;

namespace MSF.Common.Models
{
    public class MSFJwt
    {
        public bool Authenticated { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public string Message { get; set; }
    }
}
