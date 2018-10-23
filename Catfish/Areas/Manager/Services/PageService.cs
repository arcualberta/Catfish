using Piranha;
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
           var page = Piranha.Models.Page.GetSingle(new Guid(strGuid));

            return page;
        }

        public Page GetPageByPermalink(string permalink)
        {
            var page = Piranha.Models.Page.GetByPermalink(permalink);//       Piranha.Models.Page.GetByPermalink(permalink);
            return page;
        }
        
    }
}