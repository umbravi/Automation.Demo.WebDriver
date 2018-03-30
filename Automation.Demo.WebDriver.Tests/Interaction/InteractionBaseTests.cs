using System;
using System.Security.Cryptography.X509Certificates;
using Automation.Demo.WebDriver.Interaction;
using Automation.Demo.WebDriver.Utilities;
using Automation.Demo.WebDriver.Utilities.Interfaces;
using FakeItEasy;
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

        [TestMethod]
        public void Retries_Failed_Step()
        {
            var numA = 1;
            var numB = 2;
            var fakeAdd = A.Fake<TestFunctions>();

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).Returns(numA + numB);
            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).Throws<Exception>(exception => new Exception("I'm an exception")).Once();

            var result = interactionsBase.DoWithResult(() => fakeAdd.AddWithReturn(numA, numB));

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).MustHaveHappened(Repeated.Exactly.Twice);
            result.Should().Be(3);
            reporting.Exceptions.Count.Should().Be(1);
        }

        [TestMethod]
        public void Returns_Default_When_All_Steps_Fail()
        {
            var numA = 1;
            var numB = 2;
            var fakeAdd = A.Fake<TestFunctions>();

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).Returns(numA + numB);
            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).Throws<Exception>().Twice();

            var result = interactionsBase.DoWithResult(() => fakeAdd.AddWithReturn(numA, numB));

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).MustHaveHappened(Repeated.Exactly.Twice);
            result.Should().Be(default(int));
            reporting.Exceptions.Count.Should().Be(2);
        }

    }
}
