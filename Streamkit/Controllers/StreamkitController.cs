using Microsoft.AspNetCore.Mvc;

using Streamkit.Web;
using Streamkit.Routes;

namespace Streamkit.Controllers
{
    public class StreamkitController : WebController
    {
        public IActionResult Index() {
            ActionRequestHandler req = new ActionRequestHandler(
                    this, StreamkitRoutes.Index);
            return req.Handle();
        }

        public IActionResult BitbarSource() {
            ActionRequestHandler req = new ActionRequestHandler(
                    this, StreamkitRoutes.BitbarSource);
            return req.Handle();
        }

        public IActionResult BitbarSubmit() {
            ActionRequestHandler req = new ActionRequestHandler(
                    this, StreamkitRoutes.BitbarSubmit);
            return req.Handle();
        }

        // TODO: Create API...
        public IActionResult GetBitbar() {
            ActionRequestHandler req = new ActionRequestHandler(
                    this, StreamkitRoutes.GetBitbar);
            return req.Handle();
        }
    }
}