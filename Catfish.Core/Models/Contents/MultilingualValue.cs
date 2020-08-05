using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class MultilingualValue : MultilingualText
    {
        public static readonly string TagName = "value";
        public MultilingualValue() : base(TagName) { }
        public MultilingualValue(XElement data) : base(data) { }
    }
}
