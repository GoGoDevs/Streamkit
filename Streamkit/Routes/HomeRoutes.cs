using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

using Streamkit.Web;
using Streamkit.Core;

namespace Streamkit.Routes {
    public static class HomeRoutes {
        public static IActionResult Index(RequestHandler<IActionResult> req) {
            return req.Controller.View();
        }

        public static IActionResult Privacy(RequestHandler<IActionResult> req) {
            return req.Controller.View();
        }

        public static IActionResult Subscribers(RequestHandler<IActionResult> req) {
            User user = UserManager.GetUserTwitch("damouryouknow");

            // TODO: Chain requests if gogomic ever gets over 100 subs.
            string url = "https://api.twitch.tv/kraken/channels/" + user.TwitchId 
                       + "/subscriptions?limit=100"; 

            GetRequest getReq = new GetRequest(url);
            getReq.AddHeader(HttpRequestHeader.Accept, "application/vnd.twitchtv.v5+json");
            getReq.AddHeader("Client-ID", Config.TwitchOAuth.ClientId);
            getReq.AddHeader("Authorization", "OAuth " + user.TwitchToken);

            JObject resp = getReq.GetResponseJson();
            JArray subs = (JArray)resp["subscriptions"];
            subs.RemoveAt(0);

            req.View.Subs = subs;
            return req.Controller.View();
        }
    }
}