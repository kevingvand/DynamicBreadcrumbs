using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicBreadcrumbs.Annotations
{
    public abstract class ResolverAttribute : ActionFilterAttribute
    {
        public string ResolverKey { get; set; }
        public abstract Dictionary<string, object> Resolve(object resolver);
    }
}