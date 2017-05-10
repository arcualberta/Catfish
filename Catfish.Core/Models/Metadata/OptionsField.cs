using System.ComponentModel.DataAnnotations;

namespace Catfish.Core.Models.Metadata
{
    public class OptionsField: MetadataField
    {
        [DataType(DataType.MultilineText)]
        public string Options { get; set; }
    }
}
