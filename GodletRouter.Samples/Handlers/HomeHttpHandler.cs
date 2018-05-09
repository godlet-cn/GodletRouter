using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GodletRouter.Samples.Handlers
{
    class HomeHttpHandler : IHttpHandler
    {
        public void Service(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responseString =
                @"<html>
                    <head><title>Welcome</title></head>
                    <body><h2>Thank you for using GodletRouter</h2></body>
                  </html>";

            response.ContentLength64 = Encoding.UTF8.GetByteCount(responseString);
            response.ContentType = "text/html; charset=UTF-8";
            Stream output = response.OutputStream;
            StreamWriter writer = new StreamWriter(output);
            writer.Write(responseString);
            writer.Close();
        }
    }
}
