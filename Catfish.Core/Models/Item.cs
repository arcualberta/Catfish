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
        public virtual List<DataFile> Files
        {
            get
            {
                return GetChildModels("data/file", Data).Select(c => c as DataFile).ToList();
            }

            set
            {
                //Removing all children inside the data element
                RemoveAllElements("data/file", Data);

                if (value != null)
                {
                    var data = GetDataElement();
                    foreach (DataFile df in value)
                        data.Add(df.Data);
                }
            }
        }

        protected XElement GetDataElement(bool createIfNotExist = true)
        {
            XElement data = Data.Element("data");

            if (data == null && createIfNotExist)
                Data.Add(data = new XElement("data"));

            return data;
        }

        public void AddFile(DataFile df)
        {
            GetDataElement().Add(df.Data);
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