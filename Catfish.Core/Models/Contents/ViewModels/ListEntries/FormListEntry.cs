using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Catfish.Core.Models.Contents.ViewModels.ListEntries
{
    public class FormListEntry
    {
        public Guid Id { get; set; }
        public MultilingualText Name { get; protected set; }
        public MultilingualText Description { get; protected set; }

        public FormListEntry(Form form)
        {
            Id = form.Id;
            Name = form.Name;
            Description = form.Description;
        }
    }
}
