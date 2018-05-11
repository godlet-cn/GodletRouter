# GodletRouter

## Introduction ##
GodletRouter is a HTTP request multiplexer. It matches the URL of each incoming request against a list of registered patterns and calls the handler for the pattern that most closely matches the URL.

GodletRouter provides a class `HttpServer`, which works over `HttpListener`. Clients can embed a `HttpServer` in their applications, which can be easily called by a browser or a curl client.

## Samples ##

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
            server.AddHandler("/test", new TestHttpHandler());
            server.AddHandler("/test/child", new TestChildHttpHandler());

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

    //How to implement a http handler
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

When you run this application, you can try it now:


1. Visit the home page

    curl http://localhost:4006


2. Test "Get" method 

    curl http://localhost:4006/test

3. Test "Post" method

    curl -X POST http://localhost:4006/test -d 'FOO'

4. Test a subsidiary url request

    curl http://localhost:4006/test/child

5. Visit a unknown page
    
    curl http://localhost:4006/db