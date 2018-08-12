using System;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;

using Streamkit.Web;

namespace Streamkit.Routes.API {
    public static class APIRoutes {
        public static JToken Bitbar(RequestHandler<JToken> req) {
            JObject obj = new JObject();
            obj["test"] = "hello";
            return obj;
            throw new NotImplementedException();
        }
    }
}