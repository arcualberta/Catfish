using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Expressions
{
    public class RequiredCondition : Expression
    {
        public const string TagName = "required-if";

        public RequiredCondition() : base(TagName) { }
        public RequiredCondition(XElement data) : base(data) { }
    }
}
