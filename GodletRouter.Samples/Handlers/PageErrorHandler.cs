using System.Net;

namespace GodletRouter.Samples.Handlers
{
    class PageErrorHandler : AbstractHttpHandler
    {
        public override void Service(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responseString =
                @"<html>
                    <head><title>404</title></head>
                    <body><h2>Page not found</h2></body>
                  </html>";

            this.WriteString(response, responseString);
        }
    }
}
