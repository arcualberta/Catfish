using Catfish.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Collections;

namespace Catfish.Services
{
    public interface IEntityService
    {
        public IEnumerable GetCollections();
        public IEnumerable GetItemTemplates();
        public List<SelectListItem> GetListOfCollectionNames();
        public List<SelectListItem> GetListOfItemTemplateNames();
        public IQueryable<T> GetEntities<T>() where T : Entity;
    }

    public class EntityService : IEntityService
    {

        private readonly AppDbContext _db;
        public EntityService(AppDbContext db)
        {
            _db = db;
        }
        public IQueryable<T> GetEntities<T>() where T: Entity
        {
            return _db.Entities.Where(d => d is T).Select(d => d as T);
        }
        public IEnumerable GetCollections()
        {
          

            return _db.Collections.ToList();
        }
        public IEnumerable GetItemTemplates()
        {
         
            return _db.ItemTemplates.ToList();
        }

        public List<SelectListItem> GetListOfCollectionNames()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach(Collection c in GetCollections())
            {
                string name = c.Name.GetContent();
                SelectListItem li = new SelectListItem { Text = name, Value = c.Id.ToString() };
                items.Add(li);
            }
            
            return items;
        }

        public List<SelectListItem> GetListOfItemTemplateNames()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (ItemTemplate c in GetItemTemplates())
            {
                string name = c.Name.GetContent();
                SelectListItem li = new SelectListItem { Text = name, Value = c.Id.ToString() };
                items.Add(li);
            }
            
            return items;
        }
    }
}
