using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SpecFlowProject.Drivers;
using TechTalk.SpecFlow;

namespace SpecFlowProject.Hooks
{
    [Binding]
    public class WebDriverHooks
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
            WebDriverController.StartDriver();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            WebDriverController.Quit();
        }
    }
}
