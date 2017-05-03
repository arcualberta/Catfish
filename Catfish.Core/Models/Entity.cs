using System;

namespace Catfish.Core.Models
{
    [Serializable]
    public class Entity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Name { get; set; }

        public Entity()
        {
            Created = DateTime.Now;
        }
    }
}