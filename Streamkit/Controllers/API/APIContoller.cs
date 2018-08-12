using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Streamkit.Core;
using Streamkit.Web;
using Streamkit.Routes.API;

namespace Streamkit.Controllers.Web {
    [Route("api/streamkit/[action]")]
    public class StreamkitAPIController : APIController {
        public string Bitbar() {
            JsonRequestHandler req = new JsonRequestHandler(
                    this, APIRoutes.Bitbar);
            return req.Handle().ToString();
        }

        public string BitbarSource() {
            JsonRequestHandler req = new JsonRequestHandler(
                    this, APIRoutes.BitbarSource);
            return req.Handle().ToString();
        }
    }
}
