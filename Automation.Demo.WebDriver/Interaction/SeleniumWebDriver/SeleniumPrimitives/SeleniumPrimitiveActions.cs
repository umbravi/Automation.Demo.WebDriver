using Automation.Demo.WebDriver.Interaction.Interfaces;
using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Interaction.SeleniumWebDriver.SeleniumPrimitives
{
    public class SeleniumPrimitiveActions : SeleniumPrimitiveBase, IPrimitiveActions
    {
        private readonly IWebDriver selenium;

        public SeleniumPrimitiveActions(IWebDriver selenium)
        {
            this.selenium = selenium;
        }

        public void NavigateToUrl(string url)
        {
            selenium.Navigate().GoToUrl(url);
        }

        public void ClickElement(By by)
        {
            var element = selenium.FindElement(by);

            element.Click();
        }

        public void SendTextToElement(By by, string text)
        {
            var element = selenium.FindElement(by);

            element.Clear();
            element.SendKeys(text);
        }

        public void SelectOptionByText(By by, string text)
        {
            var element = selenium.FindElement(by);
            var selectElement = GetSelectElement(element);

            selectElement.SelectByText(text);
        }

        public virtual ISelectElement GetSelectElement(IWebElement element)
        {
            return GetSeleniumSelectElementFacade(element);
        }
    }
}