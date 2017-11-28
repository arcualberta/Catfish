using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
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
                return GetChildModels("data/" + DataFile.TagName, Data).Select(c => c as DataFile);
            }
        }

        protected XElement GetDataObjectRoot(bool createIfNotExist = true)
        {
            return GetImmediateChild("data");
        }

        public void AddData(DataObject obj)
        {
            GetDataObjectRoot().Add(obj.Data);
        }

        public FormSubmission GetFormSubmission(string formSubmissionRef)
        {
            var xpath = "./data/" + FormSubmission.TagName + "[@ref='" + formSubmissionRef + "']";
            return GetChildModels(xpath, Data).FirstOrDefault() as FormSubmission;
        }


        public void RemoveFile(string fileGuidName)
        {
            var xpath = "./data/" + DataFile.TagName + "[@guid-name='" + fileGuidName + "']";
            XElement file = GetChildElements(xpath, Data).FirstOrDefault();
            if (file == null)
                throw new Exception("File does not exist.");
            file.Remove();
        }

    }
}