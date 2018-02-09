using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Interaction.Interfaces
{
    public interface IPrimitiveActions
    {
        void ClickElement(By by);
        void NavigateToUrl(string url);
        void SelectOptionByText(By by, string text);
        void SendTextToElement(By by, string text);
        ISelectElement GetSelectElement(IWebElement element);
    }
}