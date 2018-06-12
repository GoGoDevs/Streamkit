using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Streamkit.Web;

namespace Streamkit.Controllers
{
    public class ConnectController : Controller
    {
        public IActionResult Index()
        {
            // TODO: Map OAuth connection links to model.
            // TODO: Move OAuth logic to OAuth namespace after we get it working.

            // URL params for requesting twitch authentification.
            // TODO: Read values from credentials config file.
            UrlParams urlParams = new UrlParams();
            urlParams.Add("client_id", null);
            urlParams.Add("redirect_uri", null);
            urlParams.Add("response_type", "code");
            urlParams.Add("scope", null);
            urlParams.Add("force_verify", "true");
            urlParams.Add("state", null); // TODO: Generate token for this.

            // TODO: ServiceConnectUrl class for storing the connection urls
            // to all OAuth services.
            ViewData["service_connect_url"] = null;

            return View();
        }
    }
}