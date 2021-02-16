using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class TextArea : TextField
    {
        public int Rows
        {
            get => GetAttribute("rows", 5);
            set => SetAttribute("rows", 5);
        }

        public int Cols
        {
            get => GetAttribute("cols", 5);
            set => SetAttribute("cols", value);
        }

        public TextArea SetSize(int rows, int cols) { Rows = rows; Cols = cols; return this; }

        public TextArea() : base() { DisplayLabel = "Paragraph"; }
        public TextArea(XElement data) : base(data) { DisplayLabel = "Paragraph"; }
        public TextArea(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Paragraph"; }
    }
}
