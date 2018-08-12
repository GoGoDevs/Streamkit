using System;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

using Streamkit.Core;
using Streamkit.Utils;

using Streamkit.Web;

namespace Streamkit.Routes.API {
    public static class APIRoutes {
        public static JToken Bitbar(RequestHandler<JToken> req) {
            string id = req.Request.Query["id"];

            Bitbar bitbar = BitbarManager.GetBitbar(id);
            JObject source = new JObject();
            source["source_id"] = bitbar.Id;
            source["value"] = bitbar.Value;
            source["max_value"] = bitbar.MaxValue;
            source["target_color"] = "#" + bitbar.TargetColor;
            source["fill_color"] = "#" + bitbar.FillColor;

            return source;
        }

        public static JToken BitbarSource(RequestHandler<JToken> req) {
            string id = req.Request.Query["id"];

            Bitbar bitbar = BitbarManager.GetBitbar(id);
            JObject source = new JObject();
            source["source_id"] = bitbar.Id;
            source["image"] = Base64.Encode(bitbar.Image);

            return source;
        }
    }
}