using System;
using System.Linq.Expressions;
using Automation.Demo.WebDriver.Interaction.Interfaces;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Tests.Interaction.SeleniumWebDriver.SeleniumPrimitives
{
    [TestClass]
    public class SeleniumPrimitiveActionsTests : SeleniumPrimitiveTestSetupBase
    {
        [TestMethod]
        public void Selenium_NavigateToUrl()
        {
            const string url = "anUrl";

            Expression<Func<INavigation>> navigation = () => fakeSeleniumDriver.Navigate();
            Expression<Action> navigateAction = () => fakeNavigation.GoToUrl(url);

            A.CallTo(navigation).Returns(fakeNavigation);
            A.CallTo(navigateAction).DoesNothing();

            fakeActions.NavigateToUrl(url);

            A.CallTo(navigation).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(navigateAction).MustHaveHappened(Repeated.Exactly.Once);

        }

        [TestMethod]
        public void Selenium_ClickElement()
        {
            Expression<Func<IWebElement>> findElement = () => fakeSeleniumDriver.FindElement(fakeBy);
            Expression<Action> clickAction = () => fakeElement.Click();

            A.CallTo(findElement).Returns(fakeElement);
            A.CallTo(clickAction).DoesNothing();

            fakeActions.ClickElement(fakeBy);

            A.CallTo(findElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(clickAction).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Selenium_SendTextToElement()
        {
            var textToSend = "abcd";

            Expression<Func<IWebElement>> findElement = () => fakeSeleniumDriver.FindElement(fakeBy);
            Expression<Action> clearAction = () => fakeElement.Clear();
            Expression<Action> sendKeysAction = () => fakeElement.SendKeys(textToSend);

            A.CallTo(findElement).Returns(fakeElement);
            A.CallTo(clearAction).DoesNothing();
            A.CallTo(sendKeysAction).DoesNothing();

            fakeActions.SendTextToElement(fakeBy, textToSend);

            A.CallTo(findElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(clearAction).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(sendKeysAction).MustHaveHappened(Repeated.Exactly.Once);
        }

        [TestMethod]
        public void Selenium_SelectOptionByText()
        {
            var optionText = "some option text";

            Expression<Func<IWebElement>> findElement = () => fakeSeleniumDriver.FindElement(fakeBy);
            Expression<Func<ISelectElement>> selectElement = () => fakeActions.GetSelectElement(fakeElement);
            Expression<Action> selectByText = () => fakeSelectElement.SelectByText(optionText);
            
            A.CallTo(findElement).Returns(fakeElement);
            A.CallTo(selectElement).Returns(fakeSelectElement);
            A.CallTo(selectByText).DoesNothing();

            fakeActions.SelectOptionByText(fakeBy, optionText);

            A.CallTo(findElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(selectElement).MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(selectByText).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
