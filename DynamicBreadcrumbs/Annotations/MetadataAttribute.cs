using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicBreadcrumbs.Annotations
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public abstract class MetadataAttribute : ActionFilterAttribute
    {
        public abstract void Process(ModelMetadata modelMetadata);
    }
}