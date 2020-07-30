using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Data
{
    public class DataItem : FieldContainer
    {
        public const string TagName = "data-item";

        public DataItem() : base(TagName) { Initialize(eGuidOption.Ensure); }
        public DataItem(XElement data) : base(data) { Initialize(eGuidOption.Ensure); }

        public new void Initialize(eGuidOption guidOption)
        {
            //Ensuring that each metadata set has a unique ID
            base.Initialize(guidOption == eGuidOption.Ignore ? eGuidOption.Ensure : guidOption);
        }
    }
}
