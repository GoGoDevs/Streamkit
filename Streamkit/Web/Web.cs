using System;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore;
using Newtonsoft.Json.Linq;

using Streamkit.Core;
using Streamkit.Crypto;

namespace Streamkit.Web {
    public abstract class RequestHandler<T> {
        protected WebController controller;
        protected Func<RequestHandler<T>, T> handler;
        protected User user;

        public RequestHandler(
                WebController controller, Func<RequestHandler<T>, T> handler) {
            this.controller = controller;
            this.handler = handler;
        }

        public WebController Controller {
            get { return this.controller; }
        }

        public User User {
            get { return this.user; }
        }

        public Microsoft.AspNetCore.Http.HttpRequest Request {
            get { return this.controller.Request; }
        }

        public Microsoft.AspNetCore.Http.HttpResponse Response {
            get { return this.controller.Response; }
        }

        public dynamic View {
            get { return this.controller.ViewBag; }
        }

        public T Handle() {
            try {
                // Get user making request.
                this.user = SessionManager.GetUser(
                        this.controller.HttpContext.Session.Id);
                // Add user to view bag so we can controller visible items in main layout.
                this.controller.ViewBag.User = user;

                this.handleMiddleware();
                return this.handler(this);

            } catch(Exception ex) {
                Logger.Log("Error in request " + this.Request.Path + ": " + ex.Message);
                return this.handleError(ex);
            }
        }

        /// <summary>
        /// This should be overriden if you would like to run additional middleware.
        /// </summary>
        protected virtual void handleMiddleware() {
            return;
        }

        protected abstract T handleError(Exception ex);
    }


    public class ActionRequestHandler : RequestHandler<IActionResult> {
        public ActionRequestHandler(
                WebController controller,
                Func<RequestHandler<IActionResult>, IActionResult> handler) : base(controller, handler) { }


        protected override IActionResult handleError(Exception ex) {
            this.Response.StatusCode = 500;
            return Controller.View("~/Views/Error.cshtml");
        }
    }



    // In case we ever need to add our own behaviour to Controller.
    public class WebController : Controller {
        public WebController() {
            
        }
    }


    public class SessionManager  {
        private static Dictionary<string, User> Sessions = new Dictionary<string, User>();

        public static void AddSession(string id, User user) {
            Sessions[id] = user;
        }

        public static User GetUser(string sessionId) {
            if (Sessions.ContainsKey(sessionId)) {
                return Sessions[sessionId];
            }
            return null;
        }
    }


    public abstract class HttpRequest {
        protected WebRequest request;
        protected HttpWebResponse response;
        protected string url;
        protected UrlParams param = new UrlParams();
        protected byte[] body;

        protected Dictionary<HttpRequestHeader, string> headers
                = new Dictionary<HttpRequestHeader, string>();

        protected Dictionary<string, string> strHeaders
                = new Dictionary<string, string>();

        public HttpRequest(string url) {
            this.url = url;
        }

        public void AddParam(string name, string value) {
            this.param.Add(name, value);
        }

        public void AddHeader(HttpRequestHeader header, string value) {
            this.headers.Add(header, value);
        }

        public void AddHeader(string header, string value) {
            this.strHeaders.Add(header, value);
        }

        public abstract string Method { get; }

        public virtual byte[] GetResponseBytes() {
            this.configure();
            this.response = (HttpWebResponse)this.request.GetResponse();

            using (Stream responseStream = this.response.GetResponseStream()) {
                using (BinaryReader reader = new BinaryReader(responseStream)) {
                    return reader.ReadBytes((int)this.response.ContentLength);
                }
            }
        }

        public string GetResponse() {
            return Encoding.UTF8.GetString(this.GetResponseBytes());
        }

        public JObject GetResponseJson() {
            return JObject.Parse(this.GetResponse());
        }

        private void configure() {
            this.request = WebRequest.Create(url + param.ToString());
            this.request.Method = this.Method;

            foreach (KeyValuePair<HttpRequestHeader, string> header in this.headers) {
                this.request.Headers.Add(header.Key, header.Value);
            }

            foreach (KeyValuePair<string, string> header in this.strHeaders) {
                this.request.Headers.Add(header.Key, header.Value);
            }

            if (this.body != null) {
                this.request.ContentLength = this.body.Length;
                using (var stream = this.request.GetRequestStream()) {
                    stream.Write(this.body, 0, this.body.Length);
                }
            }
        }
    }


    public class GetRequest : HttpRequest {
        public GetRequest(string url) : base(url) { }

        public override string Method {
            get { return "GET"; }
        }
    }


    public class PostRequest : HttpRequest {
        public PostRequest(string url) : base(url) { }

        public override string Method {
            get { return "POST"; }
        }

        public byte[] Body {
            set {
                this.body = value;
            }
        }

        public string BodyString {
            set {
                this.Body = Encoding.UTF8.GetBytes(value);
            }
        }

        public JObject BodyJson {
            set {
                this.BodyString = value.ToString();
            }
        }
    }


    // TODO: Add other HTTP request methods as required.


    public class UrlParams : IDictionary<string, string> {
        private Dictionary<string, string> param
                = new Dictionary<string, string>();

        public string Form {
            get { return this.ToString().Replace("?", ""); }
        }

        public string this[string key] {
            get { return this.param[key]; }
            set { this.param[key] = value; }
        }

        public ICollection<string> Keys {
            get { return this.param.Keys; }
        }

        public ICollection<string> Values {
            get { return this.param.Values; }
        }

        public int Count {
            get { return this.param.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void Add(string key, string value) {
            this.param.Add(key, value);
        }

        public void Add(KeyValuePair<string, string> item) {
            this.param.Add(item.Key, item.Value);
        }

        public void Clear() {
            this.param.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item) {
            foreach (KeyValuePair<string, string> kvp in this.param) {
                if (kvp.Key == item.Key && kvp.Value == item.Value) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(string key) {
            return this.param.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) {
            foreach (KeyValuePair<string, string> kvp in this.param) {
                array[arrayIndex++] = kvp;
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return this.param.GetEnumerator();
        }

        public bool Remove(string key) {
            return this.param.Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item) {
            if (this.param.ContainsKey(item.Key) 
                    && this.param[item.Key] == item.Value) {
                return this.Remove(item.Key);
            }
            return false;
        }

        public bool TryGetValue(string key, out string value) {
            value = null;
            if (this.param.ContainsKey(key)) {
                value = this.param[key];
                return true;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public override string ToString() {
            if (this.param.Count == 0) return null;

            List<string> kvpStrings = new List<string>();
            foreach (KeyValuePair<string, string> kvp in this.param) {
                kvpStrings.Add(kvp.Key + "=" + kvp.Value);
            }

            return "?" + String.Join("&", kvpStrings.ToArray());
        }

        public string ToPostString() {
            return this.ToString().Substring(1);
        }
    }
}