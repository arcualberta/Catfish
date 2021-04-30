using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Solr
{
    public class ResultEntry
    {
        public Guid Id { get; set; }
        public Guid TemplateId { get; set; }
        public List<ResultEntrySnippet> Snippets { get; set; } = new List<ResultEntrySnippet>();
    }
}
