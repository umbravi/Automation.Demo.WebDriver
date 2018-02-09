using System;
using Automation.Demo.WebDriver.Utilities.Interfaces;

namespace Automation.Demo.WebDriver.Interaction.Interfaces
{
    public interface IWebDriverInteractionsBase
    {
        IPedanticReporting Reporting { get; set; }

        void Do(Action singleAnonymousDelegate);

        TResult DoWithResult<TResult>(Func<TResult> singleAnonymousDelegate);
    }
}