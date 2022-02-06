using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class FieldContainer : FieldContainerBase
    {
        [NotMapped]
        public MultilingualName Name { get; protected set; }
      
        [NotMapped]
        public MultilingualDescription Description { get; protected set; }

        public Guid? TemplateId
        {
            get => GetAttribute("template-id", null as Guid?);
            set => Data.SetAttributeValue("template-id", value);
        }

        public Guid? ParentId
        {
            get => GetAttribute("parent-id", null as Guid?);
            set => Data.SetAttributeValue("parent-id", value);
        }

        [NotMapped]
        public FieldContainerList Children { get; protected set; }

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

            Name = new MultilingualName(GetElement(MultilingualName.TagName, true));
            Description = new MultilingualDescription(GetElement(MultilingualDescription.TagName, true));
        }

        public string GetName(string lang)
        {
            if (string.IsNullOrEmpty(lang))
                return string.Join(" + ", Name.Values.Where(val => val.Value != null).Select(val => val.Value));
            else
            {
                Text val = Name.Values.Where(val => val.Language == lang).FirstOrDefault();
                return val != null ? val.Value : null;
            }
        }

        public void SetName(string containerName, string lang = null)
        {
            Name.SetContent(containerName, lang);
        }

        public string GetDescription(string lang)
        {
            Text val = Description.Values.Where(val => val.Language == lang).FirstOrDefault();
            return val != null ? val.Value : null;
        }

        public void SetDescription(string containerDescription, string lang = null)
        {
            Description.SetContent(containerDescription, lang);
        }
    }
}
