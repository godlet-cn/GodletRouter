using System;

namespace GodletRouter.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer server = new HttpServer();
            server.Start();
            
            // Print system exit method
            while (true)
            {
                string str = Console.ReadLine();
                if (str == "exit" || str == "quit")
                {
                    server.Stop();
                    break;
                }
                Console.WriteLine("please type 'exit' or 'quit' to exit this application...");
            }
        }
    }
}
