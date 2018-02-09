using System;
using System.Linq;
using Automation.Demo.WebDriver.Interaction.Interfaces;
using Automation.Demo.WebDriver.Types;
using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Interaction.SeleniumWebDriver.SeleniumPrimitives
{
    public class SeleniumPrimitiveValidations : SeleniumPrimitiveBase, IPrimitiveValidations
    {
        private readonly IWebDriver selenium;

        public SeleniumPrimitiveValidations(IWebDriver selenium)
        {
            this.selenium = selenium;
        }

        public void ValidateElementExists(By by)
        {
            var elements = selenium.FindElements(by);

            if (elements.Count == 0)
                throw new SeleniumValidationException($"Expected element is not displayed - Element Locator:{by}");
        }

        public void ValidateTextDisplayedOnPage(string text)
        {
            var by = By.XPath($"//*[contains(text(),'{text}')]");
            var elements = selenium.FindElements(by);
            
            if (elements.Count == 0)
                throw new SeleniumValidationException($"Expected text not displayed on page - Expected: \"{text}\"");
        }

        public void ValidateElementHasText(By by, string text)
        {
            var element = selenium.FindElement(by);

            var actualText = element.GetAttribute("text");
            if (text != actualText)
                throw new SeleniumValidationException($"Expected text not found - Expected: \"{text}\" - Actual: \"{actualText}\"");
        }

        public void ValidateOptionSelectedByText(By by, string text)
        {
            var element = selenium.FindElement(by);
            var selectElement = GetSelectElement(element);

            var actualOptionTextList = selectElement.AllSelectedOptions.Select(o => o.Text).ToList();
            if (!actualOptionTextList.Contains(text))
                throw new SeleniumValidationException($"Expected option not selected - Expected: \"{text}\" - Actual Options: \"{String.Join(", ", actualOptionTextList.ToArray())}\"");
        }

        public void ValidateUrlContainsUri(string uri)
        {
            var url = selenium.Url;

            if (!url.ToUpper().Contains(uri.ToUpper()))
                throw new SeleniumValidationException($"Expected uri not present - Expected Contains: \"{uri}\" - Actual: \"{url}\"");
        }
        public virtual ISelectElement GetSelectElement(IWebElement element)
        {
            return GetSeleniumSelectElementFacade(element);
        }
    }
}