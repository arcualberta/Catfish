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

        public static string GetBrokenLinkThumbnail(HttpRequestBase request)
        {
            return Path.Combine(GetThumbnailRoot(request), "broken.png");
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
                GetGuidCache(session).Add(f.GuidName);
        }

        public static bool CheckGuidCache(HttpSessionStateBase session, string guidName)
        {
            return GetGuidCache(session).Contains(guidName);
        }
    }
}