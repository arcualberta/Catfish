using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class MultilingualDescription : MultilingualText
    {
        public static readonly string TagName = "description";
        public MultilingualDescription() : base(TagName) { }
        public MultilingualDescription(XElement data) : base(data) { }
    }
}
