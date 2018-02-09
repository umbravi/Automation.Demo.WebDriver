using System.Collections.Generic;

namespace Automation.Demo.WebDriver.Interaction.Interfaces
{
    public interface IBrowserDriverInteractions
    {
        void ClickButtonByText(string buttonText);
        void ClickByCssSelector(string cssSelector);
        void ClickById(string id);
        void ClickByXpath(string xpath);
        void ClickLinkByText(string linkText);
        string GetElementTextByLabelText(string text);
        string GetElementTextByXpath(string xPath);
        IList<string> GetOptionListBySelectId(string questionOneSelectId);
        IList<string> GetOptionsSelectedBySelectId(string questionOneSelectId);
        void NavigateToUrl(string url);
        void SelectOptionTextById(string id, string text);
        void SendTextByCssSelector(string cssSelector, string text);
        void SendTextById(string id, string text);
        void SendTextByXpath(string xPath, string text);
        void ValidateElementExistsByCssSelector(string cssSelector);
        void ValidateElementExistsById(string id);
        void ValidateElementExistsByXPath(string xPath);
        void ValidateTextIsDisplayed(string text);
        void ValidateUri(string uri);
    }
}