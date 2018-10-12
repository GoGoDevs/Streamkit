using System;
using Microsoft.AspNetCore.Mvc;

using Streamkit.Core;
using Streamkit.Web;
using Streamkit.Crypto;
using Streamkit.OAuth;

namespace Streamkit.Routes.Web {
    public static class LoginRoutes {
        public static IActionResult Index(RequestHandler<IActionResult> req) {
            UrlParams param = new UrlParams();
            param.Add("client_id", Config.TwitchOAuth.ClientId);
            param.Add("redirect_uri", Config.OAuthRedirect);
            param.Add("response_type", "code");
            param.Add("scope", Config.TwitchScope);
            param.Add("force_verify", "true");
            param.Add("state", TokenGenerator.Generate()); // TODO: Generate token for this.

            string twitchUrl = "https://api.twitch.tv/kraken/oauth2/authorize"
                    + param.ToString();

            req.View.TwitchUrl = twitchUrl;

            return req.Controller.View();
        }

        public static IActionResult Twitch(RequestHandler<IActionResult> req) {
            string code = req.Request.Query["code"];
            string state = req.Request.Query["state"];

            // Create a new streamkit account for this user.
            string token = TwitchOAuth.GetToken(code, state);

            Tuple<string, string> accountInfo = TwitchOAuth.Validate(token);

            User user = UserManager.UpsertUser(accountInfo.Item1, accountInfo.Item2, token);

            req.Controller.HttpContext.Session.Set("init", new byte[] { 0 });
            SessionManager.AddSession(req.Controller.HttpContext.Session.Id, user);

            return req.Controller.RedirectToAction("Index", "MyAccount");
        }
    }
}