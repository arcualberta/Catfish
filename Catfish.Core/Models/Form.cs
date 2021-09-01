using Catfish.Core.Models.Contents.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    [Table("Catfish_Forms")]
    public class Form: FieldContainer
    {
        public static readonly string TagName = "form";
        public string FormName { get; set; }
        public Form() : base(TagName) { Initialize(eGuidOption.Ensure); }
        public Form(XElement data) : base(data) { Initialize(eGuidOption.Ensure); }

        public DataItem ToDataItem()
        {
            DataItem dataItem = new DataItem();
            XElement clone = new XElement(Data);
            clone.Name = dataItem.Data.Name;
            foreach(var att in dataItem.Data.Attributes())
                clone.SetAttributeValue(att.Name, att.Value);

            dataItem = new DataItem(clone);

            return dataItem;
        }
    }
}
