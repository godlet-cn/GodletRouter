namespace GodletRouter
{
    public class Route
    {
        private IHttpHandler handler;
        private string pattern;

        public Route(string pattern, IHttpHandler handler)
        {
            this.pattern = pattern;
            this.handler = handler;
        }

        public IHttpHandler Handler
        {
            get
            {
                return handler;
            }
        }

        public string Pattern
        {
            get {
                return this.pattern;
            }
        }
    }
}
