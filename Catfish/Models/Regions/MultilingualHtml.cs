using Catfish.Core.Models;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "MultilingualHtml")]
    [ExportMetadata("Name", "Multilingual Html")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class MultilingualHtml : CatfishRegion
    {
        public List<TextValue> Content { get; set; }

        public MultilingualHtml()
        {
            Content = new List<TextValue>();
        }

        public string GetContent(string langCode)
        {
            return Content.Where(c => c.LanguageCode == langCode).Select(c => c.Value).FirstOrDefault();
        }
    }
}