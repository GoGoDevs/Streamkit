using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

using Streamkit.Core;
using Streamkit.Web;
using Streamkit.Routes.Web;

namespace Streamkit.Controllers.Web
{
    public class HomeController : WebController
    {
        public IActionResult Index() {
            ActionRequestHandler req = new ActionRequestHandler(
                    this, HomeRoutes.Index);
            return req.Handle();
        }

        public IActionResult Privacy() {
            ActionRequestHandler req = new ActionRequestHandler(
                    this, HomeRoutes.Privacy);
            return req.Handle();
        }

        public IActionResult Subscribers() {
            ActionRequestHandler req = new ActionRequestHandler(
                    this, HomeRoutes.Subscribers);
            return req.Handle();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
