using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicBreadcrumbs.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Domain> Domains {get; set;}

        public Project(int id, string name)
        {
            Id = id;
            Name = name;
            Domains = new List<Domain>();
        }
    }
}