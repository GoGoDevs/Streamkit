using Microsoft.AspNetCore.Mvc;

using Streamkit.Web;

namespace Streamkit.Routes {
    public static class MyAccountRoutes {
        public static IActionResult Index(RequestHandler<IActionResult> req) {
            if (req.User == null) return req.Controller.Unauthorized();
            req.View.Username = req.User.TwitchUsername;
            return req.Controller.View();
        }
    }
}