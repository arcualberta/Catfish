using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.ViewModels
{
    public class FieldContainerListVM
    {
        public List<FieldContainerListEntry> Entries { get; set; } = new List<FieldContainerListEntry>();

        public int OffSet { get; set; }
        public int? Max { get; set; }
    }
}
