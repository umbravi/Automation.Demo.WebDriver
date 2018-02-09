using System.Collections.Generic;
using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Interaction.Interfaces
{
    public interface ISelectElement
    {
        IList<IWebElement> AllSelectedOptions { get; }
        IWebElement WrappedElement { get; }
        IList<IWebElement> Options { get; }
        IWebElement SelectedOption { get; }

        void DeselectAll();
        void DeselectByIndex(int index);
        void DeselectByText(string text);
        void DeselectByValue(string value);
        void SelectByIndex(int index);
        void SelectByText(string text);
        void SelectByValue(string value);
    }
}