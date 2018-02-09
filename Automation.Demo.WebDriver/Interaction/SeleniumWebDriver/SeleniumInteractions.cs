using System.Collections.Generic;
using Automation.Demo.WebDriver.Interaction.Interfaces;
using Automation.Demo.WebDriver.Interaction.SeleniumWebDriver.SeleniumPrimitives;
using Automation.Demo.WebDriver.Utilities.Interfaces;
using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Interaction.SeleniumWebDriver
{
    public class SeleniumInteractions : InteractionsBase, IBrowserDriverInteractions
    {
        private readonly SeleniumPrimitiveGets gets;
        private readonly SeleniumPrimitiveActions actions;
        private readonly SeleniumPrimitiveValidations validations;

        public SeleniumInteractions(IWebDriver selenium, IPedanticReporting reporting) : base(reporting)
        {
            this.gets = new SeleniumPrimitiveGets(selenium);
            this.actions = new SeleniumPrimitiveActions(selenium);
            this.validations = new SeleniumPrimitiveValidations(selenium);
        }

        public void NavigateToUrl(string url)
        {
            Do(() => actions.NavigateToUrl(url));
        }

        public void SendTextById(string id, string text)
        {
            Do(() => actions.SendTextToElement(By.Id(id), text));
        }

        public void SendTextByXpath(string xPath, string text)
        {
            Do(() => actions.SendTextToElement(By.XPath(xPath), text));
        }

        public void SendTextByCssSelector(string cssSelector, string text)
        {
            Do(() => actions.SendTextToElement(By.CssSelector(cssSelector), text));
        }

        public void ClickById(string id)
        {
            Do(() => actions.ClickElement(By.Id(id)));
        }

        public void ClickByCssSelector(string cssSelector)
        {
            Do(() => actions.ClickElement(By.CssSelector(cssSelector)));
        }

        public void ClickByXpath(string xpath)
        {
            Do(() => actions.ClickElement(By.XPath(xpath)));
        }

        public void ClickButtonByText(string buttonText)
        {
            Do(() => actions.ClickElement(By.XPath($"//button[text() = '{buttonText}']")));
        }

        public void ClickLinkByText(string linkText)
        {
            Do(() => actions.ClickElement(By.LinkText(linkText)));
        }

        public void SelectOptionTextById(string id, string text)
        {
            Do(() => actions.SelectOptionByText(By.Id(id), text));
        }

        public void ValidateElementExistsById(string id)
        {
            Do(() => validations.ValidateElementExists(By.Id(id)));
        }

        public void ValidateElementExistsByCssSelector(string cssSelector)
        {
            Do(() => validations.ValidateElementExists(By.CssSelector(cssSelector)));
        }

        public void ValidateElementExistsByXPath(string xPath)
        {
            Do(() => validations.ValidateElementExists(By.XPath(xPath)));
        }

        public void ValidateTextIsDisplayed(string text)
        {
            Do(() => validations.ValidateTextDisplayedOnPage(text));
        }

        public void ValidateUri(string uri)
        {
            Do(() => validations.ValidateUrlContainsUri(uri));
        }

        public IList<string> GetOptionsSelectedBySelectId(string questionOneSelectId)
        {
            return DoWithResult(() => gets.GetOptionSelectedTextBy(By.Id(questionOneSelectId)));
        }

        public IList<string> GetOptionListBySelectId(string questionOneSelectId)
        {
            return DoWithResult(() => gets.GetSelectOptionListTextBy(By.Id(questionOneSelectId)));
        }

        public string GetElementTextByXpath(string xPath)
        {
            return DoWithResult(() => gets.GetElementTextBy(By.XPath(xPath)));
        }

        public string GetElementTextByLabelText(string text)
        {
            // xpath for element sibling of label
            //   $"//label[text() = '{text}']/following-sibling::*[1]"
            // xpath for elemet child of label
            //   $"//label[text() = '{text}']/*[1]"
            return GetElementTextByXpath($"//label[text() = '{text}']/following-sibling::*[1]");
        }
    }
}
