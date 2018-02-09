using System;
using System.Collections.Generic;
using Automation.Demo.WebDriver.Types;

namespace Automation.Demo.WebDriver.Utilities.Interfaces
{
    public interface IPedanticReporting
    {
        IList<Exception> Exceptions { get; set; }
        IList<string> Steps { get; set; }

        IList<Exception> GetExceptions();
        void LogStepReport(StepOutcome stepOutcome, string stepReport);
        void ReportFailure(string stepTaken, Exception exception);
        void ReportSuccess(string stepTaken);
    }
}