using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Tests.IntegrationTests.Helpers;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Catfish.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using Catfish.Services;

namespace Catfish.Tests.IntegrationTests.Regions
{
    [TestFixture(typeof(ChromeDriver))]
    class AdvancedSearchPanelTest<TWebDriver> : BaseIntegrationTest<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        public static readonly string[] CategoryNames = { "One 1", "Two 2", "Three 3" };
        public static string[] MetadataSetNames = 
        {
            "BasicMetadata"
        };

        public static FormField[] MetadataFields = {
            new TextField() { Name = "Name" },
            new NumberField() { Name = "Amount" },
            new NumberField() { Name = "Year" },
            new DropDownMenu() { Name = "Category", Options = new List<Option>(){
                new Option() { Value = { new TextValue("en", "English", CategoryNames[0]) } },
                new Option() { Value = { new TextValue("en", "English", CategoryNames[1]) } },
                new Option() { Value = { new TextValue("en", "English", CategoryNames[2]) } },
            }.AsReadOnly() }
        };

        public static CFEntityType.eTarget[] TargetTypes =
        {
            CFEntityType.eTarget.Items
        };

        public void CreateSearchEntityType()
        {
            MetadataFields[3].Serialize();
            CreateMetadataSet(MetadataSetNames[0], "The base metadata set for advanced search", MetadataFields);

            CreateEntityType(EntityTypeName, "Entity type used for testing advanced search", MetadataSetNames, TargetTypes,
                new Tuple<string, FormField>[] {
                    new Tuple<string, FormField>(MetadataSetNames[0], MetadataFields[1]),
                    new Tuple<string, FormField>(MetadataSetNames[0], MetadataFields[2]),
                    new Tuple<string, FormField>(MetadataSetNames[0], MetadataFields[3])
                });
        }

        private void CreateItems(int count, Func<int, int> yearFunction, Func<int, float> amountFunction, Func<int, string> optionFunc, Func<int, string> nameFunction)
        {
            MetadataFields[0].Serialize();
            MetadataFields[1].Serialize();
            MetadataFields[2].Serialize();
            MetadataFields[3].Serialize();

            FormField[] fields = new FormField[4];

            for (int i = 0; i < count; ++i)
            {
                
                fields[0] = new TextField() { Content = MetadataFields[0].Content };
                fields[0].SetValues(new string[]{ nameFunction(i) });

                fields[1] = new NumberField() { Content = MetadataFields[1].Content };
                fields[1].SetValues(new string[] { amountFunction(i).ToString() });

                fields[2] = new NumberField() { Content = MetadataFields[2].Content };
                fields[2].SetValues(new string[] { yearFunction(i).ToString() });

                fields[3] = new DropDownMenu() { Content = MetadataFields[3].Content };
                fields[3].SetValues(new string[] { optionFunc(i) });
                ((DropDownMenu)fields[3]).UpdateValues(fields[3]);

                CreateItem(EntityTypeName, fields, false);
            }
        }

