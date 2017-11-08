using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Data.Entity;
using System.Threading;
using System.Globalization;
using Catfish.Models.Regions;
using System.Web.Script.Serialization;

namespace Catfish.Helpers
{
    public static class ViewHelper
    {
        public static CultureInfo GetActiveLanguage(HttpSessionStateBase session)
        {
            if (session["ActiveLanguage"] as CultureInfo == null)
                session["ActiveLanguage"] = ConfigHelper.Languages[0];

            return session["ActiveLanguage"] as CultureInfo;
        }

        public static void SetActiveLanguage(CultureInfo lang, HttpSessionStateBase session)
        {
            session["ActiveLanguage"] = lang;
        }

        public static void MultilingualMenuRenderer(Piranha.Web.UIHelper ui, System.Text.StringBuilder str, Piranha.Models.Sitemap page)
        {
            string menuItemText = null;
            try
            {
                CultureInfo culture = Thread.CurrentThread.CurrentCulture;

                var db = new Piranha.DataContext();
                var pageTitleRegion = db.Regions
                    .Include(r => r.RegionTemplate)
                    .Where(r => r.PageId == page.Id && r.RegionTemplate.InternalId == "PageTitle")
                    .FirstOrDefault();

                MultilingualText pageTitle = new JavaScriptSerializer().Deserialize<MultilingualText>(pageTitleRegion.InternalBody);
                menuItemText = pageTitle.GetContent(culture.TwoLetterISOLanguageName);
            }
            catch (Exception)
            {
            }

            if (string.IsNullOrEmpty(menuItemText))
                menuItemText = string.IsNullOrEmpty(page.NavigationTitle) ? page.Title : page.NavigationTitle;

            var url = ui.AbsoluteUrl("/home/" + page.Permalink);
            str.Append("<a href='" + url + "'>" + menuItemText + "</a>");

            //var regions = pageModel.Regions;
            //MultilingualText pageTitle = null;

        }

    }
}