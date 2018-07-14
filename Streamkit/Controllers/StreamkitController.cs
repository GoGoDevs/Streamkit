using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Streamkit.Web;

namespace Streamkit.Controllers
{
    public class StreamkitController : WebController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}