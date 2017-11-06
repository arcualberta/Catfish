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
        public static Language GetActiveLanguage(HttpSessionStateBase session)
        {
            if (session["ActiveLanguage"] as Language == null)
                session["ActiveLanguage"] = ConfigHelper.Languages[0];

            return session["ActiveLanguage"] as Language;
        }
    }
}