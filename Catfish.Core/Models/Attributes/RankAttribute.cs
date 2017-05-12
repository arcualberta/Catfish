using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RankAttribute : Attribute
    {
        public int Rank { get; set; }

        public RankAttribute(int rank)
        {
            Rank = rank;
        }
    }
}
