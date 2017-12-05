using Catfish.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class EntityContentViewModel : KoBaseViewModel
    {
        public List<EntityViewModel> ChildEntityList{ get; set; }
        public int ChildOffset { get; set; }
        public int ChildCount { get; set; }

        public List<EntityViewModel> MasterEntityList { get; set; }
        public int MasterOffset { get; set; }
        public int MasterCount { get; set; }

        public int PageSize { get; set; }

        public List<EntityViewModel> SelectedMasterEntities { get; set; }
        public List<EntityViewModel> SelectedChildEntities { get; set; }

        public List<EntityViewModel> RemovalPendingChildEntities { get; set; }


        public EntityContentViewModel()
        {
            ChildEntityList = new List<EntityViewModel>();
            MasterEntityList = new List<EntityViewModel>();
            SelectedMasterEntities = new List<EntityViewModel>();
            SelectedChildEntities = new List<EntityViewModel>();
            RemovalPendingChildEntities = new List<EntityViewModel>();

            ChildOffset = 0;
            MasterOffset = 0;
            PageSize = Int32.MaxValue; // 25;
        }

        public void LoadNextChildrenSet(IEnumerable<Entity> src)
        {
            if (ChildCount == 0)
                ChildCount = src.Count();

            IEnumerable<Entity> elements = src.Skip(ChildOffset).Take(PageSize);
            foreach (Entity e in elements)
                ChildEntityList.Add(new EntityViewModel(e));
        }

        public void LoadNextMasterSet(IEnumerable<Entity> src)
        {
            if (MasterCount == 0)
                MasterCount = src.Count();

            IEnumerable<Entity> elements = src.Where(e => e.Id != Id).Skip(MasterOffset).Take(PageSize);
            foreach (Entity e in elements)
                MasterEntityList.Add(new EntityViewModel(e));
        }

        public void Associate()
        {
            foreach (EntityViewModel e in SelectedMasterEntities)
            {
                if (e.Id != this.Id && ChildEntityList.Where(c => c.Id == e.Id).Any() == false)
                    ChildEntityList.Add(e);
            }
            SelectedMasterEntities.Clear();
        }

        public void Disassociate()
        {
            foreach (EntityViewModel e in SelectedChildEntities)
            {
                EntityViewModel rem = ChildEntityList.Where(le => le.Id == e.Id).FirstOrDefault();
                if (rem != null)
                {
                    RemovalPendingChildEntities.Add(rem);
                    ChildEntityList.Remove(rem);
                }
            }
            SelectedChildEntities.Clear();
        }




        ////public void InitAssociatedEntitiesList(IEnumerable<Entity> entities)
        ////{
        ////    ChildEntities.Clear();
        ////    foreach (Entity e in entities)
        ////        ChildEntities.Add(new EntityViewModel(e));
        ////}

        ////public void InitAllEntitieLists(IEnumerable<Entity> entities)
        ////{
        ////    ForeignEntities.Clear();
        ////    foreach (Entity e in entities)
        ////        ForeignEntities.Add(new EntityViewModel(e));
        ////}

        public override void UpdateDataModel(object dataModel, CatfishDbContext db)
        {
            throw new NotImplementedException();
        }


    }
}