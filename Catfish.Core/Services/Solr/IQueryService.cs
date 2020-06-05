using Catfish.Core.Models.Solr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services.Solr
{
    interface IQueryService
    {
        List<SolrItemModel> SimpleQueryByField(string fieldname, string matchword);
    }
}
