using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json.Linq;

using Streamkit.Web;
using Streamkit.Core;
using Streamkit.Utils;

namespace Streamkit.Routes.Web {
    public static class StreamkitRoutes {
        public static IActionResult Index(RequestHandler<IActionResult> req) {
            if (req.User == null) throw new UnauthorizedException();

            Bitbar bitbar = BitbarManager.GetBitbar(req.User);
            req.View.Bitbar = bitbar;

            return req.Controller.View();
        }

        public static IActionResult BitbarSource(RequestHandler<IActionResult> req) {
            string id = req.Request.Query["id"];

            return req.Controller.View();
        }

        public static IActionResult BitbarSubmit(RequestHandler<IActionResult> req) {
            IFormCollection form = req.Request.Form;

            if (req.User == null) throw new UnauthorizedException();

            try {
                int value = int.Parse(form["value"]);
                int maxValue = int.Parse(form["max_value"]);
                string targetColor = form["target_color"].ToString();
                string fillColor = form["fill_color"].ToString();

                byte[] image = null;
                if (form.Files.Count > 0) {
                    image = new byte[form.Files["image"].Length];
                    form.Files["image"].OpenReadStream().Read(image, 0, image.Length);
                }


                Bitbar bitbar = BitbarManager.GetBitbar(req.User);
                bitbar.Value = value;
                bitbar.MaxValue = maxValue;
                if (image != null) bitbar.Image = image;
                bitbar.TargetColor = targetColor;
                bitbar.FillColor = fillColor;

                BitbarManager.UpdateBitbar(bitbar);
            }
            catch (Exception ex) {
                throw new BadRequestException();
            }

            return req.Controller.RedirectToAction("Index", "Streamkit");
        }
    }
}