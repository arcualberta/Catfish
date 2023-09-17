using Catfish.API.Repository.Solr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    public class MySqlModel
    {
        public static readonly string STR2STR = "','";
        public static readonly string STR2INT = "',";
        public static readonly string INT2STR = ",'";
        public static readonly string INT2INT = ",";

        protected static string getFieldValue(
            string concatenatedFieldValues,
            int currentOffset,
            string endOfValueSeparator,
            out int nextOffset)
        {
            if (endOfValueSeparator != null)
            {
                int idx = concatenatedFieldValues.IndexOf(endOfValueSeparator, currentOffset);
                string value = concatenatedFieldValues.Substring(currentOffset, idx - currentOffset);
                nextOffset = idx + endOfValueSeparator.Length;
                return value;
            }
            else
            {
                string value = concatenatedFieldValues.Substring(currentOffset).Trim('\'');
                nextOffset = -1;
                return value;
            }
        }

        protected static void AddArrayField(SolrDoc doc, string fieldName, string? concatenatedFieldValue)
        {
            if (!string.IsNullOrEmpty(concatenatedFieldValue))
                foreach (var x in concatenatedFieldValue.Split(";", StringSplitOptions.TrimEntries).ToList())
                    doc.AddField(fieldName, x);
        }

    }
}
