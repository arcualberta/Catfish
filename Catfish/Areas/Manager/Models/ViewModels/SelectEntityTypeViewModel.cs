using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Catfish.Core.Models;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class SelectEntityTypeViewModel : KoBaseViewModel
    {
        public List<Catfish.Core.Models.EntityType> EntityTypes { get; set; }
        public Catfish.Core.Models.EntityType SelectedEntityType { get; set; }

        public override void UpdateDataModel(object dataModel, CatfishDbContext db)
        {
            
        }
    }
}