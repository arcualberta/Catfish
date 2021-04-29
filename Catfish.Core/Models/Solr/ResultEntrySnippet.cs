using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class ResultEntrySnippet
    {
        public SearchFieldConstraint.eScope Scope { get; set; }
        public Guid ContainerId { get; set; }
        public Guid FieldId { get; set; }

        public List<string> Highlights = new List<string>();
    }
}
