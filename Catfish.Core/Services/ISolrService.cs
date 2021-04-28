using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services
{
    public interface ISolrService
    {
        public void Index(Entity entity);
        public void Commit();
    }
}
