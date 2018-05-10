using System.IO;
using System.Net;
using System.Text;

namespace GodletRouter.Samples.Handlers
{
    class TestChildHttpHandler : AbstractHttpHandler
    {
        public override void Service(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responseString =
               @"<html>
                    <head><title>TestChildHttpHandler</title></head>
                    <body><h2>TestChildHttpHandler</h2></body>
                  </html>";

            this.WriteString(response, responseString);
        }
    }
}
