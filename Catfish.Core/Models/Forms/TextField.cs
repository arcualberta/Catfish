using Catfish.Core.Models.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
//using System.Web.Script.Serialization;

namespace Catfish.Core.Models.Forms
{
    [TypeLabel("Short text")]
    public class TextField : FormField
    {

        //[ScriptIgnore] KR:.NETCORE
        [IgnoreDataMember]
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
