using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Catfish.Test.Helpers
{
    partial class SeleniumHelper
    {
        public enum eDriverType { Chrome, Firefox, Edge };

        private readonly IConfiguration _configuration;
        private readonly string _siteUrl;

        public IWebDriver Driver { get; protected set; }
        //public FirefoxProfile ffp { get; private set; }

        public SeleniumHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _siteUrl = _configuration.GetSection("SiteUrl").Value.TrimEnd('/');
        }

        public IWebDriver SetDriver(eDriverType driverType)
        {
            switch (driverType)
            {
                case eDriverType.Chrome:
                    Driver = new ChromeDriver();
                    break;

                case eDriverType.Firefox:

                    FirefoxOptions ffopt = new FirefoxOptions();
                    ffopt.AcceptInsecureCertificates = true;
                    FirefoxProfile ffprofile = new FirefoxProfile();
                    ffprofile.AcceptUntrustedCertificates = true;
                    ffopt.Profile = ffprofile;

                    Driver = new FirefoxDriver(ffopt);
                    break;

                case eDriverType.Edge:
                    EdgeOptions Edgeopts = new EdgeOptions();
                    Edgeopts.AcceptInsecureCertificates = true;
                    //Edgeopts.UseChromium = true;   //if using older version of edge or explorer
                    Driver = new EdgeDriver(Edgeopts);
                    break;

                default:
                    throw new Exception("No driver found");
            }

            return Driver;
        }

        public void LoginLocal(string username = null, string password = null)
        {
            if (string.IsNullOrEmpty(username))
                username = _configuration.GetSection("Login:LocalAdmin:User").Value;

            if (string.IsNullOrEmpty(password))
                password = _configuration.GetSection("Login:LocalAdmin:Password").Value;
            Thread.Sleep(5000);
            Driver.Navigate().GoToUrl(_siteUrl);
            Thread.Sleep(5000);
            Driver.Navigate().GoToUrl(_siteUrl + "/manager");
            Driver.FindElement(By.Name("username")).SendKeys(username);
            Driver.FindElement(By.Name("password")).SendKeys(password);
            Driver.FindElement(By.Name("password")).Submit();

            // got to home page and make sure the sign-out button is there insetad of the sign-in button
            GoToUrl("/");
            List<IWebElement> elms = new List<IWebElement>();
            elms.AddRange(Driver.FindElements(By.Id("btnSignIn")));
            Assert.IsTrue(elms.Count == 0, "Error: a sign-in button was found after log-in");

        }

        public void GoToUrl(string path)
        {
            if (!path.StartsWith('/'))
                path = "/" + path;
            Driver.Navigate().GoToUrl(_siteUrl + path);
        }


        // upload file when accessing button by Id.
        // 



        public void UpLoadFile(string fieldId, string fileName)
        {
            var attachmentPath = _configuration.GetSection("AttachmentPath").Value;
            var path = Path.Combine(attachmentPath, fileName);
            //WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            //var ele = wait.Until(drv => drv.FindElement(By.Id(id)));


            string selectorString = string.Format("input[type='file'][data-model-id='{0}']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));

            //ele.Click();
            //Thread.Sleep(2000);
            ele.SendKeys(path);
            //ele.SendKeys("\r");

            //Thread.Sleep(2000);

        }

        //ClickAddButton
        public void ClickAddButton(string selectorName)
        {
            var ele = Driver.FindElement(By.CssSelector(selectorName));
            ele.Click();
        }



        /// <summary>
        /// Select the field identified by data-model-id=fieldId and then selects its option
        /// identified by value=optionId
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="optionId"></param>
        public void SelectDropdownOption(string fieldId, string optionId)
        {
            string selectorString = string.Format("select[data-model-id='{0}'] option[value='{1}']", fieldId, optionId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }

        public void SelectCheckOption(string fieldId, string optionId)
        {
            string selectorString = string.Format("input[type='checkbox'][data-model-id='{0}'][data-option-id='{1}']", fieldId, optionId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }

        public void SelectCheckOption(string optionId, int rowNumber, int columnNumber)
        {
            string selectorString = string.Format("table[class='table table-field tf-data'] tbody tr[data-r='{0}'] td input[type='checkbox'][data-c='{1}'][data-option-id='{2}']", rowNumber, columnNumber, optionId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }

        public void SelectCheckOptions(string fieldId, string[] optionIds)
        {
            for (int i = 0; i < optionIds.Length; ++i)
                SelectCheckOption(fieldId, optionIds[i]);
        }

        public void SelectCheckOptions(string[] optionIds, int rowNumber, int columnNumber)
        {
            for (int i = 0; i < optionIds.Length; ++i)
                SelectCheckOption(optionIds[i], rowNumber, columnNumber);
        }

        public void SelectCompositeCheckOption(string optionId, int childNumber, int rowNumber)
        {
            string selectorString = string.Format("[data-cf-item-index='{0}'] [data-field-index='{1}'][data-option-id='{2}']", childNumber, rowNumber, optionId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }
        public void SelectCompositeCheckOptions(string[] optionIds, int childNumber, int rowNumber)
        {
            for (int i = 0; i < optionIds.Length; ++i)
                SelectCompositeCheckOption(optionIds[i], childNumber, rowNumber);
        }

        public void SelectRadioOption(string fieldId, string optionId)
        {
            string selectorString = string.Format("input[type='radio'][data-model-id='{0}'][value='{1}']", fieldId, optionId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }
        
        public void SelectRadioOption(string optionId, int rowNumber, int columnNumber)
        {
            string selectorString = string.Format("table[class='table table-field tf-data'] tbody tr[data-r='{0}'] td div[id='{2}'] input[type='radio'][data-c='{1}'][value='{2}']", rowNumber, columnNumber, optionId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }

        public void SelectCompositeRadioOption(string optionId, int childNumber, int rowNumber)
        {
            string selectorString = string.Format("[data-cf-item-index='{0}'] [data-field-index='{1}'][value='{2}']", childNumber, rowNumber, optionId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }

        public void SetTextFieldValue(string fieldId, string value)
        {
            string selectorString = string.Format("input[type='text'][data-model-id='{0}']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Clear();
            ele.SendKeys(value);
        }
        /// <summary>
        /// This funtions set table filed text values
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="value"></param>
        /// <param name="rowNumber"></param>
        public void SetTextFieldValue(string value, int rowNumber, int columnNumber)
        {
            string selectorString = string.Format("table[class='table table-field tf-data'] tbody tr[data-r='{0}'] td input[data-c='{1}'][type='text']", rowNumber, columnNumber);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Clear();
            ele.SendKeys(value);
        }

        public void SetCompositeTextFieldValue(string value, int childNumber, int rowNumber)
        {
            string selectorString = string.Format("[data-cf-item-index='{0}'] [data-field-index='{1}']", childNumber, rowNumber);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Clear();
            ele.SendKeys(value);
        }

        public void SetNumberValue(string fieldId, string value)
        {
            string selectorString = string.Format("input[type='number'][data-model-id='{0}']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Clear();
            ele.SendKeys(value);
            ele.SendKeys("\t");
        }

        public void SetNumberValue(string value, int rowNumber, int columnNumber)
        {
            string selectorString = string.Format("table[class='table table-field tf-data'] tbody tr[data-r='{0}'] td input[data-c='{1}'][type='number']", rowNumber, columnNumber);

            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Clear();
            ele.SendKeys(value);
            ele.SendKeys("\t");
        }

        public void SetDateValue(string fieldId, DateTime date)
        {
            string selectorString = string.Format("input[type='date'][data-model-id='{0}']", fieldId);
            SetDate(selectorString, date);
        }
        public void SetDateValue(DateTime date, int rowNumber, int columnNumber)
        {
            string selectorString = string.Format("table[class='table table-field tf-data'] tbody tr[data-r='{0}'] td input[data-c='{1}'][type='date']", rowNumber, columnNumber);
            SetDate(selectorString, date);
        }

        public void SetCompositeDateValue(DateTime date, int childNumber, int rowNumber)
        {
            string selectorString = string.Format("[data-cf-item-index='{0}'] [data-field-index='{1}']", childNumber, rowNumber);
            SetDate(selectorString, date);
        }

        private void SetDate(string selectorString, DateTime date)
        {
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Clear();
            ele.SendKeys(date.Year.ToString());
            ele.SendKeys("\t");
            ele.SendKeys(date.Month.ToString());
            ele.SendKeys(date.Day.ToString());
        }

        public void SetTextAreaValue(string fieldId, string value)
        {
            string selectorString = string.Format("textarea[data-model-id='{0}']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Clear();
            ele.SendKeys(value);

        }

        public void SetTextAreaValue(string value, int rowNumber, int columnNumber)
        {
            string selectorString = string.Format("table[class='table table-field tf-data'] tbody tr[data-r='{0}'] td textarea[data-c='{1}']", rowNumber, columnNumber);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Clear();
            ele.SendKeys(value);

        }

        public void ClickOnButtonType(string dataItemTemplateId, string buttonValue)
        {
            string selectorString = string.Format("[data-template-id='{0}'] [type='button'][value='{1}']", dataItemTemplateId, buttonValue);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }

        public void ClickSaveButton(string dataItemTemplateId, string buttonValue)
        {
            string selectorString = string.Format("form[data-template-id='{0}'] input[type='submit'][value='{1}']", dataItemTemplateId, buttonValue);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }

        public void ClickTableRowDeleteButton(int rawNumber)
        {
            string selectorString = string.Format("table[class='table table-field tf-data'] tbody tr[data-r='{0}'] th div span", rawNumber);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }

        public void CkickModalSubmit(string dataItemTemplateId, string cssClass)
        {
            Thread.Sleep(2000);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            string selectorString = string.Format("form[data-template-id='{0}'] div[id='buttonLayer'] button[class='{1}']", dataItemTemplateId, cssClass);
            var ele = wait.Until(drv => drv.FindElement(By.CssSelector(selectorString)));
            ele.Click();
        }


        public void ClickSimpleSubmitButton()
        {
            string selectorString = string.Format("#Submit");
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }

        public void ClickModalSubmitButton()
        {
            Thread.Sleep(2000);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(55));
            //string selectorString = string.Format("form[data-template-id='{0}'] div[id='buttonLayer'] button[class='{1}']", dataItemTemplateId, cssClass);
            var ele = wait.Until(drv => drv.FindElement(By.ClassName("btn-success")));

            ele.Click();
        }

        public void ClickOnALink(string linkText)
        {
            Thread.Sleep(5000);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(100));
            var ele = wait.Until(drv => drv.FindElement(By.LinkText(linkText)));
            ele.Click();
        }

        public void ClickOnRowDeleteButton(int childNumber, string cssClass)
        {
            string selectorString = string.Format("[data-cf-item-index='{0}'] [class='{1}']", childNumber, cssClass);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }

        public string GetTableSummaryColumnSum(string fieldId)
        {
            Thread.Sleep(2000);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            string selectorString = string.Format("table[class='table table-field tf-data'] tbody tr[class='footer-row first-footer-row'] td input[data-model-id='{0}']", fieldId);
            var ele = wait.Until(drv => drv.FindElement(By.CssSelector(selectorString))); 
            return ele.GetAttribute("value");
        }
        
        public string GetTableRowColumnSum(int rowNumber, int columnNumber)
        {
            //Thread.Sleep(2000);
            //WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            string selectorString = string.Format("table[class='table table-field tf-data'] tbody tr[data-r='{0}'] td input[data-c='{1}']", rowNumber, columnNumber);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            return ele.GetAttribute("value");
        }

        public bool IsTextFieldRequired(string fieldId)
        {
            Thread.Sleep(2000);
            string selectorString = string.Format("input[data-model-id='{0}']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            string isRequired = ele.GetAttribute("required");
            if (isRequired == "true")
                return true;
            else
                return false;
        }

        public string GetSummaryFieldValue(string fieldId)
        {
            string selectorString = string.Format("input[data-model-id='{0}']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            return ele.GetAttribute("value");
        }

        //public bool IsTextFieldNotRequired(string fieldId)
        //{
        //    string selectorString = string.Format("input[data-model-id='{0}']", fieldId);
        //    var ele = Driver.FindElement(By.CssSelector(selectorString));
        //    if (!string.IsNullOrWhiteSpace(ele.ToString()))
        //        return true;
        //    else
        //        return false;
        //}
    }
}
