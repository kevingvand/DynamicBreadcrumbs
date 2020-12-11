using DynamicBreadcrumbs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicBreadcrumbs.Annotations
{
    public class ProjectNameResolverAttribute : ResolverAttribute
    {
        private ProjectService _projectService;

        public ProjectNameResolverAttribute(string resolverKey)
        {
            _projectService = new ProjectService();
            ResolverKey = resolverKey;
        }

        public override Dictionary<string, object> Resolve(object resolver)
        {
            var result = new Dictionary<string, object>();

            var projectId = Convert.ToInt32(resolver);
            var project = _projectService.GetProjectById(projectId);

            result.Add("projectName", project.Name);
            return result;
        }
    }
}