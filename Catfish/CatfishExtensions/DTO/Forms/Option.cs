using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO.Forms
{
    public class Option
    {
        public Guid Id { get; set; }

        public bool Selected { get; set; }

        public TextCollection OptionText { get; set; }

        public bool IsExtendedInput { get; set; }

        public bool IsExtendedInputRequired { get; set; }
    }
}
