using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Interaction.Interfaces
{
    public interface IPrimitiveValidations
    {
        void ValidateTextDisplayedOnPage(string text);
        void ValidateElementHasText(By by, string text);
        void ValidateOptionSelectedByText(By by, string text);
        void ValidateUrlContainsUri(string uri);
        ISelectElement GetSelectElement(IWebElement element);
    }
}