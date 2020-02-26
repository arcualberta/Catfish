using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using System.Xml.Linq;

namespace Catfish.Core.Models.Access
{
    public class CFAccessGroup : XmlModel
    {
        
        public  static string TagName { get; } = "access-group";

        private static string IsInheritedAttribute { get; } = "is-inherited";
        private static string AccessGuidElementName { get; } = "access-guid";

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

                RemoveAllElements(TagName, Data);                
                InitializeAccessDefinition(value);
            }
        }

        [NotMapped]
        public bool IsInherited
        {
            get
            {
                try
                {
                    XAttribute isInherited = Data.Attribute(IsInheritedAttribute);
                    if (isInherited != null)
                    {
                        return Convert.ToBoolean(isInherited.Value);
                    }
                    return true;
                }
                catch
                {
                    return true;
                }
            }

            set
            {
                XAttribute isInherited = Data.Attribute(IsInheritedAttribute);
                if (isInherited == null)
                {
                    isInherited = new XAttribute(IsInheritedAttribute, value.ToString());                    
                    Data.Add(isInherited);
                }

                isInherited.Value = value.ToString();
            }
        }

        [NotMapped]
        public Guid AccessGuid
        {
            get
            {
                try
                {

                    XElement accessGuidElement = Data.Element(AccessGuidElementName);
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
                XElement accessGuid = Data.Element(AccessGuidElementName);
                if (accessGuid == null)
                {
                    accessGuid = new XElement(AccessGuidElementName);
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
