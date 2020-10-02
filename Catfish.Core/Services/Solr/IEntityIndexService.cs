using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services.Solr
{
    public interface IEntityIndexService
    {
        public bool AddUpdateEntity(Entity entity);
    }
}
