using Catfish.Core.Models.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    public class Item : Aggregation
    {
        public virtual ICollection<Aggregation> ParentRelations { get; set; }

        public Item()
            : base()
        {
            ParentRelations = new List<Aggregation>();
            Data.Add(new XElement("data"));
        }

        public override string GetTagName() { return "item"; }

        [NotMapped]
        public virtual IEnumerable<DataObject> DataObjects
        {
            get
            {
                return GetChildModels("data/*", Data).Select(c => c as DataObject);
            }
        }

        [NotMapped]
        public virtual IEnumerable<DataFile> Files
        {
            get
            {
                return GetChildModels("data/file", Data).Select(c => c as DataFile);
            }
        }

        protected XElement GetDataObjectRoot(bool createIfNotExist = true)
        {
            XElement data = Data.Element("data");

            if (data == null && createIfNotExist)
                Data.Add(data = new XElement("data"));

            return data;
        }

        public void AddObject(DataObject obj)
        {
            GetDataObjectRoot().Add(obj.Data);
        }


        public void RemoveFile(string fileGuidName)
        {
            var xpath = "./data/file[@guid-name='" + fileGuidName + "']";
            XElement file = GetChildElements(xpath, Data).FirstOrDefault();
            if (file == null)
                throw new Exception("File does not exist.");
            file.Remove();
        }

    }
}