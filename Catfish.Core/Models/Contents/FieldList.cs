using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class FieldList : XmlModelList<BaseField>
    {
        public FieldList()
            :base(new XElement(FieldContainer.FieldContainerTag), false)
        {

        }

        public FieldList(XElement data)
            :base(data, true, "field")
        {

        }
    }
}
