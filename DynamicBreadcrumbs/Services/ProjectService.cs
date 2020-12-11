
using DynamicBreadcrumbs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicBreadcrumbs.Services
{
    public class ProjectService
    {
        public List<Project> Projects { get; set; }

        public ProjectService()
        {
            var projectA = new Project(1, "Project A");
            var domainA = new Domain(1, "Domain A", projectA);
            var domainB = new Domain(2, "Domain B", projectA);
            var domainC = new Domain(3, "Domain C", projectA);

            projectA.Domains.Add(domainA);
            projectA.Domains.Add(domainB);
            projectA.Domains.Add(domainC);

            var projectB = new Project(2, "Project B");
            var domainD = new Domain(4, "Domain D", projectB);
            var domainE = new Domain(5, "Domain E", projectB);

            projectB.Domains.Add(domainD);
            projectB.Domains.Add(domainE);

            Projects = new List<Project>
            {
                projectA,
                projectB
            };
        }

        public List<Project> GetAllProjects()
        {
            return Projects;
        }

        public Project GetProjectById(int id)
        {
            return Projects.SingleOrDefault(project => project.Id == id);
        }

        public Domain GetDomainById(int id)
        {
            foreach(var project in Projects)
            {
                var domain = project.Domains.SingleOrDefault(projectDomain => projectDomain.Id == id);
                if (domain != null) return domain;
            }

            return null;
        }

        public Domain GetDomainByProjectAndId(int projectId, int id)
        {
            var project = GetProjectById(projectId);
            return project.Domains.SingleOrDefault(domain => domain.Id == id);
        }
    }
}