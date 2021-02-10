using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Expressions
{
    public class ComputationExpression : XmlModel
    {
        public enum eMath { PLUS, MINUS, MULT, DIV }
        public enum eLogical { AND, OR }
        public enum eRelational { EQUAL, LESS_THAN, GREATER_THAN, LESS_OR_EQ, GREATER_OR_EQ, NOT_EQ }

        public ComputationExpression(string tagName) : base(tagName) { }
        public ComputationExpression(XElement data) : base(data) { }
        public string Value { get { return Data.Value; } }
        public ComputationExpression Clear()
        {
            Data.Value = "";
            return this;
        }

        public static string Expression(SelectField field, eRelational @operator, Option val)
        {
            return string.Format("StrValue('{0}'){1}'{2}'", field.Id, Str(@operator), val.Id);
        }

        public static string Expression(RadioField field, eRelational @operator, Option val)
        {
            return string.Format("RadioValue('{0}'){1}'{2}'", field.Id, Str(@operator), val.Id);
        }

        public static string Expression(CheckboxField field, Option val, bool isChecked)
        {
            return string.Format("CheckboxValue('{0}', '{1}') === {2}", field.Id, val.Id, isChecked ? "true" : "false");
        }
        public ComputationExpression Append(string val)
        {
            Data.Value += val;
            return this;
        }

        public ComputationExpression Append(SelectField field, eRelational @operator, Option val)
        {
            Data.Value += Expression(field, @operator, val);
            return this;
        }

        public ComputationExpression Append(RadioField field, eRelational @operator, Option val)
        {
            Data.Value += Expression(field, @operator, val);
            return this;
        }

        public ComputationExpression Append(CheckboxField field, Option val, bool isChecked)
        {
            Data.Value += Expression(field, val, isChecked);
            return this;
        }

        public ComputationExpression AppendMatch(OptionsField field, Option[] vals, eLogical aggregatorOperator)
        {
            List<string> fragments = new List<string>();
            for (int i = 0; i < vals.Length; ++i)
            {
                if(typeof(CheckboxField).IsAssignableFrom(field.GetType()))
                    fragments.Add(Expression(field as CheckboxField, vals[i], true));
                else if (typeof(RadioField).IsAssignableFrom(field.GetType()))
                    fragments.Add(Expression(field as RadioField, eRelational.EQUAL, vals[i]));
                else if (typeof(SelectField).IsAssignableFrom(field.GetType()))
                    fragments.Add(Expression(field as SelectField, eRelational.EQUAL, vals[i]));
            }
            Data.Value += string.Format("({0})", string.Join(Str(aggregatorOperator), fragments));
            return this;
        }

        public static string Str(eRelational val)
        {
            switch (val)
            {
                case eRelational.EQUAL: return "===";
                case eRelational.LESS_THAN: return "<";
                case eRelational.GREATER_THAN: return ">";
                case eRelational.LESS_OR_EQ: return "<=";
                case eRelational.GREATER_OR_EQ: return ">=";
                case eRelational.NOT_EQ: return "!==";
                default: throw new Exception("Unknow relational operator");
            }
        }

        public static string Str(eLogical val)
        {
            switch (val)
            {
                case eLogical.AND: return "&&";
                case eLogical.OR: return "||";
                default: throw new Exception("Unknow logical operator");
            }
        }
        
    }
}
