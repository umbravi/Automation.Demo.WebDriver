using System.Collections.Generic;
using System.Linq;
using Automation.Demo.WebDriver.Interaction.Interfaces;
using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Interaction.SeleniumWebDriver.SeleniumPrimitives
{
    public class SeleniumPrimitiveGets : SeleniumPrimitiveBase, IPrimitiveGets
    {
        private readonly IWebDriver selenium;

        public SeleniumPrimitiveGets(IWebDriver selenium)
        {
            this.selenium = selenium;
        }

        public string GetElementTextBy(By by)
        {
            var element = selenium.FindElement(by);
            var elementText = element.GetAttribute("text");

            return elementText;
        }

        public IList<string> GetOptionSelectedTextBy(By by)
        {
            var element = selenium.FindElement(by);
            var selectElement = GetSelectElement(element);

            return selectElement.Options.Where(o => o.Selected).Select(o => o.Text).ToList();
        }

        public IList<string> GetSelectOptionListTextBy(By by)
        {
            var element = selenium.FindElement(by);
            var selectElement = GetSelectElement(element);

            return selectElement.Options.Select(o => o.Text).ToList();
        }

        public virtual ISelectElement GetSelectElement(IWebElement element)
        {
            return GetSeleniumSelectElementFacade(element);
        }
    }
}