using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    public class FieldValue
    {
        public Guid Id { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }

        public FieldValue() { }

        public FieldValue(Text text) {

            Id = text.Id;
            Language = text.Language;
            Value = text.Value;
        
        }
    }
}
