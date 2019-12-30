using Catfish.Core.Models.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
//using System.Web.Mvc;
//using System.Web.Script.Serialization;

namespace Catfish.Core.Models.Forms
{
    [CFTypeLabel("Paragraph")]
    public class TextArea : FormField
    {
        //[ScriptIgnore] KR:.NETCORE
        [IgnoreDataMember]
        [NotMapped]
        [Display(Name = "Is Rich Text")]
        public bool IsRichText
        {
            get
            {
                if (Data != null)
                {
                    var att = Data.Attribute("IsRichText");
                    return att != null ? att.Value == "true" : false;
                }
                return false;
            }

            set
            {
                Data.SetAttributeValue("IsRichText", value);
            }
        }

        protected override bool IsHtmlField()
        {
            return IsRichText;
        }

        public override void Merge(FormField newField)
        {
            base.Merge(newField);

            IsRichText = ((TextArea)newField).IsRichText;
        }
    }
}
