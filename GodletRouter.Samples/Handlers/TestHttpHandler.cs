using System.Net;

namespace GodletRouter.Samples.Handlers
{
    class TestHttpHandler : AbstractHttpHandler
    {
        protected override void doGet(HttpListenerRequest request, HttpListenerResponse response) {
            string responseString =
               @"<html>
                    <head><title>doGet</title></head>
                    <body><h2>test doGet</h2></body>
                  </html>";

            this.WriteString(response, responseString);
        }

        protected override void doPost(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responseString =
               @"<html>
                    <head><title>doPost</title></head>
                    <body><h2>test doPost</h2></body>
                  </html>";

            this.WriteString(response, responseString);
        }
    }
}
