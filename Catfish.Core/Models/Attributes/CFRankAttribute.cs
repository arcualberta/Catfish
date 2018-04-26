using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CFRankAttribute : Attribute
    {
        public int Rank { get; set; }

        public CFRankAttribute(int rank)
        {
            Rank = rank;
        }
    }
}
