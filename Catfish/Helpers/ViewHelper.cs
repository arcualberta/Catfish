using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Data.Entity;
using System.Threading;
using System.Globalization;

namespace Catfish.Helpers
{
    public static class ViewHelper
    {
        public static Language GetActiveLanguage(HttpSessionStateBase session)
        {
            if (session["ActiveLanguage"] as Language == null)
                session["ActiveLanguage"] = ConfigHelper.Languages[0];

            return session["ActiveLanguage"] as Language;
        }

        public static void SetActiveLanguage(Language lang, HttpSessionStateBase session)
        {
            session["ActiveLanguage"] = lang;
        }

        public static void MultilingualMenuRenderer(Piranha.Web.UIHelper ui, System.Text.StringBuilder str, Piranha.Models.Sitemap page)
        {
            var db = new Piranha.DataContext();

            var pageTitleRegion = db.Regions
                .Include(r => r.RegionTemplate)
                .Where(r => r.PageId == page.Id && r.RegionTemplate.InternalId == "PageTitle")
                .FirstOrDefault();

            //MultilingualText pageTitle = new JavaScriptSerializer().Deserialize<MultilingualText>(pageTitleRegion.InternalBody);
            //menuItemText = pageTitle.GetContent(ViewHelper.GetActiveLanguage(Session))
            //str.Append(string.IsNullOrEmpty(pageTitle.) ? page.Title : page.NavigationTitle);


            //var regions = pageModel.Regions;
            //MultilingualText pageTitle = null;

        }

    }
}