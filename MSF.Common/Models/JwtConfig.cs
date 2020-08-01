namespace MSF.Common.Models
{
    public class JwtConfig
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }
        
        public int AccessTokenExpiration { get; set; }
    }
}
