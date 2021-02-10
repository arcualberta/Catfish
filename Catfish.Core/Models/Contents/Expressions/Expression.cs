using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Expressions
{
    public class Expression : XmlModel
    {
        public enum eMath { PLUS, MINUS, MULT, DIV };
        public enum eLogical { AND, OR };
        public enum eRelational { EQUAL, LESS_THAN, GREATER_THAN, LESS_OR_EQ, GREATER_OR_EQ, NOT_EQ };

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
            Data.Value += string.Format("StrValue('{0}') === '{1}'", field.Id, val.Id);
            return this;
        }

        public Expression Append(OptionsField field, Option[] vals, eLogical aggregatorOperator)
        {
            List<string> fragments = new List<string>();
            for (int i = 0; i < vals.Length; ++i)
                fragments.Add(string.Format("StrValue('{0}') === '{1}'", field.Id, vals[i].Id));

            string op = aggregatorOperator == eLogical.AND ? " && " : " || ";
            Data.Value += string.Format("({0})", string.Join(op, fragments));
            return this;
        }

        public Expression Append(IntegerField field, string @operator, int val)
        {
            Data.Value += string.Format("IntValue('{0}'){1}{2}", field.Id, @operator, val);
            return this;
        }

        public Expression Append(IntegerField field, string @operator, double val)
        {
            Data.Value += string.Format("DoubleValue('{0}'){1}{2}", field.Id, @operator, val);
            return this;
        }

        public Expression Append(IntegerField field, string @operator, float val)
        {
            Data.Value += string.Format("FloatValue('{0}'){1}{2}", field.Id, @operator, val);
            return this;
        }

        public Expression Append(IntegerField field, string @operator, decimal val)
        {
            Data.Value += string.Format("DecimalValue('{0}'){1}{2}", field.Id, @operator, val);
            return this;
        }
    }
}
