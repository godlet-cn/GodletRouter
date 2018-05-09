using System;
using System.Collections.Generic;

namespace GodletRouter
{
    // RouteMatch stores information about a matched route.
    public class RouteMatch
    {
        public Route Route { get; set; }

        public IHttpHandler Handler { get; set; }

        public Dictionary<string, string> Vars { get; set; }

        public Exception MatchErr { get; set; }
    }
}
