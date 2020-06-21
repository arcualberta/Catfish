using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Catfish.Core.Models.Contents.ViewModels.ListEntries
{
    public class FieldContainerListEntry
    {
        public Guid Id { get; set; }
        public MultilingualText Name { get; protected set; }
        public MultilingualText Description { get; protected set; }

        public FieldContainerListEntry(FieldContainer form)
        {
            Id = form.Id;
            Name = form.Name;
            Description = form.Description;
        }
    }
}
