using System;
using System.Collections.Generic;
using System.Linq;
using Automation.Demo.WebDriver.Types;
using Automation.Demo.WebDriver.Utilities.Interfaces;

namespace Automation.Demo.WebDriver.Utilities
{
    public class PedanticReporting : IPedanticReporting
    {
        public IList<Exception> Exceptions { get; set; } = new List<Exception>();

        public IList<string> Steps { get; set; } = new List<string>();

        public void ReportSuccess(string stepTaken)
        {
            LogStepReport(StepOutcome.Success, stepTaken);
        }

        public void ReportFailure(string stepTaken, Exception exception)
        {
            Exceptions.Add(exception);
            LogStepReport(StepOutcome.Failure, $"{stepTaken} ; {exception.Message}");
        }

        public IList<Exception> GetExceptions()
        {
            return new List<Exception>(Exceptions);
        }

        public void LogStepReport(StepOutcome stepOutcome, string stepReport)
        {
            Steps.Add($"Step {Steps.Count + 1} - {stepOutcome.Text}: {stepReport}");
            Console.WriteLine(Steps.Last());
        }
    }
}

