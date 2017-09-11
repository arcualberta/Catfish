using Catfish.Core.Models;
using Catfish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class EntityAssociationViewModel
    {
        public int ParentEntityId { get; set; }

        public List<EntityViewModel> AllEntities { get; set; }

        public List<EntityViewModel> AssociatedEntities { get; set; }

        public List<EntityViewModel> SelectedEntities { get; set; }

        public EntityAssociationViewModel()
        {
            AllEntities = new List<EntityViewModel>();
            AssociatedEntities = new List<EntityViewModel>();
            SelectedEntities = new List<EntityViewModel>();
        }

        public void Associate()
        {
            foreach (EntityViewModel e in SelectedEntities)
            {
                if (!AssociatedEntities.Where(le => le.Id == e.Id).Any())
                    AssociatedEntities.Add(e);
            }
            SelectedEntities.Clear();
        }

        public void Disassociate()
        {
            foreach (EntityViewModel e in SelectedEntities)
            {
                EntityViewModel rem = AssociatedEntities.Where(le => le.Id == e.Id).FirstOrDefault();
                if (rem != null)
                    AssociatedEntities.Remove(rem);
            }
            SelectedEntities.Clear();
        }

        public void InitAssociatedEntitiesList(IEnumerable<Entity> entities)
        {
            AssociatedEntities.Clear();
            foreach (Entity e in entities)
                AssociatedEntities.Add(new EntityViewModel(e));
        }

        public void InitAllEntitieLists(IEnumerable<Entity> entities)
        {
            AllEntities.Clear();
            foreach (Entity e in entities)
                AllEntities.Add(new EntityViewModel(e));
        }

    }
}