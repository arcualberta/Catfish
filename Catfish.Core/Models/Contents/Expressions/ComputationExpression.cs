using Catfish.Core.Models.Contents.Fields;
using Newtonsoft.Json;
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

        //public static string Expression(IntegerField field, Option val, bool isChecked)
        //{
        //    return string.Format("CheckboxValue('{0}', '{1}') === {2}", field.Id, val.Id, isChecked ? "true" : "false");
        //}
        //public static string Expression(Decimal field, Option val, bool isChecked)
        //{
        //    return string.Format("CheckboxValue('{0}', '{1}') === {2}", field.Id, val.Id, isChecked ? "true" : "false");
        //}

        public ComputationExpression AppendOperator(eMath @operator)
        {
            Data.Value += Str(@operator);
            return this;
        }

        public ComputationExpression AppendOperator(eLogical @operator)
        {
            Data.Value += Str(@operator);
            return this;
        }

        public ComputationExpression AppendOperator(eRelational @operator)
        {
            Data.Value += Str(@operator);
            return this;
        }

        public ComputationExpression AppendOpenBrace()
        {
            Data.Value += "(";
            return this;
        }

        public ComputationExpression AppendClosedBrace()
        {
            Data.Value += ")";
            return this;
        }

        public ComputationExpression AppendLogicalExpression(SelectField field, eRelational @operator, Option val)
        {
            Data.Value += Expression(field, @operator, val);
            return this;
        }

        public ComputationExpression AppendLogicalExpression(RadioField field, eRelational @operator, Option val)
        {
            Data.Value += Expression(field, @operator, val);
            return this;
        }

        public ComputationExpression AppendLogicalExpression(CheckboxField field, Option val, bool isChecked)
        {
            Data.Value += Expression(field, val, isChecked);
            return this;
        }

        /// <summary>
        /// Test the value selected for the "field" against each value in the "vals" to find which 
        /// element in "vals" matches with the selection and then agregates these matches based on
        /// the operator defined by "aggregatorOperator". IMPORTANT: that all elements in "vals"
        /// MUST BE part of option elements retrieved from the specified "field" (i.e. their id's 
        /// should match in addition to display labels.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="vals"></param>
        /// <param name="aggregatorOperator"></param>
        /// <returns></returns>
        public ComputationExpression AppendLogicalExpression(OptionsField field, Option[] vals, eLogical aggregatorOperator)
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

        /// <summary>
        /// Appends the value of a field to the expression.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public ComputationExpression AppendValue(DateField field)
        {
            Data.Value += string.Format("DateValue('{0}')", field.Id);
            return this;
        }

        /// <summary>
        /// Appends the value of a field to the expression.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public ComputationExpression AppendValue(DecimalField field)
        {
            Data.Value += string.Format("DecimalValue('{0}')", field.Id);
            return this;
        }

        /// <summary>
        /// Appends the value of a field to the expression.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public ComputationExpression AppendValue(IntegerField field)
        {
            Data.Value += string.Format("IntValue('{0}')", field.Id);
            return this;
        }

        /// <summary>
        /// Appends the value of a field to the expression.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public ComputationExpression AppendValue(RadioField field)
        {
            Data.Value += string.Format("RadioValue('{0}')", field.Id);
            return this;
        }

        /// <summary>
        /// Appends the value of a field to the expression.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public ComputationExpression AppendValue(SelectField field)
        {
            Data.Value += string.Format("StrValue('{0}')", field.Id);
            return this;
        }

        public ComputationExpression AppendReadableValue(SelectField field,
            string delimiter = null,
            int selectItemIndex = 0,
            bool trim = true)
        {
            var valueString = string.Format("SelectFieldReadableValue('{0}')", field.Id);

            if (!string.IsNullOrEmpty(delimiter) || trim)
                valueString = string.Format("Extract({0}, '{1}', {2}, {3})",
                    valueString, delimiter, selectItemIndex, trim.ToString().ToLower());

            Data.Value += valueString;

            return this;
        }

        public ComputationExpression AppendReadableValue(RadioField field,
            string delimiter = null,
            int selectItemIndex = 0,
            bool trim = true)
        {
            var valueString = string.Format("RadioFieldReadableValue('{0}')", field.Id);

            if (!string.IsNullOrEmpty(delimiter) || trim)
                valueString = string.Format("Extract({0}, '{1}', {2}, {3})",
                    valueString, delimiter, selectItemIndex, trim.ToString().ToLower());

            Data.Value += valueString;

            return this;
        }

        public ComputationExpression AppendColumnSum(TableField field, int columnIndex)
        {
            Data.Value += string.Format("TableColumnSum('{0}', {1})", field.Id, columnIndex);
            return this;
        }

        public ComputationExpression AppendCompositeFieldSum(CompositeField field, int childFieldIndex)
        {
            Data.Value += string.Format("CompositeFieldSum('{0}', {1})", field.Id, childFieldIndex);
            return this;
        }



        public void ReplaceReferences(Guid oldId, Guid newId)
        {
            Data.Value = Data.Value.Replace(oldId.ToString(), newId.ToString());
        }

        public static string Str(eMath val)
        {
            switch (val)
            {
                case eMath.PLUS: return "+";
                case eMath.MINUS: return "-";
                case eMath.MULT: return "*";
                case eMath.DIV: return "/";
                default: throw new Exception("Unknow mathematical operator");
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

        /// <summary>
        /// Appends the value of a field to the expression.
        /// </summary>
        /// <param name="intValue"></param>
        /// <returns></returns>
        public ComputationExpression AppendValue(int val)
        {
            Data.Value += string.Format("{0}", val);
            return this;
        }

        public ComputationExpression AppendValue(decimal val)
        {
            Data.Value += string.Format("{0}", val);
            return this;
        }

        public ComputationExpression AppendValue(double val)
        {
            Data.Value += string.Format("{0}", val);
            return this;
        }

        public ComputationExpression AppendValue(string val)
        {
            Data.Value += string.Format("'{0}'", val);
            return this;
        }

    }
}
