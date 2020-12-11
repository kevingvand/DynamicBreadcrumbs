using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicBreadcrumbs.Models
{
    public class Domain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Project Project { get; set; }

        public Domain(int id, string name, Project project)
        {
            this.Id = id;
            this.Name = name;
            this.Project = project;
        }
    }
}