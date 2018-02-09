using System;

namespace Automation.Demo.WebDriver.Types
{
    [Serializable]
    public class SeleniumValidationException : Exception
    {
        public SeleniumValidationException() { }
        public SeleniumValidationException(string message) : base(message) { }
        public SeleniumValidationException(string message, Exception inner) : base(message, inner) { }
    }
}
