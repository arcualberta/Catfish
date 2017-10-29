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
            Data.Add(new XElement("files"));
        }

        public override string GetTagName() { return "item"; }

        [NotMapped]
        public virtual List<DataFile> Files
        {
            get
            {
                return GetChildModels("files/file", Data).Select(c => c as DataFile).ToList();
            }

            set
            {
                //Removing all children inside the files element
                RemoveAllElements("files/file", Data);

                if (value != null)
                {
                    foreach (DataFile df in value)
                        InsertChildElement("./files", df.Data);
                }
            }
        }

        public void AddFile(DataFile df)
        {
            InsertChildElement("./files", df.Data);
        }

        public void RemoveFile(string fileGuidName)
        {
            var xpath = "./files/file[@guid-name='" + fileGuidName + "']";
            XElement file = GetChildElements(xpath, Data).FirstOrDefault();
            if (file == null)
                throw new Exception("File does not exist.");
            file.Remove();
        }

    }
}