using System;
using System.Net;

namespace GodletRouter
{
    /// <summary>
    /// A http middleware function which can handle http request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    public delegate void MiddleWareFunc(HttpListenerRequest request, HttpListenerResponse response);

    /// <summary>
    /// IHttpHandler is an interface which can handle http request
    /// </summary>
    public interface IHttpHandler
    {
        /// <summary>
        /// Handle http request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        void Service(HttpListenerRequest request,HttpListenerResponse response);
    }
}
