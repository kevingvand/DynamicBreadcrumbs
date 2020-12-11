using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicBreadcrumbs.App_Start
{
    public class FilterAttributeProvider : FilterAttributeFilterProvider
    {
        protected override IEnumerable<FilterAttribute> GetActionAttributes(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetActionAttributes(controllerContext, actionDescriptor);

            var resolvedAttributes = new Dictionary<string, object>();

            attributes.OfType<Annotations.ResolverAttribute>().ToList().ForEach(attribute =>
            {
                var resolver = controllerContext.RouteData.Values[attribute.ResolverKey];
                attribute.Resolve(resolver).ToList().ForEach(resolvedAttribute =>
                {
                    resolvedAttributes.Add(resolvedAttribute.Key, resolvedAttribute.Value);
                });
            });

            attributes.OfType<Annotations.FilterAttribute>().ToList().ForEach(attribute => attribute.Process(controllerContext, actionDescriptor, resolvedAttributes));

            return attributes;
        }
    }
}