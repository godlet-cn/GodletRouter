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

            this.NotFoundHandler = new DefaultNotFoundHandler();
        }

        public IHttpHandler NotFoundHandler { get; set; }

        private List<Route> routes;

        #region ===Use MiddleWare===

        private event MiddleWareFunc middleWares;

        public void Use(params MiddleWareFunc[] mws)
        {
            foreach (var mw in mws)
            {
                this.middleWares += mw;
            }
        }

        public void Use(params IHttpHandler[] mws)
        {
            foreach (var mw in mws)
            {
                this.middleWares += mw.Service;
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

        public void Service(HttpListenerRequest request, HttpListenerResponse response)
        {
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
            return false;
        }
    }
}
