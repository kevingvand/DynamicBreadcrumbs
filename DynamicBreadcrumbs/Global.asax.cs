﻿using DynamicBreadcrumbs.App_Start;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DynamicBreadcrumbs
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelMetadataProviders.Current = new App_Start.ModelMetadataProvider();
            FilterProviders.Providers.Add(new FilterAttributeProvider());
        }
    }
}
