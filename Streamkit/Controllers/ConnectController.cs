﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Streamkit.Controllers
{
    public class ConnectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}