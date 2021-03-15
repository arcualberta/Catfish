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
            set => SetAttribute("rows", value);
        }

        public int Cols
        {
            get => GetAttribute("cols", 5);
            set => SetAttribute("cols", value);
        }

        public int MaxWords
        {
            get => GetAttribute("max-words", 0);
            set => SetAttribute("max-words", value);
        }

        public int MaxChars
        {
            get => GetAttribute("max-chars", 0);
            set => SetAttribute("max-chars", value);
        }

        public TextArea SetSize(int rows, int cols) { Rows = rows; Cols = cols; return this; }

        public TextArea() : base() { DisplayLabel = "Paragraph"; }
        public TextArea(XElement data) : base(data) { DisplayLabel = "Paragraph"; }
        public TextArea(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Paragraph"; }
    }
}
