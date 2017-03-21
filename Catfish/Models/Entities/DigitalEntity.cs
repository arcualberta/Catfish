using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.Entities
{
    public class DigitalEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Title { get; set; }
    }
}