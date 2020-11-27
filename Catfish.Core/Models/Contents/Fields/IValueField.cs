using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Fields
{
    /// <summary>
    /// IValueField represents a field that can hold a value provided as user input.
    /// </summary>
    public interface IValueField
    {
        public IEnumerable<Text> GetValues(string lang = null);

        /// <summary>
        /// Returns a concatenated string of values input by users. If a langauge is specified, the
        /// output is limited to values provided in that language.
        /// </summary>
        /// <returns></returns>
        public string GetValues(string separator, string lang = null);
    }
}
