using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automation.Demo.WebDriver.Tests.Interaction.SeleniumWebDriver.SeleniumPrimitives
{
    [TestClass]
    public class SeleniumPrimitiveBaseClassTests: SeleniumPrimitiveTestSetupBase
    {
        [TestMethod]
        public void Selenium_GetSelectElement_ReturnsSelectElement()
        {
            A.CallTo(() => fakeElement.TagName).Returns("select");

            var expectedElement = fakeActions.GetSelectElement(fakeElement);

            expectedElement.ShouldBeEquivalentTo(fakeSelectElement);
        }
    }
}
