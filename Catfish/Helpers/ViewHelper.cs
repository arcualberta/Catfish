using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Catfish.Helpers
{
    public static class ViewHelper
    {
        public static string GetActiveLanguage(HttpSessionStateBase session)
        {
            var lang = session["ActiveLanguage"] as string;
            return string.IsNullOrEmpty(lang) ? ConfigHelper.DefaultLanguage : lang;
        }
    }
}