using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GodletRouter.Samples
{
    class HttpServer
    {
        private HttpRouter router;
        private HttpListener httpListener;

        public void Start()
        {
            string conn = getConnectionString("localhost", 4006);
            try
            {
                httpListener = new HttpListener();
                httpListener.Prefixes.Add(conn);
                httpListener.Start();
                httpListener.BeginGetContext(new AsyncCallback(getContextCallBack), httpListener);
                Console.WriteLine("Initializing HTTP server");
                Console.WriteLine("Listening at:" + conn);

                router = new HttpRouter();
                router.HandleFunc("/test", this.HandleTest);
            }
            catch (Exception err)
            {
                Console.WriteLine("Fatal:err start server:" + err.Message + " " + err.StackTrace);
            }
        }

        public void Stop()
        {
            httpListener.Stop();
        }

        private void HandleTest(HttpListenerRequest request, HttpListenerResponse response)
        {

        }

        /// <summary>
        /// Returns the connection string.
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private string getConnectionString(string hostname, int port)
        {
            return string.Format("http://{0}:{1}/", hostname, port);
        }

        /// <summary>
        /// Handle client request asynchronously
        /// </summary>
        /// <param name="ar"></param>
        private void getContextCallBack(IAsyncResult ar)
        {
            HttpListener httpServer = ar.AsyncState as HttpListener;
            httpServer.BeginGetContext(new AsyncCallback(getContextCallBack), httpServer);

            try
            {
                HttpListenerContext context = httpServer.EndGetContext(ar);
                router.Service(context.Request, context.Response);
            }
            catch (Exception err)
            {
                Console.WriteLine("Fatal:error handle client request:" + err.Message);
            }
        }
    }
}
