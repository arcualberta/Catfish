using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Expressions
{
    public class Expression : XmlModel
    {
        public Expression(string tagName) : base(tagName) { }
        public Expression(XElement data) : base(data) { }
        public string Value { get { return Data.Value; } }
        public Expression Clear()
        {
            Data.Value = "";
            return this;
        }

        public Expression Append(string val)
        {
            Data.Value += val;
            return this;
        }

        public Expression Append(OptionsField field, Option val)
        {
            Data.Value += string.Format("{0}:{1}", field.Id, val.Id);
            return this;
        }
        public Expression Append(IntegerField field, string @operator, int val)
        {
            Data.Value += string.Format("{0}{1}{2}", field.Id, @operator, val);
            return this;
        }

    }
}
