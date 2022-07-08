using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class MultilingualName : MultilingualText
    {
        public static readonly string TagName = "name";
        public MultilingualName() : base(TagName) { }
        public MultilingualName(XElement data) : base(data) { }
    }
}
