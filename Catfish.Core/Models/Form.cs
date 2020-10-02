using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    [Table("Catfish_Forms")]
    public class Form: FieldContainer
    {
        public static readonly string TagName = "form";
        public string FormName { get; set; }
        public Form() : base(TagName) { Initialize(eGuidOption.Ensure); }
        public Form(XElement data) : base(data) { Initialize(eGuidOption.Ensure); }
    }
}
