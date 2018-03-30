using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Automation.Demo.WebDriver.Interaction.Interfaces;
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
            TimeSpan? retryInterval = null, 
            int maxAttemptCount = 2)
        {
            // Encapsulate action delegate within a Func delegate with null return type
            DoWithResult<object>(() =>
                {
                    action();
                    return null;
                }, retryInterval ?? TimeSpan.FromSeconds(1), maxAttemptCount
            );
        }

        public T DoWithResult<T>(
            Func<T> action,
            TimeSpan? retryInterval = null,
            int maxAttemptCount = 2)
        {
            retryInterval = retryInterval ?? TimeSpan.FromSeconds(1);
            
            // If action is from Do() then get methodInfo from the encapsulated delegate
            var methodInfo = action.Method.ReturnType.BaseType != null ? GetDelegateInformation(action) : GetDelegateInformation(action.Target.GetType().GetFields()[0].GetValue(action.Target) as Delegate);
            var reportMessage = $"\"{methodInfo.name}\" had parameters \"{methodInfo.parametersList}\"";

            for (var attempt = 1; attempt < maxAttemptCount; attempt++)
            {
                try
                {
                    if (attempt > 1)
                    {
                        Thread.Sleep((TimeSpan)retryInterval);
                    }
                    var result = action();
                    Reporting.ReportSuccess(reportMessage);
                    return result;
                }
                catch (Exception e)
                {
                    Reporting.ReportFailure(reportMessage + $" Attempt: {attempt}", e);
                }
            }
            return default(T);
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