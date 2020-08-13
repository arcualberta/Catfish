using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class Mapping : XmlModel
    {
        public static readonly string TagName = "mapping";
        public static readonly string CurrentAtt = "current";
        public static readonly string NextAtt = "next";
        public static readonly string LableAtt = "button-label";
        public string Current
        {
            get => GetAttribute(CurrentAtt, null as string);
            set => SetAttribute(CurrentAtt, value);
        }

        public string Next
        {
            get => GetAttribute(NextAtt, null as string);
            set => SetAttribute(NextAtt, value);
        }

        public string ButtonLabel
        {
            get => GetAttribute(LableAtt, null as string);
            set => SetAttribute(LableAtt, value);
        }

        public Mapping(XElement data)
            : base(data)
        {

        }
        public Mapping()
            : base(new XElement(TagName))
        {

        }


    }
}
