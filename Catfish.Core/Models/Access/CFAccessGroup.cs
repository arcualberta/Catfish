using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Catfish.Core.Models.Access
{
    public class CFAccessGroup : CFXmlModel
    {
        public static string TagName {
            get
            {
                return "access-group";                
            }
        }

        public override string GetTagName() { return TagName; }


        [NotMapped]
        public CFAccessDefinition AccessDefinition
        {
            get
            {
                CFAccessDefinition accessDefinition = GetChildModels(CFAccessDefinition.TagName).FirstOrDefault() as CFAccessDefinition;

                if (accessDefinition == null)
                {
                    accessDefinition = new CFAccessDefinition();
                    Data.Add(accessDefinition.Data);
                }
                
                return accessDefinition;
                
            }
            set
            {

                RemoveAllElements("access-group", Data);                
                InitializeAccessDefinition(value);
            }
        }

        [NotMapped]
        public Guid AccessGuid
        {
            get
            {
                try
                {

                    XElement accessGuidElement = Data.Element("access-guid");
                    return new Guid(accessGuidElement.Value);
                }
                catch
                {
                    // Empty Guid by default
                    return new Guid();
                }
            }

            set
            {
                XElement accessGuid = Data.Element("access-guid");
                if (accessGuid == null)
                {
                    accessGuid = new XElement("access-guid");
                    Data.Add(accessGuid);
                }

                accessGuid.Value = value.ToString();
                             
            }
        }

        private void InitializeAccessDefinition(CFAccessDefinition accessDefinition)
        {
            Data.Add(accessDefinition.Data);
        }

        public CFAccessGroup()
        {
            //AccessGuids = new List<System.Guid>();
        }
        

    }
}
