using System;
using System.Linq;
using System.Reflection;
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

            reporting.Steps.Count.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void Can_Evoke_Func_Delegate()
        {
            var numA = 1;
            var numB = 2;

            var sum = interactionsBase.DoWithResult(() => new TestFunctions().AddWithReturn(numA, numB));

            sum.Should().Be(3);
        }

        [TestMethod]
        public void DoWithResult_Retries_Failed_Step()
        {
            var numA = 1;
            var numB = 2;
            var fakeAdd = A.Fake<TestFunctions>();

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).Returns(numA + numB);
            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).Throws<Exception>().Once();

            var result = interactionsBase.DoWithResult(() => fakeAdd.AddWithReturn(numA, numB));

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).MustHaveHappened(Repeated.Exactly.Twice);
            result.Should().Be(3);
            reporting.Steps.Last().Should().Contain("Attempts: 2");
        }

        [TestMethod]
        public void DoWithResult_Returns_Default_When_All_Steps_Fail()
        {
            var numA = 1;
            var numB = 2;
            var fakeAdd = A.Fake<TestFunctions>();

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).Returns(numA + numB);
            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).Throws<Exception>().Twice();

            var result = interactionsBase.DoWithResult(() => fakeAdd.AddWithReturn(numA, numB));

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).MustHaveHappened(Repeated.Exactly.Twice);
            result.Should().Be(default(int));
            reporting.Exceptions.Count.Should().Be(1);
        }

        [TestMethod]
        public void Do_Invokes_DoWithResult()
        {
            var numA = 1;
            var numB = 2;
            var fakeAdd = A.Fake<TestFunctions>();
            
            A.CallTo(() => fakeAdd.Add(numA, numB)).Throws<Exception>().Twice();

            interactionsBase.Do(() => fakeAdd.Add(numA, numB));

            A.CallTo(() => fakeAdd.Add(numA, numB)).MustHaveHappened(Repeated.Exactly.Twice);
            reporting.Exceptions.Count.Should().Be(1);
        }

        [TestMethod]
        public void Method_Info_From_Do_To_DoWithResult_Is_Preserved_In_Success()
        {
            var numA = 1;
            var numB = 2;
            var fakeAdd = A.Fake<TestFunctions>();
            var delegateTargetName = MethodBase.GetCurrentMethod().Name;

            A.CallTo(() => fakeAdd.Add(numA, numB)).Throws<Exception>().Once();

            interactionsBase.Do(() => fakeAdd.Add(numA, numB));

            reporting.Steps.ToList().ForEach(s => s.Should().Contain(delegateTargetName));
        }

        [TestMethod]
        public void Method_Info_From_Do_To_DoWithResult_Is_Preserved_In_Failure()
        {
            var numA = 1;
            var numB = 2;
            var fakeAdd = A.Fake<TestFunctions>();
            var delegateTargetName = MethodBase.GetCurrentMethod().Name;

            A.CallTo(() => fakeAdd.Add(numA, numB)).Throws<Exception>().Twice();

            interactionsBase.Do(() => fakeAdd.Add(numA, numB));

            reporting.Steps.ToList().ForEach(s => s.Should().Contain(delegateTargetName));
        }

        [TestMethod]
        public void Method_Info_From_DoWithResult_Is_Correct_In_Success()
        {
            var numA = 1;
            var numB = 2;
            var fakeAdd = A.Fake<TestFunctions>();
            var delegateTargetName = MethodBase.GetCurrentMethod().Name;

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).Throws<Exception>().Once();

            interactionsBase.DoWithResult(() => fakeAdd.AddWithReturn(numA, numB));

            reporting.Steps.ToList().ForEach(s => s.Should().Contain(delegateTargetName));
        }

        [TestMethod]
        public void Method_Info_From_DoWithResult_Is_Correct_In_Failure()
        {
            var numA = 1;
            var numB = 2;
            var fakeAdd = A.Fake<TestFunctions>();
            var delegateTargetName = MethodBase.GetCurrentMethod().Name;

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).Throws<Exception>().Twice();

            interactionsBase.DoWithResult(() => fakeAdd.AddWithReturn(numA, numB));

            reporting.Steps.ToList().ForEach(s => s.Should().Contain(delegateTargetName));
        }

        [TestMethod]
        public void Successful_Invocation_Of_Delegate_Exits_Loop()
        {
            var numA = 1;
            var numB = 2;
            var fakeAdd = A.Fake<TestFunctions>();

            interactionsBase.DoWithResult(() => fakeAdd.AddWithReturn(numA, numB));

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).MustHaveHappened(Repeated.Exactly.Once);
            reporting.Steps.ToList().Last().Should().Contain("Attempts: 1");
        }

        [TestMethod]
        public void Success_After_Failure_Exits_Loop()
        {
            var numA = 1;
            var numB = 2;
            var fakeAdd = A.Fake<TestFunctions>();

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).Throws<Exception>().Once();

            interactionsBase.DoWithResult(() => fakeAdd.AddWithReturn(numA, numB), maxAttemptCount: 3);

            A.CallTo(() => fakeAdd.AddWithReturn(numA, numB)).MustHaveHappened(Repeated.Exactly.Twice);
            reporting.Steps.ToList().Last().Should().Contain("Attempts: 2");
        }
    }
}
