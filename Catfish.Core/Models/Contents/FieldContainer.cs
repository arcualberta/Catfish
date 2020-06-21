using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class FieldContainer : XmlModel
    {
        public static readonly string FieldContainerTag = "fields";

        [NotMapped]
        public MultilingualText Name { get; protected set; }

        [NotMapped]
        public MultilingualText Description { get; protected set; }

        [NotMapped]
        public XmlModelList<BaseField> Fields { get; set; }

        public FieldContainer(string tagName) : base(tagName) 
        { 
            Initialize(eGuidOption.Ignore); 
            Created = DateTime.Now; 
        }
        public FieldContainer(XElement data) : base(data) 
        {
            Initialize(eGuidOption.Ignore); 
        }
        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            Name = new MultilingualText(GetElement(Entity.NameTag, true));
            Description = new MultilingualText(GetElement(Entity.DescriptionTag, true));

            Fields = new XmlModelList<BaseField>(GetElement(FieldContainerTag, true), true, BaseField.FieldTagName);
        }

        public BaseField GetFieldByName(string fieldName, string lang)
        {
            foreach (var field in Fields)
            {
                if (field.Name.Values.Where(v => v.Language == lang && v.Value == fieldName) != null)
                    return field;
            }
            return null;
        }
    }
}
