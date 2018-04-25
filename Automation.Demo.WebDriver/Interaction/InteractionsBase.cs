using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using Automation.Demo.WebDriver.Interaction.Interfaces;
using Automation.Demo.WebDriver.Types;
using Automation.Demo.WebDriver.Utilities.Interfaces;
using OpenQA.Selenium.Internal;

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
            }, retryInterval, maxAttemptCount);
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

            var processedAction = ProcessAction(action, reportMessage, (TimeSpan)retryInterval, maxAttemptCount);

            if (processedAction.stepOutcome == StepOutcome.Success)
                Reporting.ReportSuccess(processedAction.actionReport);
            else Reporting.ReportFailure(processedAction.actionReport, processedAction.exception);

            return processedAction.result;
        }

        private (T result, StepOutcome stepOutcome, string actionReport, Exception exception) ProcessAction<T>(
            Func<T> action,
            string reportMessage,
            TimeSpan retryInterval,
            int maxAttempts = 2,
            int attempt = 1)
        {
            const int recursionMaxDepth = 5; //can configure in future if needed
            Exception actionException;
            (T result, StepOutcome stepOutcome, string actionReport, Exception exception) processedAction;
            try
            {
                if (attempt > maxAttempts || attempt > recursionMaxDepth) { return (default(T), StepOutcome.Failure, reportMessage + $" Attempts: {Math.Min(attempt, recursionMaxDepth)}", new Exception("Max attempts exceeded.")); }
                if (attempt > 1) { Thread.Sleep(retryInterval); }

                var result = action();
                return (result, StepOutcome.Success, reportMessage + $" Attempts: {attempt}", null);
            }
            catch (Exception e)
            {
                actionException = e;
                processedAction = ProcessAction(action, reportMessage, retryInterval, maxAttempts, attempt + 1);
            }
            return (processedAction.result, processedAction.stepOutcome, processedAction.actionReport, new Exception(actionException.Message, processedAction.exception));
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