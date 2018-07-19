using Microsoft.AspNetCore.Mvc;

using Streamkit.Routes;
using Streamkit.Web;

namespace Streamkit.Controllers {
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