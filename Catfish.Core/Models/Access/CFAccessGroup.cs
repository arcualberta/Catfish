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
        public List<Guid> AccessGuids
        {
            get
            {
                try
                {
                    XElement accessGuidsElement = Data.Element("access-guids");
                 
                    List<XElement> accessGuidsElements = accessGuidsElement.Elements("access-guid").ToList();
                    List<Guid> accessGuids = new List<Guid>();

                    foreach (XElement accessGuidElement in accessGuidsElements)
                    {
                        accessGuids.Add(new Guid(accessGuidElement.Value));
                    }

                    return accessGuids;
                }
                catch
                {
                    return null;
                }

            }

            set
            {
                XElement accessGuids = Data.Element("access-guids");
                if (accessGuids == null)
                {
                    accessGuids = new XElement("access-guids");
                    Data.Add(accessGuids);
                }
                foreach (Guid accessGuid in value)
                {
                    XElement accessGuidElement = new XElement("access-guid");
                    accessGuidElement.Value = accessGuid.ToString();
                    accessGuids.Add(accessGuidElement);
                }
            }
        }

        private void InitializeAccessDefinition(CFAccessDefinition accessDefinition)
        {
            Data.Add(accessDefinition.Data);
        }

        public CFAccessGroup()
        {
            AccessGuids = new List<System.Guid>();
        }
        

    }
}
