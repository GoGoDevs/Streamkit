using System;
using System.IO;
using System.Configuration;

using Newtonsoft.Json.Linq;

namespace Streamkit {
    public enum Env {
        Development,
        Production
    }


    public static class Config {
        public static Env Environment;
        public static OAuthCredentials TwitchOAuth;
        public static string TwitchChatToken;
        public static string RootUrl;
        public static string TwitchScope;
        public static string OAuthRedirect;
        public static string AppPath;
        public static string AESKey;


        public static void Configure() {
            // TODO: Clean up this mess when we get things working.
            AppPath = Directory.GetCurrentDirectory();

            // Read oauth file.
            JObject oauth = JObject.Parse(
                    File.ReadAllText(AppPath + "/credentials/oauth.json"));

            JObject twitch = oauth["twitch"] as JObject;
            TwitchOAuth = new OAuthCredentials(
                    (string)twitch["client_id"], (string)twitch["secret"]);
            TwitchChatToken = (string)twitch["chat_token"];


            JObject config = null;
            if (Environment == Env.Development) {
                config = JObject.Parse(
                        File.ReadAllText(AppPath + "./appsettings.Development.json"));
            }
            if (Environment == Env.Production) {
                config = JObject.Parse(
                        File.ReadAllText(AppPath + "./appsettings.Production.json"));
            }

            RootUrl = (string)config["root_url"];
            TwitchScope = (string)config["oauth"]["twitch"]["scope"];
            OAuthRedirect = (string)config["oauth"]["redirect"];

            JObject credentials = JObject.Parse(
                    File.ReadAllText(AppPath + "/credentials/credentials.json"));

            AESKey = (string)credentials["aes_key"];

        }
    }




    public class OAuthCredentials {
        private string clientId;
        private string secret;

        public OAuthCredentials(string clientId, string secret) {
            this.clientId = clientId;
            this.secret = secret;
        }

        public string ClientId {
            get { return this.clientId; }
        }

        public string Secret {
            get { return this.clientId; }
        }
    }
}