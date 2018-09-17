using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class EntityAccessDefinitionsViewModel
    {
        public int Id { get; set; } //collection/item id

        public string User { get; set; }

        public bool BlockInheritance { get; set; }

        public string EntityName { get; set; }

        
        public List<AccessGroup> SelectedAccessGroups { get; set; }

       // public SelectList AvailableUsers { get; set; }

        public Dictionary<string,string> AvailableUsers2 { get; set; }
        public SelectList AvailableAccessDefinitions { get; set; }

        public string SelectedAccessDefinition { get; set; }
        public IEnumerable<SelectListItem> AvailableAccessDefinitions2 { get; set; }

       
        public EntityAccessDefinitionsViewModel()
        {
            SelectedAccessGroups = new List<AccessGroup>();
            AvailableUsers2 = new Dictionary<string, string>();
          
        }
        public void UpdateModel(CFEntity entity)
        {
            Id = entity.Id;
            EntityName = entity.Name;
        }

    }

    public class AccessGroup
    {
        public string userId { get; set; } //guid
        public string User { get; set; }
        public string AccessMode { get; set; }
        public int AccessModesNum { get; set; }
    }

    
}