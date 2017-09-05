using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models
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
    }
}