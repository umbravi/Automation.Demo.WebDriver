using System;
using System.IO;
using Automation.Demo.WebDriver.Interaction.SeleniumWebDriver;
using Automation.Demo.WebDriver.Types;
using Automation.Demo.WebDriver.Utilities;
using Automation.Demo.WebDriver.Utilities.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace Automation.Demo.WebDriver
{
    public class BrowserDriver
    {
        private IWebDriver webDriver;

        public SeleniumInteractions Interactions;

        public IPedanticReporting Reporting;
        
        public BrowserDriver(string webBrowserToOpen)
        {
            Reporting = new PedanticReporting();

            webDriver = GetDriver(webBrowserToOpen);

            webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            Interactions = new SeleniumInteractions(webDriver, Reporting);
        }

        private IWebDriver GetDriver(string webBrowserToOpen)
        {
            var pathToDriverDirectory = Directory.GetCurrentDirectory();

            switch (webBrowserToOpen.ToUpper())
            {
                case "CHROME":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments("start-maximized");
                    return new ChromeDriver(pathToDriverDirectory, chromeOptions);
                case "FIREFOX":
                    return new FirefoxDriver(pathToDriverDirectory);
                case "INTERNETEXPLORER":
                case "IE":
                    var ieOptions = new InternetExplorerOptions
                    {
                        IgnoreZoomLevel = true,
                        EnableNativeEvents = false
                    };
                    return new InternetExplorerDriver(pathToDriverDirectory, ieOptions);
                case "EDGE":
                    return new EdgeDriver(pathToDriverDirectory);
                default:
                    throw new Exception($"Unknown or unlisted web browser: {webBrowserToOpen}");
            }
        }

        public void KillDrivers()
        {
            webDriver.Close();
            webDriver.Quit();
            webDriver = null;

            if (Reporting.GetExceptions().Count > 0)
                throw new SeleniumValidationException("Errors found in test run. Please refer to logs for a list of errors");
        }
    }
}
