using System.Collections.Generic;
using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Interaction.Interfaces
{
    public interface IPrimitiveGets
    {
        string GetElementTextBy(By by);
        IList<string> GetOptionSelectedTextBy(By by);
        IList<string> GetSelectOptionListTextBy(By by);
        ISelectElement GetSelectElement(IWebElement element);
    }
}