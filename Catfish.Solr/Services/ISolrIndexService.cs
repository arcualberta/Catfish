using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Solr
{
    public interface ISolrIndexService<T>
    {
        bool AddUpdate(T document);
        bool Delete(T document);
    }
}
