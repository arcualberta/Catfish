using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Configuration;
using OpenQA.Selenium.Support.UI;
using Catfish.Core.Models;
using System.Data;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Catfish.Core.Models.Access;

namespace Catfish.Tests.Views
{

    [TestFixture(typeof(ChromeDriver))]
    public class AccessIntegration<TWebDriver> : BaseIntegration<TWebDriver> where TWebDriver : IWebDriver, new()
    {

        // values
        const string readWriteModeLabel = "Read Write mode";

        private string GetAccessModesLabel(AccessMode accessModes)
        {
            return String.Join(" - ", accessModes.AsStringList());
        }

        [Test]
        public void CanCreateAccessDefinition()
        {
            AccessMode modes = AccessMode.Write | AccessMode.Read;

            AccessDefinitionParameters parameters = new AccessDefinitionParameters
            {
                Modes = modes,
                AccessModesLabel = GetAccessModesLabel(modes)
            };

            CreateAccessDefinition(parameters);

            // Assertions for creation
            // Check for last table row which contains latest access definition
            IReadOnlyList<IWebElement> result = Driver.FindElements(By.CssSelector("tr:last-child td"));
            Assert.GreaterOrEqual(result.Count, 2);
            Assert.AreEqual(parameters.AccessModesLabel, result[0].Text);

            // second td element has list of access modes
            List<string> retreivedModes = result[1].Text.Split(',').Select(x => x.Trim()).ToList();
            List<string> allModes = modes.AsStringList();

            foreach (string currentMode in retreivedModes)
            {
                Assert.Contains(currentMode, allModes);
            }

            // Destroy access definition
            int rowsBeforeDeletion = Driver.FindElements(By.TagName("tr")).Count();
            // result[4] has the delete button
            result[4].Click();
            Driver.SwitchTo().Alert().Accept();
            WaitPageSourceChange(5, 500);
            int rowsAfterDeletion = Driver.FindElements(By.TagName("tr")).Count();
            
            // Check if it was detroyed
            Assert.AreEqual(rowsBeforeDeletion - 1, rowsAfterDeletion);            

        }

        [Test]
        public void TestMethods()
        {
            //CreateItem("item name now this is an itemr");
            AccessMode modes = AccessMode.Write;
            AccessDefinitionParameters accessDef = new AccessDefinitionParameters()
            {
                Modes = modes,
                AccessModesLabel = GetAccessModesLabel(modes)
            };
            CreateAccessDefinition(accessDef);
            
            Assert.IsTrue(true);
        }
    }
}
