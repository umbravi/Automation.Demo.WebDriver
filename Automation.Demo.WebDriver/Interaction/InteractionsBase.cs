using System;
using System.Linq;
using System.Text.RegularExpressions;
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

        public void Do(Action singleAnonymousDelegate)
        {
            var methodInfo = GetDelegateInformation(singleAnonymousDelegate);
            var reportMessage = $"\"{methodInfo.name}\" had parameters \"{methodInfo.parametersList}\"";

            try
            {
                singleAnonymousDelegate();
                Reporting.ReportSuccess(reportMessage);
            }
            catch (Exception e)
            {
                Reporting.ReportFailure(reportMessage, e);
            }
        }

        public TResult DoWithResult<TResult>(Func<TResult> singleAnonymousDelegate)
        {
            var methodInfo = GetDelegateInformation(singleAnonymousDelegate);
            var reportMessage = $"\"{methodInfo.name}\" had parameters \"{methodInfo.parametersList}\"";

            try
            {
                TResult result = singleAnonymousDelegate();
                Reporting.ReportSuccess(reportMessage);
                return result;
            }
            catch (Exception e)
            {
                Reporting.ReportFailure(reportMessage, e);
                return (dynamic)string.Empty;
            }
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