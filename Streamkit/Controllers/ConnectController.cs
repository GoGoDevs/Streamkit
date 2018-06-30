using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

using Streamkit.Web;

namespace Streamkit.Controllers
{
    public class ConnectController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Map OAuth connection links to model.
            // TODO: Move OAuth logic to OAuth namespace after we get it working.
            HttpGetRequest req = new HttpGetRequest("todo url");
            req.AddParam("client_id", Config.TwitchOAuth.ClientId);
            req.AddParam("redirect_uri", Config.OAuthRedirect);
            req.AddParam("response_type", "code");
            req.AddParam("scope", Config.TwitchScope);
            req.AddParam("force_verify", "true");
            req.AddParam("state", null); // TODO: Generate token for this.

            JObject res = req.GetResponseJson();


            // TODO: ServiceConnectUrl class for storing the connection urls
            // to all OAuth services.
            ViewData["service_connect_url"] = null;

            return View();
        }
    }
}