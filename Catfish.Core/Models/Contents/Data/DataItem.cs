using Catfish.Core.Models.Contents.Fields;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Data
{
    public class DataItem : FieldContainer
    {
        public const string TagName = "data-item";

        public bool IsRoot
        {
            get => GetAttribute("is-root", false);
            set => SetAttribute("is-root", value);
        }

        public Guid? EntityId
        {
            get => GetAttribute("entity-id", null as Guid?);
            set => SetAttribute("entity-id", value.ToString());
        }

        public string OwnerId
        {
            get => GetAttribute("owner-id", null as string);
            set => SetAttribute("owner-id", value);
        }
        public string OwnerName
        {
            get => GetAttribute("owner-name", null as string);
            set => SetAttribute("owner-name", value);
        }

        public string TestField { get; set; }

        public DataItem() : base(TagName) { Initialize(eGuidOption.Ensure); }
        public DataItem(XElement data) : base(data) { Initialize(eGuidOption.Ensure); }

        public new void Initialize(eGuidOption guidOption)
        {
            //Ensuring that each metadata set has a unique ID
            base.Initialize(guidOption == eGuidOption.Ignore ? eGuidOption.Ensure : guidOption);

        }
    }
}
