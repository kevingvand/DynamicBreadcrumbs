using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicBreadcrumbs.Annotations
{
    public abstract class FilterAttribute : ActionFilterAttribute
    {
        public abstract void Process(ControllerContext controllerContext, ActionDescriptor actionDescriptor, Dictionary<string, object> resolvedAttributes);
    }
}