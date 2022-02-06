using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class FieldContainerList : XmlModelList<FieldContainer>
    {
        public FieldContainerList()
            :base(new XElement(FieldContainer.FieldContainerTag), false)
        {

        }

        public FieldContainerList(XElement data)
            :base(data, true, string.Format("{0},{1}", DataItem.TagName, MetadataSet.TagName))
        {

        }
    }
}
