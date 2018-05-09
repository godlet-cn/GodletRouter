using GodletRouter.Samples.Handlers;
using GodletRouter.Samples.MiddleWares;
using System;

namespace GodletRouter.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer server = new HttpServer();
            server.Start("localhost", 4006);

            //add MiddleWares
            server.AddMiddleWares(new LogMiddleWare());

            //set common page handlers
            server.SetHomePageHandler(new HomeHttpHandler());
            server.SetPageErrorHandler(new PageErrorHandler());

            //dispatch urls to handlers
            server.AddHandler("/test", new TestGetHttpHandler(),"Get");
            server.AddHandler("/test", new TestPostHttpHandler(), "Post");
            server.AddHandler("/test/child", new TestChildHttpHandler(), "Get");

            // wait for system exit
            while (true)
            {
                string text = Console.ReadLine();
                if (text == "exit" || text == "quit")
                {
                    server.Stop();
                    break;
                }
                Console.WriteLine("please type 'exit' or 'quit' to exit this application...");
            }
        }
    }
}
