using Automation.Demo.WebDriver.Utilities;

namespace Automation.Demo.WebDriver.Types
{
    /// <summary>
    /// Used in <see cref="PedanticReporting"/> to set outcome of the step
    /// </summary>
    public class StepOutcome
    {
        public static StepOutcome Success = new StepOutcome(nameof(Success));
        public static StepOutcome Failure = new StepOutcome(nameof(Failure));

        internal readonly string Text;

        private StepOutcome(string text)
        {
            this.Text = text;
        }
    }
}
