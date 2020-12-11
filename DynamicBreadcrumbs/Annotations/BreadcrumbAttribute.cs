using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace DynamicBreadcrumbs.Annotations
{
    public class BreadcrumbAttribute : FilterAttribute
    {
        public string Text { get; set; }
        public string Url { get; set; }

        private Type ParentController { get; set; }
        private string ParentAction { get; set; }

        private Type[] ParentActionAttributes { get; set; }

        public BreadcrumbAttribute(string text)
        {
            this.Text = text;
        }

        public BreadcrumbAttribute(string text, string parentAction)
            : this(text)
        {
            this.ParentAction = parentAction;
        }

        public BreadcrumbAttribute(string text, Type parentController, string parentAction)
            : this(text, parentAction)
        {
            this.ParentController = parentController;
        }

        public BreadcrumbAttribute(string text, string url, Type parentController, string parentAction, params Type[] parentActionArguments)
            : this(text, parentController, parentAction)
        {
            this.ParentActionAttributes = parentActionArguments;
        }

        public override void Process(ControllerContext controllerContext, ActionDescriptor actionDescriptor, Dictionary<string, object> resolvedAttributes)
        {
            //TODO: additional parameters over attributes (resolve using resolverAttributes)
            foreach (var routeValue in controllerContext.RouteData.Values)
                resolvedAttributes.Add(routeValue.Key, routeValue.Value);

            var thisBreadcrumb = new BreadcrumbAttribute(this.Text, this.Url, this.ParentController, this.ParentAction, this.ParentActionAttributes);

            if (thisBreadcrumb.Url == null)
                thisBreadcrumb.Url = ((System.Web.Routing.Route)controllerContext.RouteData.Route)?.Url ?? string.Empty;

            thisBreadcrumb.Url = ApplyAttributes(thisBreadcrumb.Url, resolvedAttributes);
            thisBreadcrumb.Text = ApplyAttributes(thisBreadcrumb.Text, resolvedAttributes);

            if (ParentController == null) thisBreadcrumb.ParentController = controllerContext.Controller.GetType();

            var breadCrumbs = new List<BreadcrumbAttribute>();
            breadCrumbs.Add(thisBreadcrumb);

            BreadcrumbAttribute parentBreadcrumb = thisBreadcrumb;

            while (true)
            {
                parentBreadcrumb = parentBreadcrumb.GetParent(parentBreadcrumb.ParentController, resolvedAttributes);
                if (parentBreadcrumb == null) break;
                breadCrumbs.Add(parentBreadcrumb);
            }

            controllerContext.HttpContext.Items.Add("Breadcrumbs", breadCrumbs);
        }

        public BreadcrumbAttribute GetParent(Type defaultController, Dictionary<string, object> attributeParameters)
        {
            if (ParentAction == null) return null;

            MethodBase parentActionMethod = ParentController.GetMethod(ParentAction);
            var parentAttribute = parentActionMethod.GetCustomAttribute<BreadcrumbAttribute>();

            var resolveAttributes = parentActionMethod.GetCustomAttributes<ResolverAttribute>();
            resolveAttributes.ToList().ForEach(attribute =>
            {
                var resolver = attributeParameters[attribute.ResolverKey];
                attribute.Resolve(resolver).ToList().ForEach(resolvedAttribute =>
                {
                    attributeParameters.Add(resolvedAttribute.Key, resolvedAttribute.Value);
                });
            });

            //TODO: add additional arguments over attributes

            if (parentAttribute.ParentController == null) parentAttribute.ParentController = defaultController;

            if (parentAttribute.Url == null)
            {
                var prefixAttribute = parentAttribute.ParentController.GetCustomAttribute<RoutePrefixAttribute>();
                var routeAttribute = parentActionMethod.GetCustomAttribute<RouteAttribute>();

                //TODO: auto-resolve using default routing (controller + action name)?
                var prefix = prefixAttribute?.Prefix ?? string.Empty;
                var route = routeAttribute?.Template ?? string.Empty;

                parentAttribute.Url = prefix + "/" + route;
                parentAttribute.Url = parentAttribute.ApplyAttributes(parentAttribute.Url, attributeParameters);
            }

            parentAttribute.Text = parentAttribute.ApplyAttributes(parentAttribute.Text, attributeParameters);

            return parentAttribute;
        }
        private string ApplyAttributes(string template, Dictionary<string, object> attributeParameters)
        {
            foreach (var key in attributeParameters.Keys)
                template = template.Replace($"{{{key}}}", attributeParameters[key].ToString());

            return template;
        }
    }
}