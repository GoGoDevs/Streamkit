using System.Net;

using Newtonsoft.Json.Linq;

using Streamkit.Web;

namespace Streamkit.OAuth {
    public static class TwitchOAuth {
        public static string GetToken(string code, string state) {
            PostRequest req = new PostRequest(
                    "https://api.twitch.tv/kraken/oauth2/token");

            UrlParams param = new UrlParams();
            param.Add("client_id", Config.TwitchOAuth.ClientId);
            param.Add("client_secret", Config.TwitchOAuth.Secret);
            param.Add("code", code);
            param.Add("grant_type", "authorization_code");
            param.Add("redirect_uri", Config.OAuthRedirect); // May need to add root url in front of this?
            param.Add("state", state);

            req.AddHeader(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");

            req.BodyString = param.ToString();

            JObject resp = req.GetResponseJson();
            return (string)resp["access_token"];
        }

        public static void Validate(string token) {
            GetRequest req = new GetRequest("https://api.twitch.tv/kraken");

            req.AddHeader(HttpRequestHeader.Authorization, "OAuth " + token);
            req.AddHeader(HttpRequestHeader.Accept, "application/vnd.twitchtv.v5+json");

            JObject resp = new JObject();
            string userId = (string)resp["token"]["user_id"];
            string username = (string)resp["token"]["user_name"];
        }
    }
}