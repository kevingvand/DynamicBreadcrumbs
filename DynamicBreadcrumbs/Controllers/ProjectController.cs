using DynamicBreadcrumbs.Annotations;
using DynamicBreadcrumbs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace DynamicBreadcrumbs.Controllers
{
    [RoutePrefix("Projects")]
    public class ProjectController : Controller
    {
        ProjectService _projectService;
        public ProjectController()
        {
            _projectService = new ProjectService();
        }

        // GET: Project
        [Route("")]
        [Breadcrumb("Projects")]
        public ActionResult Index()
        {
            return View(_projectService.GetAllProjects());
        }

        [Route("{projectId}")]
        [ProjectNameResolver("projectId")]
        [Breadcrumb("{projectName}", "Index")]
        public ActionResult Project(int projectId)
        {
            var project = _projectService.GetProjectById(projectId);
            return View(project);
        }

        [Route("{projectId}/{domainId}")]
        [DomainNameResolver("domainId")]
        [Breadcrumb("{domainName}", "Project")]
        public ActionResult Domain(int projectId, int domainId)
        {
            var domain = _projectService.GetDomainByProjectAndId(projectId, domainId);
            return View(domain);
        }

        /*
         * TODO: 
         *  - Check if you can read other attributes from within an attribute
         *      - Read the route and route prefix attributes and use those values for the url
         *  - Add custom values to the link text
         *  
         */
    }
}