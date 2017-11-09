using Catfish.Core.Models;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "MultilingualText")]
    [ExportMetadata("Name", "Multilingual Text")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class MultilingualText: CatfishRegion
    {
        public List<TextValue> Content { get; set; }

        public MultilingualText()
        {
            Content = new List<TextValue>();
        }

        public string GetContent(string langCode)
        {
            return Content.Where(c => c.LanguageCode == langCode).Select(c => c.Value).FirstOrDefault();
        }
    }
}