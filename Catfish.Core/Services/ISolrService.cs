using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Services
{
    public interface ISolrService
    {
        public void Index(Entity entity);
        public void Index(List<SolrDoc> docs);
        public void Commit();
    }
}
