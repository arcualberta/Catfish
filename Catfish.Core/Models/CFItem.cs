using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    [Serializable]
    public class CFItem : CFAggregation
    {
        [IgnoreDataMember]
        public virtual ICollection<CFAggregation> ParentRelations { get; set; }

        public CFItem()
            : base()
        {
            ParentRelations = new List<CFAggregation>();
            Data.Add(new XElement("data"));
        }

        public override string GetTagName() { return "item"; }

        [NotMapped]
        [IgnoreDataMember]
        public virtual IEnumerable<CFDataObject> DataObjects
        {
            get
            {
                return GetChildModels("data/*", Data).Select(c => c as CFDataObject);
            }
        }

        /// <summary>
        /// This field is only used for passing file attachments between controller action and Items/Edit view through model binding
        /// </summary>
        [NotMapped]
        [IgnoreDataMember]
        public Attachment AttachmentField
        {
            get
            {
                if (mAttachmentField == null)
                {
                    mAttachmentField = new Attachment();
                    mAttachmentField.FileGuids = string.Join(Attachment.FileGuidSeparator.ToString(), Files.Select(f => f.Guid));
                }
                return mAttachmentField;
            }
            set
            {
                mAttachmentField = value;
            }
        }

        [NonSerialized]
        private Attachment mAttachmentField;

        [NotMapped]
        [IgnoreDataMember]
        public virtual IEnumerable<CFDataFile> Files
        {
            get
            {
                return GetChildModels("data/" + CFDataFile.TagName, Data).Select(c => c as CFDataFile);
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public virtual IEnumerable<CFFormSubmission> FormSubmissions
        {
            get
            {
                return GetChildModels("data/" + CFFormSubmission.TagName, Data).Select(c => c as CFFormSubmission);
            }
        }

        // XXX copy this for files
        protected XElement GetDataObjectRoot(bool createIfNotExist = true)
        {
            return GetImmediateChild("data");
        }

        // XXX copy this for files
        public void AddData(CFDataObject obj)
        {
            GetDataObjectRoot().Add(obj.Data);

            if (this.Id > 0)//Onlye adding logs for new data objects when updating existing items
                LogChange(obj.Guid, "Added data object");
        }

        public CFFormSubmission GetFormSubmission(string formSubmissionRef)
        {
            var xpath = "./data/" + CFFormSubmission.TagName + "[@ref='" + formSubmissionRef + "']";
            return GetChildModels(xpath, Data).FirstOrDefault() as CFFormSubmission;
        }

        public void RemoveFile(CFDataFile file)
        {
            var xpath = "./data/" + CFDataFile.TagName + "[@guid='" + file.Guid + "']";
            XElement fileElement = GetChildElements(xpath, Data).FirstOrDefault();
            if (fileElement == null)
                throw new Exception("File does not exist.");
            fileElement.Remove();

            file.DeleteFilesFromFileSystem();

            LogChange(file.Guid, "Deleted " + file.FileName);
        }

    }
}