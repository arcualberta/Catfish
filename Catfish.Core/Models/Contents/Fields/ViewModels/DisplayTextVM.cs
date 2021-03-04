using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    public class DisplayTextVM
    {
        public Guid Id { get; set; }
        public string Format { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }

        public DisplayTextVM(MultilingualText src)
        {
            //has more than one, but should be handled outside this class...
            UpdateDisplayTextVM(src.Values);
        }

        public void UpdateDisplayTextVM(Text src)
        {
            Id = src.Id;
            Format = src.Format.ToString();
            Language = src.Language;
            Value = src.Value;
        }
    }
}
