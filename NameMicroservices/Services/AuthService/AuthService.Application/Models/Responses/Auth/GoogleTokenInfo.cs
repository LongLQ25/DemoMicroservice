using System.Text.Json.Serialization;

namespace AuthService.Application.Models.Responses.Auth
{
    public class GoogleTokenInfo
    {
        public string Email { get; set; }
        [JsonPropertyName("email_verified")]
        public string EmailVerified { get; set; }
        public string Aud { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
    }
}
