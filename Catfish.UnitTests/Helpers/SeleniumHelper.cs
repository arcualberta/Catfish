using Microsoft.Edge.SeleniumTools;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Catfish.UnitTests.Helpers
{
    public class SeleniumHelper
    {
        public enum eDriverType { Chrome, Firefox, Edge };

        private readonly IConfiguration _configuration;
        private readonly string _siteUrl;

        public IWebDriver Driver { get; protected set; }

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

        public IWebElement GetElementByDataModelId(string id)
        {
            throw new NotImplementedException();
        }

        public IWebElement GetElementByValue (string val)
        {
            throw new NotImplementedException();

        }


        /// <summary>
        /// Select the field identified by data-model-id=fieldId and then selects its option
        /// identified by value=optionId
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="optionId"></param>
        public void SelectDropdownOption(string fieldId, string optionId)
        {
            var dd = GetElementByDataModelId(fieldId);

            var option = GetElementByValue(optionId);

        }

        public void SelectCheckboxOption(string fieldId, string optionId)
        {
            var chk = GetElementByDataModelId(fieldId);

            var option = GetElementByValue(optionId);

        }

        public void SelectRadioOption(string fieldId, string optionId)
        {
            var dd = GetElementByDataModelId(fieldId);

            var option = GetElementByValue(optionId);

        }

        public void SetTextFieldValue(string fieldId, string value)
        {

        }

        public void SetTextAreaValue(string fieldId, string value)
        {

        }

        public void ClickButton (string buttonId)
        {

        }

    }
}
