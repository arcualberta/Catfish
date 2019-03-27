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

        public static IWebElement FindElement(
            this IWebDriver driver, 
            By by, 
            int timeoutInSeconds,
            int threadSleepMicroSeconds = 1500)
        {
            if (timeoutInSeconds > 0)
            {
                Thread.Sleep(threadSleepMicroSeconds);
                WebDriverWait wait = new WebDriverWait(driver,
                    TimeSpan.FromSeconds(timeoutInSeconds));
                wait.IgnoreExceptionTypes(
                    typeof(NoSuchElementException),
                    typeof(ElementNotVisibleException),
                    typeof(StaleElementReferenceException)
                    );
                //return wait.Until(drv => drv.FindElement(by));
                IWebElement element = wait.Until(drv => drv.FindElement(by));
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView(false);", element);
                return element;
            }
            return driver.FindElement(by);
        }

        // Web driver extensions

        public static IWebElement FindElement(
            this IWebDriver driver,
            By by,
            Action jsTrigger = null,
            int timeoutInSeconds = 5)
        {
            if (jsTrigger == null)
            {
                return driver.FindElement(by);
            }
            TimeSpan timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            WebDriverWait wait = new WebDriverWait(driver, timeout);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
            IWebElement element = driver.FindElement(by);
            jsTrigger?.Invoke();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(element));
            element = driver.FindElement(by);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView(false);", element);
            return element;
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

        public static ReadOnlyCollection<IWebElement> FindElements(
            this IWebDriver driver, 
            By by, 
            int timeoutInSeconds, 
            int threadSleepMicroSeconds = 1000)
        {
            if (timeoutInSeconds > 0)
            {
                Thread.Sleep(threadSleepMicroSeconds);
                WebDriverWait wait = new WebDriverWait(driver,
                    TimeSpan.FromSeconds(timeoutInSeconds));
                wait.IgnoreExceptionTypes(
                    typeof(NoSuchElementException),
                    typeof(ElementNotVisibleException),
                    typeof(StaleElementReferenceException)
                    );
                return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.PresenceOfAllElementsLocatedBy(by));
            }
            return driver.FindElements(by);
        }
        
    }
}

