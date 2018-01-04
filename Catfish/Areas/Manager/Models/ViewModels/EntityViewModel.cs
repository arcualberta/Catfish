using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class EntityViewModel
    {
        public int Id { get; set; }
        public string Label { get; set; }

        public EntityViewModel()
        {

        }

        public EntityViewModel(Entity src)
        {
            Id = src.Id;
            Label = src.GetName();
        }

        public EntityViewModel(MetadataSet src)
        {
            Id = src.Id;
            Label = src.GetName();
        }

    }
}