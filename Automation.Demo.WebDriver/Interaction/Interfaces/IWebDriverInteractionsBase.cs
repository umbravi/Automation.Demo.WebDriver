using System;
using Automation.Demo.WebDriver.Utilities.Interfaces;

namespace Automation.Demo.WebDriver.Interaction.Interfaces
{
    public interface IWebDriverInteractionsBase
    {
        IPedanticReporting Reporting { get; set; }

        void Do(Action action,
            TimeSpan? retryInterval,
            int maxAttemptCount);

        T DoWithResult<T>(Func<T> action,
            TimeSpan? retryInterval,
            int maxAttemptCount);
    }
}