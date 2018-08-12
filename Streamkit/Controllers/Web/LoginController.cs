using Microsoft.AspNetCore.Mvc;

using Streamkit.Routes.Web;
using Streamkit.Web;

namespace Streamkit.Controllers.Web {
    public class LoginController : WebController {
        public IActionResult Index() {
            ActionRequestHandler req = new ActionRequestHandler(
                    this, LoginRoutes.Index);
            return req.Handle();
        }

        public IActionResult Twitch() {
            ActionRequestHandler req = new ActionRequestHandler(
                    this, LoginRoutes.Twitch);
            return req.Handle();
        }
    }
}