using Catfish.Core.Models.Contents.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class CompositeField : BaseField
    {
        //We set the ChildTemplate to be the template of the form that should
        //be renderd as each child. The front-end can use this template to
        //add more children dynamically but it does not require to let the user
        //input data to the ChildTemplate
        public DataItem ChildTemplate 
        { 
            get
            {
                var childTemplateContainer = GetElement("child-template", false);
                if(childTemplateContainer != null)
                {
                    var dataItemXml = childTemplateContainer.Element(DataItem.TagName);
                    if (dataItemXml != null)
                        return new DataItem(dataItemXml);
                }
                return null;
            }
            set
            {
                var childTemplateContainer = GetElement("child-template", true);
                var dataItemXml = childTemplateContainer.Element(DataItem.TagName);
                if (dataItemXml != null)
                    dataItemXml.Remove();
                childTemplateContainer.Add(value.Data);
            }
        }
        
        public XmlModelList<DataItem> Children { get; set; }

        public int Min 
        {
            get => GetAttribute("min", 1);
            set => SetAttribute("min", value);
        }

        public CompositeField() { DisplayLabel = "Composite Field"; }
        public CompositeField(XElement data) : base(data) { DisplayLabel = "Composite Field"; }
        public CompositeField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Composite Field"; }

        public void CreateChildTemplate(string name, string description, string lang)
        {
            
            ChildTemplate = new DataItem();
            ChildTemplate.SetName(name, lang);
            ChildTemplate.SetDescription(description, lang);

            Initialize(eGuidOption.Ignore);
        }
        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            Children = new XmlModelList<DataItem>(GetElement("children", true), true, DataItem.TagName);
        }

        public DataItem AddChild()
        {
            DataItem clone = ChildTemplate.Clone() as DataItem;
            clone.TemplateId = ChildTemplate.Id;
            clone.Id = Guid.NewGuid();
            Children.Add(clone);
            return clone;
        }
        public void RemoveChild(Guid childId)
        {
            var child = Children.Find(childId);
            Children.Remove(child);
        }

        public void InsertChildren()
        {
            for (var i = Children.Count; i < Min; ++i)
                AddChild();
        }




    }
}
