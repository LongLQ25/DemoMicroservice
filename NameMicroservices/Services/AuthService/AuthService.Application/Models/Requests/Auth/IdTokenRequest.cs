using System.Text.Json.Serialization;

namespace AuthService.Application.Models.Requests.Auth
{
    public class IdTokenRequest
    {
        [JsonPropertyName("idToken")]
        public string IdToken { get; set; }
    }
}
