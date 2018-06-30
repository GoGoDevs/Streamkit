using Streamkit.Web;

namespace Streamkit.OAuth {
    public static class TwitchOAuth {
        public static string GetToken(string code) {
            GetRequest req = new GetRequest(
                    "https://api.twitch.tv/kraken/oauth2/token");

            req.AddParam("client_id", Config.TwitchOAuth.ClientId);
            req.AddParam("client_secret", Config.TwitchOAuth.Secret);
            req.AddParam("code", code);
            req.AddParam("grant_type", "authorization_code");
            req.AddParam("redirect_uri", Config.OAuthRedirect); // FIX need to add root url in front of this.

            // TODO...
            throw new System.NotImplementedException();
        }
    }
}