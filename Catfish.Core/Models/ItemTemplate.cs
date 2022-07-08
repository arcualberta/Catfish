using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    public class ItemTemplate : EntityTemplate 
    {
        public ItemTemplate()
            : base()
        {
            ////Initialize(false);
        }

        public ItemTemplate(XElement data)
            : base(data)
        {
        }


    }
}
