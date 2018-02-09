using Automation.Demo.WebDriver.Interaction.Interfaces;
using Automation.Demo.WebDriver.Interaction.SeleniumWebDriver.SeleniumFacades;
using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Interaction.SeleniumWebDriver.SeleniumPrimitives
{
    public abstract class SeleniumPrimitiveBase
    {
        public virtual ISelectElement GetSeleniumSelectElementFacade(IWebElement element)
        {
            return new SelectElementFacade(element);
        }
    }
}