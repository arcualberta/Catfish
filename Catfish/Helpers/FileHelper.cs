using Catfish.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Catfish.Helpers
{
    public class FileHelper
    {
        public static string GetThumbnailRoot(HttpRequestBase request)
        {
            return request.RequestContext.HttpContext.Server.MapPath("~/Content/Thumbnails");
        }

        public static string GetBaseURL(HttpRequestBase request)
        {
            System.Web.Mvc.UrlHelper url = new System.Web.Mvc.UrlHelper(request.RequestContext);
            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, url.Content("~"));
        }

        public static string GetBrokenLinkThumbnail(HttpRequestBase request)
        {
            return GetBaseURL(request) + "/Content/Thumbnails/404.png";
        }

        public static List<string> GetGuidCache(HttpSessionStateBase session)
        {
            List<string> guidStore = session["GuidCache"] as List<string>;
            if (guidStore == null)
                session["GuidCache"] = guidStore = new List<string>();
            return guidStore;
        }

        public static void CacheGuids(HttpSessionStateBase session, List<DataFile> files)
        {
            foreach (var f in files)
                GetGuidCache(session).Add(f.Guid);
        }

        public static bool CheckGuidCache(HttpSessionStateBase session, string guid)
        {
            return GetGuidCache(session).Contains(guid);
        }
    }
}