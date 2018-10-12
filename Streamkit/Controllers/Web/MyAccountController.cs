using Microsoft.AspNetCore.Mvc;

using Streamkit.Web;
using Streamkit.Routes.Web;

namespace Streamkit.Controllers.Web {
    public class MyAccountController : WebController {
        public IActionResult Index() {
            ActionRequestHandler req = new ActionRequestHandler(
                    this, MyAccountRoutes.Index);
            return req.Handle();
        }
    }
}