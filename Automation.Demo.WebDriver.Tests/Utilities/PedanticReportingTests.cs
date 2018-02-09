using System;
using System.Linq.Expressions;
using Automation.Demo.WebDriver.Types;
using Automation.Demo.WebDriver.Utilities;
using Automation.Demo.WebDriver.Utilities.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automation.Demo.WebDriver.Tests.Utilities
{
    [TestClass]
    public class PedanticReportingTests
    {
        private IPedanticReporting fakePedanticReporting;

        [TestInitialize]
        public void TestSetup()
        {
            fakePedanticReporting = A.Fake<PedanticReporting>();
        }

        [TestCleanup]
        public void TestTeardown()
        {
            Fake.ClearConfiguration(fakePedanticReporting);
        }

        [TestMethod]
        public void Can_Report_Success()
        {
            var stepTaken = "I'm a step that was taken";

            Expression<Action> sucessStep = () => fakePedanticReporting.LogStepReport(StepOutcome.Success, stepTaken);
            fakePedanticReporting.ReportSuccess(stepTaken);

            fakePedanticReporting.Steps.Count.ShouldBeEquivalentTo(1);
            var expectedText = "Step 1 - Success: I'm a step that was taken";
            fakePedanticReporting.Steps[0].Should().BeEquivalentTo(expectedText);
            fakePedanticReporting.Exceptions.Count.ShouldBeEquivalentTo(0);
        }

        [TestMethod]
        public void Can_Report_Failure()
        {
            var stepTaken = "I'm a step that was taken";
            var exception = new Exception(stepTaken);

            Expression<Action> sucessStep = () => fakePedanticReporting.LogStepReport(StepOutcome.Success, stepTaken);
            fakePedanticReporting.ReportFailure(stepTaken, exception);

            fakePedanticReporting.Steps.Count.ShouldBeEquivalentTo(1);
            var expectedText = $"Step 1 - Failure: {stepTaken} ; {exception.Message}";
            fakePedanticReporting.Steps[0].Should().BeEquivalentTo(expectedText);
            fakePedanticReporting.Exceptions.Count.ShouldBeEquivalentTo(1);
        }
    }
}
