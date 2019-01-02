using Catfish.Core.Models.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace Catfish.Core.Models.Forms
{
    [CFTypeLabel("Short text")]
    public class TextField : FormField
    {

        [ScriptIgnore]
        [NotMapped]
        [Display(Name = "Is Multiple")]
        public bool IsMultiple
        {
            get
            {
                return GetAttribute("IsMultiple", false);
                //if (Data != null)
                //{
                //    var att = Data.Attribute("IsMultiple");
                //    return att != null ? att.Value == "true" : false;
                //}
                //return false;
            }

            set
            {
                SetAttribute("IsMultiple", value);
                //Data.SetAttributeValue("IsMultiple", value);
            }
        }

       
    }
}
