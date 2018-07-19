using Microsoft.AspNetCore.Mvc;

using Streamkit.Web;

namespace Streamkit.Routes {
    public static class HomeRoutes {
        public static IActionResult Index(RequestHandler<IActionResult> req) {
            return req.Controller.View();
        }

        public static IActionResult Privacy(RequestHandler<IActionResult> req) {
            return req.Controller.View();
        }
    }
}