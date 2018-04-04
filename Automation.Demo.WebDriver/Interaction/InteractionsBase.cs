using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Automation.Demo.WebDriver.Interaction.Interfaces;
using Automation.Demo.WebDriver.Types;
using Automation.Demo.WebDriver.Utilities.Interfaces;

namespace Automation.Demo.WebDriver.Interaction
{
    public class InteractionsBase : IWebDriverInteractionsBase
    {
        public IPedanticReporting Reporting { get; set; }

        public InteractionsBase(IPedanticReporting reporting)
        {
            this.Reporting = reporting;
        }

        public void Do(
            Action action,
            int maxAttemptCount = 2,
            TimeSpan? retryInterval = null)
        {
            // Encapsulate action delegate within a Func delegate with null return type
            DoWithResult<object>(() =>
                {
                    action();
                    return null;
                }, maxAttemptCount, retryInterval ?? TimeSpan.FromSeconds(1));
        }

        public T DoWithResult<T>(
            Func<T> action,
            int maxAttemptCount = 2,
            TimeSpan? retryInterval = null)
        {
            retryInterval = retryInterval ?? TimeSpan.FromSeconds(1);
            
            // If action is from Do() then get methodInfo from the encapsulated delegate
            var methodInfo = action.Method.ReturnType.BaseType != null ? GetDelegateInformation(action) : GetDelegateInformation(action.Target.GetType().GetFields()[0].GetValue(action.Target) as Delegate);
            var reportMessage = $"\"{methodInfo.name}\" had parameters \"{methodInfo.parametersList}\"";
            var attmeptLogs = new List<(string state, string attemptMessage, Exception exception)>();

            var result = default(T);

            var attemptOutcome = StepOutcome.Failure;

            for (var attempt = 1; (attemptOutcome == StepOutcome.Success || attempt > maxAttemptCount) == false ; attempt++)
            {
                try
                {
                    if (attempt > 1)
                    {
                        Thread.Sleep((TimeSpan)retryInterval);
                    }
                    result = action();
                    attmeptLogs.Add(("success", reportMessage + $" Attempts: {attempt}", null));
                    attemptOutcome = StepOutcome.Success;
                }
                catch (Exception e)
                {
                    attmeptLogs.Add(("failure", reportMessage + $" Attempts: {attempt}", e));
                }
            }

            if (attmeptLogs.Any(l => l.state == "success")) 
                Reporting.ReportSuccess(attmeptLogs.Last().attemptMessage);
            else Reporting.ReportFailure(attmeptLogs.Last().attemptMessage, attmeptLogs.Last().exception);

            return result;
        }

        private static (string name, string parametersList) GetDelegateInformation(Delegate singleAnonymousDelegate)
        {
            var method = singleAnonymousDelegate.Method.ToString();
            var methodMatch = Regex.Match(method, "<(.*)>");
            var methodName = methodMatch.Groups[1].Value;

            var target = singleAnonymousDelegate.Target.GetType().GetFields();
            var targetMatch = new Regex("<>.__.*");
            var targetList = string.Join(", ", target.Where(p => !targetMatch.IsMatch(p.Name)).Select(p =>
                                 $"{p.Name}: {p.GetValue(singleAnonymousDelegate.Target)}")) ?? string.Empty;

            return (methodName, targetList);
        }
    }
}