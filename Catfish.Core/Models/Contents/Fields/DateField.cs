﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class DateField : MonolingualTextField
    {
        public DateField() : base() { DisplayLabel = "Date"; }
        public DateField(XElement data) : base(data) { DisplayLabel = "Date"; }
        public DateField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Date"; }
    }
}