        [Test]
        public void CanSearch()
        {
            CreateSearchEntityType();

            CreateAndAddAddvancedSearchToMain(true, new FormField[]{ MetadataFields[0], MetadataFields[1], MetadataFields[2] }, 1);
            CreateAndAddEntityListToMain(2);

            Func<int, int> yearFunc = i => i + 2000;
            Func<int, float> amountFunc = i => (float)(Math.Sin((double)i) + 2.0);
            Func<int, string> nameFunc = i => "Item Entry " + i;
            Func<int, string> optionFunc = i => null;

            CreateItems(10, yearFunc, amountFunc, optionFunc, nameFunc);

            List<string> nameList = new List<string>();
            for(int i = 0; i < 10; ++i)
            {
                nameList.Add(nameFunc(i));
            }

            Driver.Navigate().GoToUrl(FrontEndUrl);
            AssertItemsNameShows(nameList);

            // Search on the year
            int minYear = 2001;
            int maxYear = 2005;
            for(int i = 9; i >= 0; --i)
            {
                int year = yearFunc(i);
                if (year < minYear || year > maxYear)
                {
                    nameList.RemoveAt(i);
                }
            }

            IWebElement field = Driver.FindElements(By.ClassName("search-entry"))[3];
            field.FindElement(By.ClassName("search-from")).SendKeys(minYear.ToString());
            field.FindElement(By.ClassName("search-to")).SendKeys(maxYear.ToString());
            
            Driver.FindElement(By.ClassName("search-button")).Click();

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20.0));
            wait.Until(driver => driver.FindElements(By.ClassName("loading-panel")).Count == 0);
            AssertItemsNameShows(nameList);

            // Reload the page
            Driver.Navigate().Refresh();
            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20.0));
            wait.Until(driver => driver.FindElements(By.ClassName("loading-panel")).Count == 0);
            AssertItemsNameShows(nameList);
        }

        [Test]
        public void CanGenerateLineChart()
        {
            CreateSearchEntityType();

            CreateAndAddAddvancedSearchToMain(true, new FormField[] { MetadataFields[0], MetadataFields[1], MetadataFields[2] }, 1);
            CreateAndAddGraphToMain("Year", MetadataSetNames[0], MetadataFields[2].Name, "Amount", MetadataSetNames[0], MetadataFields[1].Name, 1, 1, null, null, 2);

            Func<int, int> yearFunc = i => (i % 10) + 2000;
            Func<int, float> amountFunc = i => (float)((Math.Sin((double)i) + 1.0) * i * 0.5);
            Func<int, string> nameFunc = i => "Item Entry " + i;
            Func<int, string> optionFunc = i => CategoryNames[i % 3];

            CreateItems(20, yearFunc, amountFunc, optionFunc, nameFunc);

            List<float> amountList = new List<float>();
            List<int> yearList = new List<int>();
            for (int i = 0; i < 20; ++i)
            {
                amountList.Add(amountFunc(i));
            }

            for(int i = 0; i < 10; ++i)
            {
                yearList.Add(i + 2000);
            }

            Driver.Navigate().GoToUrl(FrontEndUrl);
            AssertGraphIsCorrect(amountList, yearList);

            // Search on the year
            int minYear = 2001;
            int maxYear = 2005;
            for (int i = 9; i >= 0; --i)
            {
                int year = yearFunc(i);
                if (year < minYear || year > maxYear)
                {
                    amountList.RemoveAt(i);
                }
            }

            yearList.Clear();
            for (int i = 1; i < 6; ++i)
            {
                yearList.Add(i + 2000);
            }

            IWebElement field = Driver.FindElements(By.ClassName("search-entry"))[3];
            field.FindElement(By.ClassName("search-from")).SendKeys(minYear.ToString());
            field.FindElement(By.ClassName("search-to")).SendKeys(maxYear.ToString());

            Driver.FindElement(By.ClassName("search-button")).Click();
            
            AssertGraphIsCorrect(amountList, yearList);

            // Reload the page
            Driver.Navigate().Refresh();
            AssertGraphIsCorrect(amountList, yearList);
        }

        [Test]
        public void CanGenerateCatagorizedLineChart()
        {
            CreateSearchEntityType();

            CreateAndAddAddvancedSearchToMain(true, MetadataFields, 1);
            CreateAndAddGraphToMain("Year", MetadataSetNames[0], MetadataFields[2].Name, "Amount", MetadataSetNames[0], MetadataFields[1].Name, 1, 1, MetadataSetNames[0], MetadataFields[3].Name, 2);

            Func<int, int> yearFunc = i => (i % 10) + 2000;
            Func<int, float> amountFunc = i => (float)((Math.Sin((double)i) + 1.0) * i * 0.5);
            Func<int, string> nameFunc = i => "Item Entry " + i;
            Func<int, string> optionFunc = i => CategoryNames[i % 3];

            CreateItems(20, yearFunc, amountFunc, optionFunc, nameFunc);

            List<float> amountList = new List<float>();
            List<int> yearList = new List<int>();
            for (int i = 0; i < 20; ++i)
            {
                amountList.Add(amountFunc(i));
            }

            for (int i = 0; i < 10; ++i)
            {
                yearList.Add(i + 2000);
            }

            Driver.Navigate().GoToUrl(FrontEndUrl);
            AssertGraphIsCorrect(amountList, yearList, CategoryNames);

            // Search on the year
            int minYear = 2001;
            int maxYear = 2005;
            for (int i = 9; i >= 0; --i)
            {
                int year = yearFunc(i);
                if (year < minYear || year > maxYear)
                {
                    amountList.RemoveAt(i);
                }
            }

            yearList.Clear();
            for (int i = 1; i < 6; ++i)
            {
                yearList.Add(i + 2000);
            }

            IWebElement field = Driver.FindElements(By.ClassName("search-entry"))[3];
            field.FindElement(By.ClassName("search-from")).SendKeys(minYear.ToString());
            field.FindElement(By.ClassName("search-to")).SendKeys(maxYear.ToString());

            Driver.FindElement(By.ClassName("search-button")).Click();

            AssertGraphIsCorrect(amountList, yearList, CategoryNames);

            // Reload the page
            Driver.Navigate().Refresh();
            AssertGraphIsCorrect(amountList, yearList, CategoryNames);
        }

        [Test]
        public void CalculatedPanelTest()
        {
            CreateSearchEntityType();

            CreateAndAddAddvancedSearchToMain(true, MetadataFields, 1);
            CreateAndAddCalculationToMain(2, "Count Panel", "countPanel", ItemQueryService.eFunctionMode.COUNT, "Count", MetadataSetNames[0], MetadataFields[0].Name, "", 0, MetadataSetNames[0], MetadataFields[0].Name);
            CreateAndAddCalculationToMain(3, "Max Panel", "maxPanel", ItemQueryService.eFunctionMode.MAX, "Max Amount", MetadataSetNames[0], MetadataFields[1].Name);
            CreateAndAddCalculationToMain(4, "Mean Panel", "meanPanel", ItemQueryService.eFunctionMode.MEAN, "Mean Amount", MetadataSetNames[0], MetadataFields[1].Name);
            CreateAndAddCalculationToMain(5, "Median Panel", "medianPanel", ItemQueryService.eFunctionMode.MEDIAN, "Median Amount", MetadataSetNames[0], MetadataFields[1].Name);
            CreateAndAddCalculationToMain(6, "Min Panel", "minPanel", ItemQueryService.eFunctionMode.MIN, "Min Amount", MetadataSetNames[0], MetadataFields[1].Name);
            CreateAndAddCalculationToMain(7, "Standard Deviation Panel", "deviationPanel", ItemQueryService.eFunctionMode.STANDARD_DEVIATION, "Standard Deviation Amount", MetadataSetNames[0], MetadataFields[1].Name);
            CreateAndAddCalculationToMain(8, "Sum Panel", "sumPanel", ItemQueryService.eFunctionMode.SUM, "Sum Amount", MetadataSetNames[0], MetadataFields[1].Name);

            Func<int, int> yearFunc = i => (i % 10) + 2000;
            Func<int, float> amountFunc = i => (float)((Math.Sin((double)i) + 1.0) * i * 0.5);
            Func<int, string> nameFunc = i => "Item Entry " + (i % 3);
            Func<int, string> optionFunc = i => CategoryNames[i % 3];

            CreateItems(20, yearFunc, amountFunc, optionFunc, nameFunc);

            List<float> amountList = new List<float>();
            List<string> nameList = new List<string>();
            for (int i = 0; i < 20; ++i)
            {
                amountList.Add(amountFunc(i));
                nameList.Add(nameFunc(i));
            }

            Driver.Navigate().GoToUrl(FrontEndUrl);
            AssertCalculationPanelsAreCorrect(nameList, amountList);

            // Search on the year
            int minYear = 2001;
            int maxYear = 2005;
            for (int i = 9; i >= 0; --i)
            {
                int year = yearFunc(i);
                if (year < minYear || year > maxYear)
                {
                    amountList.RemoveAt(i);
                    nameList.RemoveAt(i);
                }
            }

            IWebElement field = Driver.FindElements(By.ClassName("search-entry"))[3];
            field.FindElement(By.ClassName("search-from")).SendKeys(minYear.ToString());
            field.FindElement(By.ClassName("search-to")).SendKeys(maxYear.ToString());

            Driver.FindElement(By.ClassName("search-button")).Click();

            AssertCalculationPanelsAreCorrect(nameList, amountList);

            // Reload the page
            Driver.Navigate().Refresh();
            AssertCalculationPanelsAreCorrect(nameList, amountList);
        }

        private void AssertCalculationPanelsAreCorrect(IEnumerable<string> names, IEnumerable<float> amounts)
        {
            // Check each calculated field.
            ICollection<IWebElement> elements = Driver.FindElements(By.ClassName("calculatedField"));

            IEnumerable<decimal> amountRound = amounts.Select(a => Math.Round((decimal)a, 2));
            int count = names.Distinct().Count();
            decimal min = amountRound.Min();
            decimal max = amountRound.Max();
            decimal mean = amountRound.Average();
            decimal median = amountRound.ElementAt(amountRound.Count() >> 1);
            decimal sum = amountRound.Sum();

            AssertCalculationPanelIsCorrect(elements.ElementAt(0), count, 0);
            AssertCalculationPanelIsCorrect(elements.ElementAt(1), max);
            AssertCalculationPanelIsCorrect(elements.ElementAt(2), mean, 0.55m);
            AssertCalculationPanelIsCorrect(elements.ElementAt(3), median, 0.55m);
            AssertCalculationPanelIsCorrect(elements.ElementAt(4), min);

            AssertCalculationPanelIsCorrect(elements.ElementAt(6), sum, 0.1m);
        }

        private string ConstructTestValue(decimal value, string prefix = "$", int decimalCount = 2)
        {
            string format = "#,##0";

            if(decimalCount > 0)
            {
                format += ".";

                for(int i = 0; i < decimalCount; ++i)
                {
                    format += "0";
                }
            }

            return prefix + value.ToString(format);
        }

        private void AssertCalculationPanelIsCorrect(IWebElement element, decimal value, decimal epsilon = 0.25m)
        {
            Assert.NotNull(element);

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20.0));
            wait.Until(driver => !element.FindElement(By.XPath("./div[2]")).Text.Contains("Loading"));

            decimal testResult = decimal.Parse(element.FindElement(By.XPath("./div[2]")).Text.Replace("$", "").Replace(",", ""));

            Assert.LessOrEqual(Math.Abs(testResult - value), epsilon);
        }

        private void AssertGraphIsCorrect(IEnumerable<float> amounts, IEnumerable<int> years, IEnumerable<string> categories = null)
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20.0));
            wait.Until(driver => driver.FindElements(By.ClassName("loading-panel")).Count == 0);

            // Check if the axis is correct.
            IEnumerable<IWebElement> dateElements = Driver.FindElements(By.CssSelector("g.axis-y > g.tick > text"));
            Assert.Greater(dateElements.Count(), 0);

            IEnumerable<IWebElement> amountElements = Driver.FindElements(By.CssSelector("g.axis-x > g.tick > text"));
            Assert.Greater(amountElements.Count(), 0);

            // Check the categories
            IWebElement legend = Driver.FindElement(By.ClassName("legend"));
            IEnumerable<IWebElement> categoryElements = legend.FindElements(By.ClassName("legend-item"));
            string[] categoryElementNames = categoryElements.Select(e => e.FindElement(By.XPath(".//span")).Text.Trim()).ToArray();

            if (categories == null)
            {
                Assert.AreEqual(1, categoryElements.Count());
                Assert.AreEqual(1, categoryElementNames.Count());
                Assert.AreEqual("", categoryElementNames[0]);
            }
            else { 
                Assert.AreEqual(categories.Count(), categoryElements.Count());

                foreach (string category in categories)
                {
                    Assert.Contains(category, categoryElementNames);
                }
            }
        }

        private void AssertItemsNameShows(IEnumerable<string> itemNames)
        {
            string tableRowsXpath = "//tbody[@id = 'ListEntitiesPanelTableBody']/tr";
            
            List<IWebElement> rows = Driver.FindElements(By.XPath(tableRowsXpath), 10).ToList();
            Assert.AreEqual(itemNames.Count(), rows.Count);

            int i = 0;
            foreach(string name in itemNames)
            {
                Assert.AreEqual(rows[i].FindElement(By.ClassName("column-1")).Text, name);
                ++i;
            }
        }
    }
}
