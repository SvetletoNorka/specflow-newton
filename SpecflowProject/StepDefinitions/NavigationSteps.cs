﻿using OpenQA.Selenium;
using SpecflowProject.Drivers;
using SpecflowProject.StepDefinitions;
using SpecFlowProject.Drivers;
using TechTalk.SpecFlow;

namespace SpecFlowProject.StepDefinition
{
    [Binding]
    public class NavigationSteps : BaseStep
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverExtensions _webDriverExtensions;
        private HashSet<string> postsList = new HashSet<string>();

        public NavigationSteps()
        {
            _driver = WebDriverController.Driver;
            _webDriverExtensions = new WebDriverExtensions(_driver, TimeSpan.FromSeconds(10));
        }

        [Given(@"I Navigate to BlankFactor page")]
        public void GivenINavigateToBlankFactorPage()
        {
            _driver.Navigate().GoToUrl(BaseUrl);
        }

        [Given(@"I accept the policy")]
        public void GivenIAcceptThePolicy()
        {
            var acceptButton = _driver.FindElement(By.Id("hs-eu-confirmation-button"));
            acceptButton.Click();
        }

        [When(@"I click on Insights menu")]
        public void WhenIClickOnInsightsMenu()
        {
            _webDriverExtensions.FindAndClick(By.XPath("//*[@id='menu-item-4436']"));
        }

        [When(@"I click on Blog menu")]
        public void WhenIClickOnBlogMenu()
        {
            _webDriverExtensions.FindAndClick(By.XPath("//*[contains(@class, 'btn btn-blue') and normalize-space()='Go to blog']"));
        }

        [When(@"I scrolls down to the end of the page with posts")]
        public void WhenIScrollsDownToTheEndOfPageOfPosts()
        {
            postsList = _webDriverExtensions.CollectElementsUntil(By.XPath("//*[@class='posts-list']/article/div/h2/a"));
        }

        [When(@"I sroll up to article ""([^""]*)""")]
        public void WhenISrollUpToArticle(string text)
        {
            _webDriverExtensions.ScrollUpUntilElementFound(By.XPath($"//a[normalize-space()='{text}']"));
        }

        [When(@"I click on the article ""([^""]*)""")]
        public void WhenIClickOnTheArticle(string text)
        {
            _webDriverExtensions.FindAndClick(By.XPath($"//a[normalize-space()='{text}']"));
        }

        [Then(@"I verify the URL = ""([^""]*)"" of the page")]
        public void ThenIVerifyTheURLOfThePage(string expectedUrl)
        {
            string actualURL = _driver.Url;
            Assert.AreEqual(expectedUrl, actualURL, $"Expected URL: {expectedUrl}, but got: {actualURL}");
        }

        [Then(@"I verify the post date = ""([^""]*)""")]
        public void ThenIVerifyThePostDate(string postDate)
        {
            _webDriverExtensions.AssertElementIsDisplayed(By.XPath("//div[@class='post-date' and normalize-space(text()) = 'August 13, 2021 - 10:04 am']"));
        }

        [Then(@"I subscribe to the newsletter")]
        public void ThenISubscribeToTheNewsletter()
        {
            _webDriverExtensions.ScrollAndFindElement(By.XPath("//button[normalize-space()='Subscribe']"));
            string randomEmail = _webDriverExtensions.GenerateRandomEmail();
            _webDriverExtensions.EnterTextInField(By.XPath("//input[@name='Email']"), randomEmail);
            _webDriverExtensions.FindAndClick(By.XPath("//button[normalize-space()='Subscribe']"));
        }

        [When(@"I go back")]
        public void WhenIGoBack()
        {
            _driver.Navigate().Back();
        }

        [Then(@"I print all post titles and their links")]
        public void ThenIPrintsAllPostTitlesAndTheirLinks()
        {
            Console.WriteLine("Съдържание на postsList:");

            int index = 1;
            foreach (var post in postsList)
            {
                Console.WriteLine($"{index}. {post}");
                index++;
            }
        }
    }
}