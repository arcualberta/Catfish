using System;

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
