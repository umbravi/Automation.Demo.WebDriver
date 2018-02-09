using Automation.Demo.WebDriver.Interaction.Interfaces;
using Automation.Demo.WebDriver.Interaction.SeleniumWebDriver.SeleniumPrimitives;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace Automation.Demo.WebDriver.Tests.Interaction.SeleniumWebDriver.SeleniumPrimitives
{
    public class SeleniumPrimitiveTestSetupBase
    {
        protected IWebDriver fakeSeleniumDriver;
        protected SeleniumPrimitiveActions fakeActions;
        protected SeleniumPrimitiveGets fakeGets;
        protected SeleniumPrimitiveValidations fakeValidations;
        protected By fakeBy;
        protected IWebElement fakeElement;
        protected ISelectElement fakeSelectElement;
        protected INavigation fakeNavigation;

        [TestInitialize]
        public void TestSetup()
        {
            fakeSeleniumDriver = A.Fake<IWebDriver>();
            fakeActions =
                A.Fake<SeleniumPrimitiveActions>(x => x.WithArgumentsForConstructor(new object[] { fakeSeleniumDriver }));
            fakeGets =
                A.Fake<SeleniumPrimitiveGets>(x => x.WithArgumentsForConstructor(new object[] { fakeSeleniumDriver }));
            fakeValidations =
                A.Fake<SeleniumPrimitiveValidations>(x => x.WithArgumentsForConstructor(new object[] { fakeSeleniumDriver }));
            fakeBy = A.Fake<By>();
            fakeElement = A.Fake<IWebElement>();
            fakeNavigation = A.Fake<INavigation>();
            fakeSelectElement = A.Fake<ISelectElement>();
        }

        [TestCleanup]
        public void TestTeardown()
        {
            Fake.ClearConfiguration(fakeActions);
            Fake.ClearConfiguration(fakeGets);
            Fake.ClearConfiguration(fakeValidations);
            Fake.ClearConfiguration(fakeSeleniumDriver);
            Fake.ClearConfiguration(fakeBy);
            Fake.ClearConfiguration(fakeElement);
            Fake.ClearConfiguration(fakeNavigation);
            Fake.ClearConfiguration(fakeSelectElement);
        }
    }
}
