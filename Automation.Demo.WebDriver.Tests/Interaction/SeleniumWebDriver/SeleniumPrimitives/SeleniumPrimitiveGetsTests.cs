using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Automation.Demo.WebDriver.Interaction.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Tests.Interaction.SeleniumWebDriver.SeleniumPrimitives
{
    [TestClass]
    public class SeleniumPrimitiveGetsTests : SeleniumPrimitiveTestSetupBase
    {
        [TestMethod]
        public void Selenium_GetElementText_ReturnsElementText()
        {
            var expectedElementText = "someText";

            Expression<Func<IWebElement>> findElement = () => fakeSeleniumDriver.FindElement(fakeBy);
            Expression<Func<String>> getElementText = () => fakeElement.GetAttribute("text");

            A.CallTo(findElement).Returns(fakeElement);
            A.CallTo(getElementText).Returns(expectedElementText);

            var actualElementText = fakeGets.GetElementTextBy(fakeBy);

            A.CallTo(findElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(getElementText).MustHaveHappened(Repeated.Exactly.Once);

            actualElementText.Should().BeEquivalentTo(expectedElementText);
        }

        [TestMethod]
        public void Selenium_GetOptionSelectedTextBy_ReturnsListOfSelectedOptionText()
        {
            var selectedOption = A.Fake<IWebElement>();
            A.CallTo(() => selectedOption.Text).Returns("expected option text");
            A.CallTo(() => selectedOption.Selected).Returns(true);

            var optionList = new List<IWebElement> {selectedOption};

            Expression<Func<IWebElement>> findElement = () => fakeSeleniumDriver.FindElement(fakeBy);
            Expression<Func<ISelectElement>> selectElement = () => fakeGets.GetSelectElement(fakeElement);
            Expression<Func<IList<IWebElement>>> getOptionList = () => fakeSelectElement.Options;

            A.CallTo(findElement).Returns(fakeElement);
            A.CallTo(selectElement).Returns(fakeSelectElement);
            A.CallTo(getOptionList).Returns(optionList);

            var returned = fakeGets.GetOptionSelectedTextBy(fakeBy);

            A.CallTo(findElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(selectElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(getOptionList).MustHaveHappened(Repeated.Exactly.Once);

            returned.Should().BeEquivalentTo(optionList.Where(o => o.Selected).Select(o => o.Text).ToList());
        }

        [TestMethod]
        public void Selenium_GetSelectOptionListTextBy_ReturnsListOfOptionText()
        {
            A.CallTo(() => fakeElement.TagName).Returns("select");
            
            var optionWithTextOne = A.Fake<IWebElement>();
            A.CallTo(() => optionWithTextOne.Text).Returns("expected text of option one element");
            
            var optionWithTextTwo = A.Fake<IWebElement>();
            A.CallTo(() => optionWithTextTwo.Text).Returns("expected test of option two element");

            var optionlist = new List<IWebElement> { optionWithTextOne, optionWithTextTwo };

            Expression<Func<IWebElement>> findElement = () => fakeSeleniumDriver.FindElement(fakeBy);
            Expression<Func<ISelectElement>> selectElement = () => fakeGets.GetSelectElement(fakeElement);
            Expression<Func<IList<IWebElement>>> getOptionList = () => fakeSelectElement.Options;

            A.CallTo(findElement).Returns(fakeElement);
            A.CallTo(selectElement).Returns(fakeSelectElement);
            A.CallTo(getOptionList).Returns(optionlist);

            var returned = fakeGets.GetSelectOptionListTextBy(fakeBy);

            A.CallTo(findElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(selectElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(getOptionList).MustHaveHappened(Repeated.Exactly.Once);

            returned.Should().BeEquivalentTo(optionlist.Select(o => o.Text).ToList());
        }
    }
}
