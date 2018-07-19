using Microsoft.AspNetCore.Mvc;

using Streamkit.Web;
using Streamkit.Routes;

namespace Streamkit.Controllers {
    public class MyAccountController : WebController {
        public IActionResult Index() {
            ActionRequestHandler req = new ActionRequestHandler(
                    this, MyAccountRoutes.Index);
            return req.Handle();
        }
    }
}