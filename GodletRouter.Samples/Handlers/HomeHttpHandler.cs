using System.Net;

namespace GodletRouter.Samples.Handlers
{
    class HomeHttpHandler : AbstractHttpHandler
    {
        public override void Service(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responseString =
                @"<html>
                    <head><title>Welcome</title></head>
                    <body><h2>Thank you for using GodletRouter</h2></body>
                  </html>";

            this.WriteString(response,responseString);
        }
    }
}
