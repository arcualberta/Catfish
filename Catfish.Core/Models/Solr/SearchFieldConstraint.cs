using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class SearchFieldConstraint
    {
        public enum eScope { Data, Metadata }

        public eScope Scope { get; set; }
        public Guid ContainerId { get; set; }
        public Guid FieldId { get; set; }
        public string SearchText { get; set; }

    }
}
