using Piranha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    interface ICatfishSiteService
    {
        public Task UpdatePageSettingsAsync(Guid siteId, string siteTypeId);
    }
}
