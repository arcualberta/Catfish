using Catfish.Core.Models;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Catfish.Models.Fields
{
    public enum EType {None, Collection, ItemTemplate, Item }
    [FieldType(Name = "DropDownList", Component = "dropdownlist-field")]
    public class CatfishSelectList<T>: IField where T : Entity,  new() 
    {
        public List<SelectListItem> Entries { get; set; }
      
        public string GetTitle()
        {
            throw new NotImplementedException();
        }
     
        public void Init(AppDbContext db, IEntityService es )
        {    
            if(es == null)
            {
              
                EntityService entitySrv = new EntityService(db);
                es = entitySrv;
            
            }
            var selection = es.GetEntities<T>();
            Entries = selection.Select(s => new SelectListItem() { Text = s.Name.GetContent(null), Value = s.Id.ToString() }).ToList();
          
        }

    }
}
