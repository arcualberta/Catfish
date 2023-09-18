using Catfish.API.Repository.Solr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataProcessing.ShowtimeMySqlProcessing
{
    public class MySqlModel
    {
        public static readonly string STR2STR = "','";
        public static readonly string STR2NUM = "',";
        public static readonly string NUM2STR = ",'";
        public static readonly string NUM2NUM = ",";

        protected static Regex _csvSplitRegx = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        protected static string? getFieldValue(
            string concatenatedFieldValues,
            int currentOffset,
            string? endOfValueSeparator,
            out int nextOffset)
        {
            int idx = 0;
            try
            {
                if (endOfValueSeparator != null)
                {
                    //Checking if the current value is a NULL string
                    if (concatenatedFieldValues.IndexOf("NULL,", currentOffset) == currentOffset)
                    {
                        nextOffset = currentOffset + 5;
                        return null;
                    }
                    else
                    {
                        idx = concatenatedFieldValues.IndexOf(endOfValueSeparator, currentOffset);

                        //Checking if the next value is a NULL string
                        bool isNextStringNull = false;
                        if (endOfValueSeparator == STR2STR || endOfValueSeparator == NUM2STR)
                        {
                            int nextNullPosition = concatenatedFieldValues.IndexOf(",NULL,", currentOffset);

                            if (idx < 0 || (nextNullPosition > 0 && nextNullPosition < idx))
                            {
                                idx = nextNullPosition;
                                isNextStringNull = true;
                            }
                        }

                        string value = concatenatedFieldValues.Substring(currentOffset, idx - currentOffset).Trim('\'');
                        if (isNextStringNull)
                            nextOffset = idx + 1;
                        else
                            nextOffset = idx + endOfValueSeparator.Length;

                        return value;
                    }
                }
                else
                {
                    string value = concatenatedFieldValues.Substring(currentOffset).Trim('\'');
                    nextOffset = -1;
                    return value;
                }
            }
            catch(Exception e) { throw e; }
        }

        protected static void AddArrayField(SolrDoc doc, string fieldName, string? concatenatedFieldValue)
        {
            if (!string.IsNullOrEmpty(concatenatedFieldValue))
                foreach (var x in concatenatedFieldValue.Split(";", StringSplitOptions.TrimEntries).ToList())
                    doc.AddField(fieldName, x);
        }

    }
}
