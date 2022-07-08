using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.FormBuilder.Fields
{
    public class Option
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Label { get; set; }
        public string Description { get; set; }
        public int? Limit { get; set; }
        public decimal? Price { get; set; }
        public bool IsExtended { get; set; }
        public bool IsDefault { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}