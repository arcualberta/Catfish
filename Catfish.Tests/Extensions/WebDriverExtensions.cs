using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Catfish.Tests.Extensions
{
    public static class WebDriverExtensions
    {


        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            // OpenQA.Selenium.StaleElementReferenceException 
            if (timeoutInSeconds > 0)
            {
                Thread.Sleep(300);
                WebDriverWait wait = new WebDriverWait(driver,
                    TimeSpan.FromSeconds(timeoutInSeconds));
                wait.IgnoreExceptionTypes(
                    typeof(NoSuchElementException),
                    typeof(ElementNotVisibleException),
                    typeof(StaleElementReferenceException)
                    );
                return wait.Until(drv => drv.FindElement(by));                
            }
            return driver.FindElement(by);
        }

        //public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        //{
        //    // OpenQA.Selenium.StaleElementReferenceException 
        //    if (timeoutInSeconds > 0)
        //    {
        //        WebDriverWait wait = new WebDriverWait(driver,
        //            TimeSpan.FromSeconds(timeoutInSeconds));
        //        wait.IgnoreExceptionTypes(
        //            typeof(NoSuchElementException),
        //            typeof(ElementNotVisibleException),
        //            typeof(StaleElementReferenceException)
        //            );
        //        //return wait.Until(drv => drv.FindElement(by));
        //        return wait.Until(ExpectedConditions.ElementToBeClickable(by));
        //    }
        //    return driver.FindElement(by);
        //}



        public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new WebDriverWait(driver,
                    TimeSpan.FromSeconds(timeoutInSeconds));
                wait.IgnoreExceptionTypes(
                    typeof(NoSuchElementException),
                    typeof(ElementNotVisibleException),
                    typeof(StaleElementReferenceException)
                    );
                return wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
            }
            return driver.FindElements(by);
        }
        
    }
}

