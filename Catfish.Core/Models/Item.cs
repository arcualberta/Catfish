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
        public string Name { get { return GetName(); } }
        [NotMapped]
        public string Description { get { return GetDescription(); } }
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

                foreach (DataFile df in value)
                    InsertChildElement("./files", df.Data);
            }
        }

        public override void UpdateValues(XmlModel src)
        {
            base.UpdateValues(src);

            var src_item = src as Item;

            foreach (MetadataSet ms in this.MetadataSets)
            {
                var src_ms = src_item.MetadataSets.Where(x => x.Ref == ms.Ref).FirstOrDefault();
                ms.UpdateValues(src_ms);
            }


            this.Serialize();
        }
    }
}