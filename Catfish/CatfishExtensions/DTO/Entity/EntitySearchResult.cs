using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO.Entity
{
    public class EntitySearchResult
    {
        public IEnumerable<EntityEntry> Result { get; set; } = new List<EntityEntry>();
        public int Offset { get; set; }
        public int Total { get; set; }
    }
}
