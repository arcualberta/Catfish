using Piranha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Services
{
    public class PageService
    {
        public Page GetAPage(string strGuid)
        {
            return Piranha.Models.Page.GetSingle(new Guid(strGuid));
        }

        public Page GetPageByPermalink(string permalink)
        {
            return Piranha.Models.Page.GetByPermalink(permalink);
        }

        public List<Page> GetPages(Piranha.Data.Params param)
        {
            return Piranha.Models.Page.Get(param);
        }
    }
}