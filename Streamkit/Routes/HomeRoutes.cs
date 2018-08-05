using System.Net;
using System.Collections.Generic;
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
            int limit = 100;
            int total = int.MaxValue;

            User user = UserManager.GetUserTwitch("damouryouknow");

            JArray subs = new JArray();
            HashSet<string> ids = new HashSet<string>();

            for (int offset = 0; offset + limit <= total; offset += limit) {
                UrlParams param = new UrlParams();
                param.Add("limit", limit.ToString());
                param.Add("offset", offset.ToString());

                // TODO: Chain requests if gogomic ever gets over 100 subs.
                string url = "https://api.twitch.tv/kraken/channels/" + user.TwitchId
                           + "/subscriptions" + param.ToString();

                GetRequest getReq = new GetRequest(url);

                getReq.AddHeader(HttpRequestHeader.Accept, "application/vnd.twitchtv.v5+json");
                getReq.AddHeader("Client-ID", Config.TwitchOAuth.ClientId);
                getReq.AddHeader("Authorization", "OAuth " + user.TwitchToken);

                JObject resp = getReq.GetResponseJson();

                foreach (JToken sub in (JArray)resp["subscriptions"]) {
                    string id = (string)sub["user"]["_id"];

                    if (ids.Contains(id) || id == user.TwitchId) continue;
         
                    ids.Add(id);
                    subs.Add(sub);
                }

                total = (int)resp["_total"];


            }

            req.View.Subs = subs;
            return req.Controller.View();
        }
    }
}