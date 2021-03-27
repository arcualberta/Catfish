using Microsoft.Edge.SeleniumTools;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;

namespace Catfish.UnitTests.Helpers
{
    public class SeleniumHelper
    {
        public enum eDriverType { Chrome, Firefox, Edge };
        ////public enum eElementType 
        ////{
        ////    Button,
        ////    Checkbox,
        ////    Date,
        ////    Div,
        ////    Hidden,
        ////    Label,
        ////    Li,
        ////    Number,
        ////    Option,
        ////    Radio,
        ////    Span,
        ////    Select,
        ////    Submit,
        ////    TextArea,
        ////    TextBox,
        ////    Ul
        ////}

        private readonly IConfiguration _configuration;
        private readonly string _siteUrl;

        public IWebDriver Driver { get; protected set; }

        public SeleniumHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _siteUrl = _configuration.GetSection("SiteUrl").Value.TrimEnd('/');
        }
        ////public string GetTagName(eElementType type)
        ////{
        ////    switch (type)
        ////    {
        ////        case eElementType.Button:
        ////            return "button";
        ////        case eElementType.Checkbox:
        ////        case eElementType.Date:
        ////        case eElementType.Number:
        ////        case eElementType.Radio:
        ////        case eElementType.Submit:
        ////        case eElementType.TextBox:
        ////            return "input";
        ////        case eElementType.Div:
        ////            return "div";
        ////        case eElementType.Select:
        ////            return "select";
        ////        case eElementType.Option:
        ////            return "option";
        ////        case eElementType.Label:
        ////            return "label";
        ////        case eElementType.Li:
        ////            return "li";
        ////        case eElementType.Span:
        ////            return "span";
        ////        case eElementType.TextArea:
        ////            return "textarea";
        ////        case eElementType.Ul:
        ////            return "ul";
        ////    }

        ////    throw new Exception(string.Format("Unknown element type: {0}", type.ToString()));
        ////}

        public IWebDriver SetDriver(eDriverType driverType)
        {
            switch (driverType)
            {
                case eDriverType.Chrome:
                    Driver = new ChromeDriver();
                    break;
                case eDriverType.Firefox:
                    Driver = new FirefoxDriver();
                    break;
                case eDriverType.Edge:
                    Driver = new EdgeDriver();
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

        /////// <summary>
        /////// Use the "Driver" property of this instance and find the element that has the
        /////// "value" attribute to the value of the given "val" parameter and return it.
        /////// </summary>
        /////// <param name="val"></param>
        /////// <returns></returns>
        ////public IWebElement GetElementByValue (string val, eElementType elementType, IWebElement parent = null)
        ////{
        ////    string tag = GetTagName(elementType);
        ////    string selectorString = string.Format("{0}[value='{1}']", tag, val);
        ////    IWebElement e = parent == null
        ////       ? Driver.FindElement(By.CssSelector(selectorString))
        ////       : parent.FindElement(By.CssSelector(selectorString));
        ////    return e;
        ////}

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

        public void SelectCheckOptions(string fieldId, string[] optionIds)
        {
            for (int i = 0; i < optionIds.Length; ++i)
                SelectCheckOption(fieldId, optionIds[i]);
        }

        public void SelectRadioOption(string fieldId, string optionId)
        {
            string selectorString = string.Format("input[type='radio'][data-model-id='{0}'][value='{1}']", fieldId, optionId);
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

        public void SetNumberValue(string fieldId, string value)
        {
            string selectorString = string.Format("input[type='number'][data-model-id='{0}']", fieldId);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Clear();
            ele.SendKeys(value);
        }

        public void SetDateValue(string fieldId, DateTime date)
        {
            string selectorString = string.Format("input[type='date'][data-model-id='{0}']", fieldId);
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

        public void ClickSubmitButton (string dataItemTemplateId, string buttonValue)
        {
            string selectorString = string.Format("form[data-template-id='{0}'] input[type='button'][value='{1}']", dataItemTemplateId, buttonValue);
            var ele = Driver.FindElement(By.CssSelector(selectorString));
            ele.Click();
        }

        public void ModalSubmit(string dataItemTemplateId)
        {
            Thread.Sleep(2000);
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            string selectorString = string.Format("form[data-template-id='{0}'] div[id='buttonLayer'] button[class='btn btn-success']", dataItemTemplateId);
            var ele = wait.Until(drv => drv.FindElement(By.CssSelector(selectorString)));
            ele.Click();
        }

    }
}
