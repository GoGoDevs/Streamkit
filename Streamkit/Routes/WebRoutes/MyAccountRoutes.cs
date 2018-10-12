using Microsoft.AspNetCore.Mvc;

using Streamkit.Web;

namespace Streamkit.Routes.Web {
    public static class MyAccountRoutes {
        public static IActionResult Index(RequestHandler<IActionResult> req) {
            if (req.User == null) throw new UnauthorizedException();
            req.View.Username = req.User.TwitchUsername;
            return req.Controller.View();
        }
    }
}