using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using SpecflowProject.StepDefinitions;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SpecFlowProject.Drivers
{
    public static class WebDriverController
    {
        public static IWebDriver Driver { get; set; }

        public static void StartDriver()
        {
            var driverManager = new DriverManager();
            string browserType = BaseStep.BrowserType;

            switch (browserType)
            {
                case "Chrome":
                    driverManager.SetUpDriver(new ChromeConfig());
                    Driver = new ChromeDriver();
                    break;
                case "Edge":
                    driverManager.SetUpDriver(new EdgeConfig());
                    Driver = new EdgeDriver();
                    break;
                case "IE":
                    driverManager.SetUpDriver(new InternetExplorerConfig());
                    Driver = new InternetExplorerDriver();
                    break;
                default:
                    throw new NotSupportedException($"Browser type '{browserType}' is not supported.");
            }

            Driver.Manage().Window.Maximize();
        }

        public static void Quit()
        {
            Driver?.Quit();
            Driver = null;
        }
    }
}
