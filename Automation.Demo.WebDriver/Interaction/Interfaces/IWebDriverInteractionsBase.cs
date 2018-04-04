using System;
using Automation.Demo.WebDriver.Utilities.Interfaces;

namespace Automation.Demo.WebDriver.Interaction.Interfaces
{
    public interface IWebDriverInteractionsBase
    {
        IPedanticReporting Reporting { get; set; }

        void Do(Action action,
            int maxAttemptCount,
            TimeSpan? retryInterval);

        T DoWithResult<T>(Func<T> action,
            int maxAttemptCount,
            TimeSpan? retryInterval);
    }
}