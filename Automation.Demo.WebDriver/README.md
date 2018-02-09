# WebDriver
The WebDriver library is a fluent wrapper that is extendable to any WebDriver api build on the WebDriver Standard. This library makes writing tests easy as the WebDriver API is abstracted away and wrapped in fluent methods.

#### Features
* [Selenium WebDriver](http://www.seleniumhq.org/)
* [Selenium .Net/Core 2.0 github](https://github.com/SeleniumHQ/selenium/tree/master/dotnet)

#### Configuration
The entry point in to the WebDriver lirary is the BrowserDrvier class, and requires the name of the browser being tested against to instance a WebDriver compatible driver.
```c#
public BrowserDriver(string webBrowserToOpen)
```

<br />

#### Interactions Classes
An Interactions Class is the entrypoint to the WebDriver API being used. All interactions for a specific WebDriver API will be accessed form the Interactions Class for that WebDriver API. Interaction methods will access the primitive methods for the WebDriver API for Actions, Gets, and Validations.
Example Interactions Class
```c#
public class SeleniumInteractions : InteractionsBase, IBrowserDriverInteractions
{
    private readonly SeleniumPrimitiveGets gets;
    private readonly SeleniumPrimitiveActions actions;
    private readonly SeleniumPrimitiveValidations validations;

    public SeleniumInteractions(IWebDriver selenium)
    {
        this.gets = new SeleniumPrimitiveGets(selenium);
        this.actions = new SeleniumPrimitiveActions(selenium);
        this.validations = new SeleniumPrimitiveValidations(selenium);
    }

    public void NavigateToUrl(string url)
    {
        Do(() => { actions.NavigateToUrl(url); });
    }

    public void SendTextById(string id, string text)
    {
        Do(() => { actions.SendTextToElement(By.Id(id), text); });
    }

    public void SendTextByXpath(string xPath, string text)
    {
        Do(() => { actions.SendTextToElement(By.XPath(xPath), text); });
    }

    public void SendTextByCssSelector(string cssSelector, string text)
    {
        Do(() => { actions.SendTextToElement(By.CssSelector(cssSelector), text); });
    }
}
```

<br />

#### Primitive Classes
The library used Primitive classes to wrap WebDriver API implementaion. Each primitive method's parameters will include the WebDriver API's locator object and often time a string or other data.

#### Actions
An Action is any method that issues an instruction to the Web Browser(navigate, click, select, submit, ect.).

### Gets
A Get is any method that retrieves information from the Web Browser. This information is always returned as common types(string, list\<string>, list<list\<string>>).

#### Validation
A Validation is a method that validates an expected condition against the browser. Validaitons will throw verbose custom exceptions.

Example Validation:
```c#
public void ValidateElementHasText(By by, string text)
{
    var element = selenium.FindElement(by);

    var actualText = element.GetAttribute("text");
    if (text != actualText)
        throw new SeleniumValidationException($"Expected text not found: Expected: \"{text}\" - Actual: \"{actualText}\"");
}
```
<br />

#### PedanticReporting
