using Catfish.Core.Models;
using SolrNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services.Solr
{
    public interface ISolrIndexService<T>
    {
        bool AddUpdate(T document);
        bool Delete(T document);
    }
}
