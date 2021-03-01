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
        public Guid Current
        {
            get => Guid.Parse(Data.Attribute(CurrentAtt).Value);
            set => SetAttribute(CurrentAtt, value);
        }

        public Guid Next
        {
            get => Guid.Parse(Data.Attribute(NextAtt).Value);
            set => SetAttribute(NextAtt, value);
        }

        public string ButtonLabel
        {
            get => GetAttribute(LableAtt, null as string);
            set => SetAttribute(LableAtt, value);
        }

        public string Condition
        {
            get => Data.Value;
            set => Data.Value = value;
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
