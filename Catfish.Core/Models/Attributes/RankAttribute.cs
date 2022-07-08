using System;

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
