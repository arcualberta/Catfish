using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.Test.Helpers
{
    partial class SeleniumHelper
    {
        public string GetTextFieldDisplayValue(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}'] div[class='multilingual-input-block']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

        // temp method if element is missing the multilingual class tag
        public string GetTextFieldDisplayValueAlt(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

        /// <summary>
        /// This method can use to get text display fields in a table. Need to pass the row number and column number as parameters.
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        public string GetTextFieldDisplayValue(int rowNumber, int columnNumber)
        {
            string selectorString = string.Format("table[class='table'] tbody tr[data-r='{0}'] td[data-c='{1}'] div", rowNumber, columnNumber);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }
        public string GetTextAreaDisplayValue(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}'] div[class='multilingual-input-block']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

        public string GetTextAreaDisplayValue(int rowNumber, int columnNumber)
        {
            string selectorString = string.Format("table[class='table'] tbody tr[data-r='{0}'] td[data-c='{1}'] div p", rowNumber, columnNumber);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

        public string GetIntegerDisplayValue(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

        public string GetIntegerDisplayValue(int rowNumber, int columnNumber)
        {
            string selectorString = string.Format("table[class='table'] tbody tr[data-r='{0}'] td[data-c='{1}'] span", rowNumber, columnNumber);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

        public string GetDecimalDisplayValue(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

        public string GetDecimalDisplayValue(int rowNumber, int columnNumber)
        {
            string selectorString = string.Format("table[class='table'] tbody tr[data-r='{0}'] td[data-c='{1}'] span", rowNumber, columnNumber);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

        public string GetDateDisplayValue(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}'] span", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

        public string GetDateDisplayValue(int rowNumber, int columnNumber)
        {
            string selectorString = string.Format("table[class='table'] tbody tr[data-r='{0}'] td[data-c='{1}'] span", rowNumber, columnNumber);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

        public string GetSelectDisplayValue(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

        public string GetRadioDisplayValue(string fieldId)
        {
            //The display-template structure of the radio field is identical to that of a
            //select field, so we can simply re-use the same helper function.
            return GetSelectDisplayValue(fieldId);
        }

        public string[] GetCheckboxDisplayValue(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}'] span", fieldId);
            var values = Driver.FindElements(By.CssSelector(selectorString))
                .Select(span => span.Text)
                .ToArray();

            return values;
        }

        public string GetTableColumnDisplayValue(string fieldId, string className)
        {
            string selectorString = string.Format("table[class='{0}'] tbody tr td[data-model-id='{1}'] span", className, fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }
        public string GetTableFooterColumnDisplayValue(string fieldId, string className)
        {
            string selectorString = string.Format("table[class='{0}'] tbody tr[class='footer-row'] td[data-model-id='{1}'] span", className, fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }
        public string GetAttachmentFieldDisplayValue(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}'] a", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string val = ele.Text;
            return val;
        }

    }
}
