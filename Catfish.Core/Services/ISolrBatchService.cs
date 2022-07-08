using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Services
{
    public interface ISolrBatchService
    {
        public void IndexItems(bool forceReindexingAll);
    }
}
