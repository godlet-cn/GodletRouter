using System;

namespace GodletRouter.Samples.MiddleWares
{
    class LogMiddleWare : IMiddleWare
    {
        public IHttpHandler Middleware(IHttpHandler handler)
        {
            Console.WriteLine("Write some log for this handler");
            return handler;
        }
    }
}
