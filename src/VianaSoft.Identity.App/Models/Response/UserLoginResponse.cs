using Newtonsoft.Json;

namespace VianaSoft.Identity.App.Models.Response
{
    public class UserLoginResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? AccessToken { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? ExpiresIn { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public UserTokenResponse? UserToken { get; set; }

    }
}
