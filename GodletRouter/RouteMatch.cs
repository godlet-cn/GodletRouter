using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GodletRouter
{
    class RouteMatch
    {
        public IHttpHandler Handler { get; internal set; }
    }
}
