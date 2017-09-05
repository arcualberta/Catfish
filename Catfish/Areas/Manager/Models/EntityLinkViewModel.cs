using Catfish.Core.Models;
using Catfish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models
{
    public class EntityLinkViewModel
    {
        public EntityViewModel ActiveItem { get; set; }

        public List<EntityViewModel> AllEntities { get; set; }

        public List<EntityViewModel> LinkedEntities { get; set; }

        public List<EntityViewModel> SelectedEntities { get; set; }

        public EntityLinkViewModel()
        {
            AllEntities = new List<EntityViewModel>();
            LinkedEntities = new List<EntityViewModel>();
            SelectedEntities = new List<EntityViewModel>();
        }

        public void Link()
        {
            foreach (EntityViewModel e in SelectedEntities)
            {
                if (!LinkedEntities.Where(le => le.Id == e.Id).Any())
                    LinkedEntities.Add(e);
            }
            SelectedEntities.Clear();
        }

        public void Unlink()
        {
            foreach (EntityViewModel e in SelectedEntities)
            {
                EntityViewModel rem = LinkedEntities.Where(le => le.Id == e.Id).FirstOrDefault();
                if (rem != null)
                    LinkedEntities.Remove(rem);
            }
            SelectedEntities.Clear();
        }

        public void SetLinkedEntities(IEnumerable<Entity> entities)
        {
            foreach (Entity e in entities)
                LinkedEntities.Add(new EntityViewModel(e));
        }

    }
}