using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Automation.Demo.WebDriver.Interaction.Interfaces;
using Automation.Demo.WebDriver.Types;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Tests.Interaction.SeleniumWebDriver.SeleniumPrimitives
{
    [TestClass]
    public class SeleniumPrimitiveValidationsTests : SeleniumPrimitiveTestSetupBase
    {
        [TestMethod]
        public void Selenium_ValidateElementHasText()
        {
            var expectedElementText = "Element Text";

            Expression<Func<IWebElement>> findElement = () => fakeSeleniumDriver.FindElement(fakeBy);
            Expression<Func<string>> getlElementText = () => fakeElement.GetAttribute("text");

            A.CallTo(findElement).Returns(fakeElement);
            A.CallTo(getlElementText).Returns(expectedElementText);

            fakeValidations.ValidateElementHasText(fakeBy, expectedElementText);

            A.CallTo(findElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(getlElementText).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(SeleniumValidationException), "Expected text not found: Expected: \"Element Text\" - Actual: \"Not Element Text\"")]
        public void Selenium_ValidateElementHasText_ThrowsSeleniumValidationException()
        {
            Expression<Func<IWebElement>> findElement = () => fakeSeleniumDriver.FindElement(fakeBy);
            Expression<Func<string>> getElementText = () => fakeElement.GetAttribute("text");

            A.CallTo(findElement).Returns(fakeElement);
            A.CallTo(getElementText).Returns("Element Text");

            fakeValidations.ValidateElementHasText(fakeBy, "Not Element Text");

            A.CallTo(findElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(getElementText).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Selenium_ValidateUrlContainsUri()
        {
            var expectedUri = "uri/text";

            Expression<Func<string>> getUrl = () => fakeSeleniumDriver.Url;

            A.CallTo(getUrl).Returns(expectedUri);

            fakeValidations.ValidateUrlContainsUri(expectedUri);

            A.CallTo(getUrl).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(SeleniumValidationException), "Expected uri not present: Expected Contains: \"exception/text\" - Actual: \"uri/text\"")]
        public void Selenium_ValidateUrlContainsUri_ThrowsSeleniumValidationException()
        {
            Expression<Func<string>> getUrl = () => fakeSeleniumDriver.Url;

            A.CallTo(getUrl).Returns("uri/text");

            fakeValidations.ValidateUrlContainsUri("exception/text");

            A.CallTo(getUrl).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Selenium_ValidateOptionSelectedByText()
        {
            A.CallTo(() => fakeElement.TagName).Returns("select");

            var expectedOptionText = "some option text";
            var selectedOption = A.Fake<IWebElement>();
            A.CallTo(() => selectedOption.Text).Returns(expectedOptionText);
            A.CallTo(() => selectedOption.Selected).Returns(true);

            var expectedOptionlist = new List<IWebElement> { selectedOption };

            Expression<Func<IWebElement>> findElement = () => fakeSeleniumDriver.FindElement(fakeBy);
            Expression<Func<ISelectElement>> getSelectElement = () => fakeValidations.GetSelectElement(fakeElement);
            Expression<Func<IList<IWebElement>>> getOptionList = () => fakeSelectElement.AllSelectedOptions;

            A.CallTo(findElement).Returns(fakeElement);
            A.CallTo(getSelectElement).Returns(fakeSelectElement);
            A.CallTo(getOptionList).Returns(expectedOptionlist);

            fakeValidations.ValidateOptionSelectedByText(fakeBy, expectedOptionText);

            A.CallTo(findElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(getSelectElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(getOptionList).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(SeleniumValidationException), "Expected uri not present: Expected Contains: \"not option text\" - Actual: \"option text\"")]
        public void Selenium_ValidateOptionSelectedByText_ThrowsSeleniumValidationException()
        {
            A.CallTo(() => fakeElement.TagName).Returns("select");

            var expectedOptionText = "option text";
            var notSelectedOption = A.Fake<IWebElement>();
            A.CallTo(() => notSelectedOption.Text).Returns(expectedOptionText);
            A.CallTo(() => notSelectedOption.Selected).Returns(false);

            var selectedOption = A.Fake<IWebElement>();
            A.CallTo(() => selectedOption.Text).Returns("Dobre Utro");
            A.CallTo(() => selectedOption.Selected).Returns(true);

            var expectedOptionList = new List<IWebElement> { notSelectedOption, selectedOption };

            Expression<Func<IWebElement>> findElement = () => fakeSeleniumDriver.FindElement(fakeBy);
            Expression<Func<ISelectElement>> getSelectElement = () => fakeGets.GetSelectElement(fakeElement);
            Expression<Func<IList<IWebElement>>> getAllSelectedOption = () => fakeSelectElement.AllSelectedOptions;
            
            A.CallTo(findElement).Returns(fakeElement);
            A.CallTo(getSelectElement).Returns(fakeSelectElement);
            A.CallTo(getAllSelectedOption).Returns(expectedOptionList);

            fakeValidations.ValidateOptionSelectedByText(fakeBy, expectedOptionText);

            A.CallTo(findElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(getSelectElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(getAllSelectedOption).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Selenium_ValidateTextDisplayedOnPage()
        {
            var elementText = "element text";

            var elementList = new List<IWebElement> { fakeElement };

            var myBy = By.XPath($"//*[contains(text(),'{elementText}')]");

            Expression<Func<IList<IWebElement>>> findElements = () => fakeSeleniumDriver.FindElements(myBy);

            A.CallTo(findElements).Returns(elementList);

            fakeValidations.ValidateTextDisplayedOnPage("text");

            A.CallTo(findElements).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(SeleniumValidationException), "Expected uri not present: Expected Contains: \"exception/text\" - Actual: \"uri/text\"")]
        public void Selenium_ValidateTextDisplayedOnPage_ThrowsSeleniumValidationException()
        {
            Expression<Func<string>> getUrl = () => fakeSeleniumDriver.Url;

            A.CallTo(getUrl).Returns("uri/text");

            fakeValidations.ValidateUrlContainsUri("exception/text");

            A.CallTo(getUrl).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
