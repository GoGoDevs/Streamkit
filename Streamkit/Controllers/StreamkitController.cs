using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Streamkit.Web;
using Streamkit.Core;

namespace Streamkit.Controllers
{
    public class StreamkitController : WebController
    {
        public IActionResult Index() {
            User user = SessionManager.GetUser(HttpContext.Session.Id);
            if (user == null) return Unauthorized();

            Bitbar bitbar = BitbarManager.GetBitbar(user);
            ViewBag.Bitbar = bitbar;

            return View();
        }

        public IActionResult BitbarSubmit() {
            IFormCollection form = Request.Form;

            User user = SessionManager.GetUser(HttpContext.Session.Id);
            if (user == null) return Unauthorized();

            try {
                int value = int.Parse(form["value"]);
                int maxValue = int.Parse(form["max_value"]);
                string color = form["color"].ToString();

                byte[] image = null;
                if (form.Files.Count > 0) {
                    image = new byte[form.Files["image"].Length];
                    form.Files["image"].OpenReadStream().Read(image, 0, image.Length);
                }


                Bitbar bitbar = BitbarManager.GetBitbar(user);
                bitbar.Value = value;
                bitbar.MaxValue = maxValue;
                if (image != null) bitbar.Image = image;
                bitbar.Color = color;

                BitbarManager.UpdateBitbar(bitbar);
            }
            catch(Exception ex) {
                return BadRequest();
            }

            return RedirectToAction("Index", "Streamkit");
        }
    }
}