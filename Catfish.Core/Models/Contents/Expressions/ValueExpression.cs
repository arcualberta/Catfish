using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Expressions
{
    public class ValueExpression : ComputationExpression
    {
        public const string TagName = "value-expression";

        public ValueExpression() : base(TagName) { }
        public ValueExpression(XElement data) : base(data) { }
    }
}
