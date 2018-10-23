using Catfish.Core.Models.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Catfish.Core.Models.Forms
{
    [CFTypeLabel("Client info")]
    
    public class CFClientInfo : CFFormField
    {
        public string GetInfo(HttpContextBase context)
        {
            string info = context.Request.UserAgent;
           
            return info;
        }

        public  override bool IsHidden()
        {
            return true;
        }


        //[NotMapped]
        //public string ClintInfo
        //{
        //    get
        //    {
        //        return GetDescription();
        //    }

        //    set
        //    {
        //        SetDescription(value);
        //    }
        //}
    }
}
