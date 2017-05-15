using System;
using System.ComponentModel.DataAnnotations;
using Catfish.Core.Models.Attributes;

namespace Catfish.Core.Models.Metadata
{
    [Ignore]
    public class OptionsField: MetadataField
    {
        [Rank(3)]
        [InputType(InputTypeAttribute.eInputType.StringArray)]
        [DataType(DataType.MultilineText)]
        public string Options { get; set; }
    }
}
