using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Catfish.UnitTests.Helpers
{
    public partial class SeleniumHelper
    {
        public string GetTextFieldDisplayValue(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}'] div[class='multilingual-input-block']", fieldId);
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

        public string GetIntegerDisplayValue(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}']", fieldId);
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

        public string GetDateDisplayValue(string fieldId)
        {
            string selectorString = string.Format("div[data-model-id='{0}'] span", fieldId);
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

    }
}
