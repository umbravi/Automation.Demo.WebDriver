using System.Collections.Generic;
using Automation.Demo.WebDriver.Interaction.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Automation.Demo.WebDriver.Interaction.SeleniumWebDriver.SeleniumFacades
{
    public class SelectElementFacade : ISelectElement
    {
        private readonly SelectElement facadeSelectElement;

        public SelectElementFacade(IWebElement element)
        {
            facadeSelectElement = new SelectElement(element);
        }

        public IWebElement WrappedElement => this.facadeSelectElement.WrappedElement;

        public IList<IWebElement> Options => this.facadeSelectElement.Options;


        public IWebElement SelectedOption => this.facadeSelectElement.SelectedOption;


        public IList<IWebElement> AllSelectedOptions => this.facadeSelectElement.AllSelectedOptions;


        public void SelectByText(string text)
        {
            this.facadeSelectElement.SelectByText(text);
        }


        public void SelectByValue(string value)
        {
            this.facadeSelectElement.SelectByValue(value);
        }

        public void SelectByIndex(int index)
        {
            this.facadeSelectElement.SelectByIndex(index);
        }

        public void DeselectAll()
        {
            this.facadeSelectElement.DeselectAll();
        }

        public void DeselectByText(string text)
        {
            this.facadeSelectElement.DeselectByText(text);
        }

        public void DeselectByValue(string value)
        {
            this.facadeSelectElement.DeselectByValue(value);
        }

        public void DeselectByIndex(int index)
        {
            this.facadeSelectElement.DeselectByIndex(index);
        }
    }
}
