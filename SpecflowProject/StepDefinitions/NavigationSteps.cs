using OpenQA.Selenium;
using SpecflowProject.Drivers;
using SpecflowProject.StepDefinitions;
using SpecFlowProject.Drivers;
using TechTalk.SpecFlow;
using static SpecflowProject.Drivers.WebDriverExtensions;

namespace SpecFlowProject.StepDefinition
{
    [Binding]
    public class NavigationSteps : BaseStep
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverExtensions _webDriverExtensions;
        private PostsTextHref postsTextHref = new PostsTextHref();
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
            postsTextHref = _webDriverExtensions.CollectElementsUntil(By.XPath("//*[@class='posts-list']/article/div/h2/a"));
        }

        [When(@"I sroll up to article ""([^""]*)""")]
        public void WhenISrollUpToArticle(string text)
        {
            _webDriverExtensions.ScrollUpUntilElementFound(By.XPath($"//a[normalize-space()='{text}']"));
        }

        [When(@"I click on the article ""([^""]*)""")]
        public void WhenIClickOnTheArticle(string text)
        {
            var element = _driver.FindElement(By.XPath($"//a[normalize-space()='{text}']"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
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
            Console.WriteLine("Collection of Blog Posts:");

            int index = 1;
            var texts = postsTextHref.UniqueTexts.ToList();
            var hrefs = postsTextHref.Hrefs.ToList();

            for (int i = 0; i < Math.Min(texts.Count, hrefs.Count); i++)
            {
                Console.WriteLine($"{index}. Title: {texts[i]}, Link: {hrefs[i]}");
                index++;
            }
        }
    }
}
