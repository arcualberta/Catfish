﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Catfish.Models.FormBuilder
{
    public class Field
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int? ForeignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public string TemplateButtonLabel { get; set; }
        public string ComponentType => GetType().Name;

        [NotMapped]
        public string Icon { get; set; }
        public Field()
        {
            TemplateButtonLabel = GetType().Name;
        }

        public Field(string templateButtonLabel)
        {
            TemplateButtonLabel = templateButtonLabel;
        }
    }
}