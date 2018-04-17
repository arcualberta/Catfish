using Catfish.Core.Models.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Catfish.Core.Models.Forms
{
    [TypeLabel("Client info")]
    public class ClientInfo : FormField
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
