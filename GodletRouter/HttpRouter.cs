using System;
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

        public Route HandleFunc(string path, Action<HttpListenerRequest, HttpListenerResponse> f)
        {
            return this.NewRoute();
        }

        public Route NewRoute()
        {
            Route route = new Route();

            this.routes.Add(route);
            return route;
        }

        /// <summary>
        /// HttpRouter.Service dispatches the handler registered in the matched route.
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

            RouteMatch match = new RouteMatch();
            IHttpHandler handler = null;

            if (this.Match(request, match))
            {
                handler = match.Handler;
            }

            if (handler == null)
            {
                handler = this.NotFoundHandler;
            }
            if (handler != null)
            {
                handler.Service(request, response);
            }
        }

        private bool Match(HttpListenerRequest request, RouteMatch match)
        {
            foreach (var route in this.routes)
            {
                if (route.Match(request, match))
                {
                    // Build middleware chain if no error was found
                    if (match.MatchErr == null)
                    {
                        foreach (var mw in this.middleWares)
                        {
                            match.Handler = mw.Middleware(match.Handler);
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
