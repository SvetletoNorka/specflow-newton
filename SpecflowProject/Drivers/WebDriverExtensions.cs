﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SpecflowProject.Drivers
{
    public class WebDriverExtensions
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public WebDriverExtensions(IWebDriver driver, TimeSpan waitTime)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, waitTime);
        }

        public void FindAndClick(By by)
        {
            // Wait for the element to be clickable
            var element = _wait.Until(drv =>
            {
                var elem = drv.FindElement(by);
                return (elem.Displayed && elem.Enabled) ? elem : null;
            });

            //Click the element
            element.Click();
        }

        public HashSet<string> CollectElementsUntil(By scrollingElements, int timeoutInSeconds = 10)
        {
            HashSet<string> uniqueTexts = new HashSet<string>();
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            bool reachedStopElement = false;

            while (!reachedStopElement)
            {
                IList<IWebElement> pageElements = _driver.FindElements(scrollingElements);

                foreach (var element in pageElements)
                {
                    uniqueTexts.Add(element.Text);
                }

                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                try
                {
                    wait.Until(d =>
                    {
                        var newPageElements = d.FindElements(scrollingElements);
                        return newPageElements.Count > pageElements.Count;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    reachedStopElement = true;
                    break;
                }
            }

            return uniqueTexts;
        }

        public void ScrollUpUntilElementFound(By by, int timeoutInSeconds = 10)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            IWebElement element = _driver.FindElement(by);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public IWebElement ScrollAndFindElement(By by, int timeoutInSeconds = 10)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));

            IWebElement element = wait.Until(driver => driver.FindElement(by));

            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);

            wait.Until(driver => element.Displayed && element.Enabled);

            return element;
        }

        public void AssertElementIsDisplayed(By by, int timeoutInSeconds = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));

                IWebElement element = wait.Until(driver =>
                {
                    try
                    {
                        var webElement = driver.FindElement(by);
                        return webElement.Displayed ? webElement : null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return null;
                    }
                });

                Assert.IsTrue(element != null && element.Displayed, $"The element located by {by} is not displayed on the page.");
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail($"The element located by {by} was not visible within {timeoutInSeconds} seconds.");
            }
        }

        public void EnterTextInField(By by, string text, int timeoutInSeconds = 10)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                IWebElement inputField = wait.Until(driver => driver.FindElement(by));

                inputField.Clear();
                inputField.SendKeys(text);
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine($"Element not found: {by}");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Element with selector {by} was not visible within {timeoutInSeconds} seconds.");
            }
        }

        public string GenerateRandomEmail()
        {
            string randomPart = Guid.NewGuid().ToString().Substring(0, 8);
            string domain = "example.com";
            return $"{randomPart}@{domain}";
        }
    }
}
