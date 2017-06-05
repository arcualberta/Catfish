using System;
using System.ComponentModel.DataAnnotations;
using Catfish.Core.Models.Attributes;

namespace Catfish.Core.Models.Metadata
{
    [Ignore]
    public class OptionsField: SimpleField
    {
        [Rank(3)]
        [InputType(InputTypeAttribute.eInputType.StringArray)]
        [DataType(DataType.MultilineText)]
        [TypeLabel("List of options, one option per line")]
        public string Options { get; set; }
    }
}
