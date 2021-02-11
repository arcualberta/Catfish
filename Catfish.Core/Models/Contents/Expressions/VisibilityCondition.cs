using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Expressions
{
    public class VisibilityCondition : ComputationExpression
    {
        public const string TagName = "visible-if";

        public VisibilityCondition() : base(TagName) { }
        public VisibilityCondition(XElement data) : base(data) { }
    }
}
