using Catfish.Core.Models.Solr;
using Piranha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    interface IPageIndexingService
    {
        public void IndexPage(PageBase page);
        public void IndexPost(PostBase post);
    }
}
