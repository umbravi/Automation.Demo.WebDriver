using Automation.Demo.WebDriver.Interaction;
using Automation.Demo.WebDriver.Utilities;
using Automation.Demo.WebDriver.Utilities.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automation.Demo.WebDriver.Tests.Interaction
{
    [TestClass]
    public class InteractionBaseTests
    {
        private IPedanticReporting reporting;
        private InteractionsBase interactionsBase;

        [TestInitialize]
        public void TestSetup()
        {
            reporting = new PedanticReporting();
            interactionsBase = new InteractionsBase(reporting);
        }

        [TestCleanup]
        public void TestTeardown()
        {
            interactionsBase = null;
        }

        [TestMethod]
        public void Can_Evoke_Action_Delegate()
        {
            var numA = 1;
            var numB = 2;

            interactionsBase.Do(() => new TestFunctions().Add(numA, numB));
            
            reporting.Steps[0].Should().BeEquivalentTo("Step 1 - Success: \"Can_Evoke_Action_Delegate\" had parameters \"numA: 1, numB: 2\"");
        }

        [TestMethod]
        public void Can_Evoke_Func_Delegate()
        {
            var numA = 1;
            var numB = 2;

            var sum = interactionsBase.DoWithResult(() => new TestFunctions().AddWithReturn(numA, numB));
            
            reporting.Steps[0].Should().BeEquivalentTo("Step 1 - Success: \"Can_Evoke_Func_Delegate\" had parameters \"numA: 1, numB: 2\"");
            sum.ShouldBeEquivalentTo(3);
        }

    }
}
