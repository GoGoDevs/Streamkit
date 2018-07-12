﻿using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

using Streamkit.Web;
using Streamkit.OAuth;
using Streamkit.Crypto;

namespace Streamkit.Controllers {
    public class LoginController : Controller {
        public IActionResult Index() {
            UrlParams param = new UrlParams();
            param.Add("client_id", Config.TwitchOAuth.ClientId);
            param.Add("redirect_uri", Config.OAuthRedirect);
            param.Add("response_type", "code");
            param.Add("scope", Config.TwitchScope);
            param.Add("force_verify", "true");
            param.Add("state", TokenGenerator.Generate()); // TODO: Generate token for this.

            string twitchUrl = "https://api.twitch.tv/kraken/oauth2/authorize"
                    + param.ToString();

            ViewBag.TwitchUrl = twitchUrl;

            return View();
        }

        public IActionResult Twitch() {
            string code = Request.Query["code"];
            string state = Request.Query["state"];

            // Create a new streamkit account for this user.
            string token = TwitchOAuth.GetToken(code, state);

            Tuple<string, string> accountInfo = TwitchOAuth.Validate(token);

            return RedirectToAction("MyAccount", "Index");
        }
    }
}