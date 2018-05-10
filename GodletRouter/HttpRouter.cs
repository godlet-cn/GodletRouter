using System.Collections.Generic;
using System.Net;

namespace GodletRouter
{
    public class HttpRouter : IHttpHandler
    {
        public HttpRouter()
        {
            routes = new List<Route>();
            middleWares = new List<IMiddleWare>();

            this.NotFoundHandler = new DefaultNotFoundHandler();
        }

        public IHttpHandler HomeHandler { get; set; }

        public IHttpHandler NotFoundHandler { get; set; }

        private List<Route> routes;

        private List<IMiddleWare> middleWares;

        private object mutex = new object();

        #region MiddleWares
        
        /// <summary>
        /// Appends middlewares to the chain.
        /// Middlewares are executed in the order that they are applied to the Router.
        /// </summary>
        /// <param name="mws"></param>
        public void Use(params IMiddleWare[] mws)
        {
            foreach (var mw in mws)
            {
                this.middleWares.Add(mw);
            }
        }

        #endregion

        public Route HandleFunc(string path, IHttpHandler handler)
        {
            lock (mutex)
            {
                Route route = new Route(path, handler);
                this.routes.Add(route);
                return route;
            }
        }

        /// <summary>
        ///  dispatches the request to the handler whose pattern most closely matches the request URL.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public void Service(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.Url.AbsolutePath.Equals("/"))
            {
                if (HomeHandler != null)
                {
                    HomeHandler.Service(request, response);
                }
            }

            IHttpHandler handler = Handler(request);

            if (handler == null)
            {
                handler = this.NotFoundHandler;
            }

            if (handler != null)
            {
                foreach (var mw in this.middleWares)
                {
                    handler = mw.Middleware(handler);
                }
                handler.Service(request, response);
            }
        }

        public IHttpHandler Handler(HttpListenerRequest request)
        {
            string host = request.UserHostAddress;
            string path = request.Url.AbsolutePath;

            IHttpHandler handler;
            string pattern;
            this.handler(host, path,out handler,out pattern);
            return handler;
        }

        private void handler(string host, string path,out IHttpHandler handler,out string pattern)
        {
            handler = null;
            pattern = "";

            // Check for exact match first.
            foreach (var route in this.routes)
            {
                if (route.Pattern == path) {
                    handler= route.Handler;
                    pattern = route.Pattern;
                }
            }

            // Check for longest valid match.
            int maxLen = 0;
            IHttpHandler h=null;

            lock (mutex)
            {
                foreach (var route in this.routes)
                {
                    if (!pathMatch(route.Pattern, path))
                    {
                        continue;
                    }
                    if (h == null || route.Pattern.Length > maxLen)
                    {
                        maxLen = route.Pattern.Length;
                        h = route.Handler;
                        pattern = route.Pattern;
                    }
                }
            }
        }

        private bool pathMatch(string pattern, string path)
        {
            if (pattern.Length == 0)
            {
                // should not happen
                return false;
            }

            int n = pattern.Length;
            if (pattern[n - 1] != '/')
            {
                return pattern == path;
            }
            return path.Length >= n && path.Substring(0, n) == pattern;
        }
    }
}
