using DynamicBreadcrumbs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicBreadcrumbs.Annotations
{
    public class DomainNameResolverAttribute : ResolverAttribute
    {
        private ProjectService _projectService;

        public DomainNameResolverAttribute(string resolverKey)
        {
            _projectService = new ProjectService();
            ResolverKey = resolverKey;
        }

        public override Dictionary<string, object> Resolve(object resolver)
        {
            var result = new Dictionary<string, object>();

            int domainId = Convert.ToInt32(resolver);

            var domain = _projectService.GetDomainById(domainId);

            result.Add("domainName", domain.Name);
            return result;
        }
    }
}