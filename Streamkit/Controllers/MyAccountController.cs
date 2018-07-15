using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Streamkit.Web;
using Streamkit.Core;

namespace Streamkit.Controllers {
    public class MyAccountController : WebController {
        public IActionResult Index() {
            User user = SessionManager.GetUser(HttpContext.Session.Id);
            if (user == null) return Unauthorized();
            ViewBag.Username = user.TwitchUsername;
            return View();
        }
    }
}