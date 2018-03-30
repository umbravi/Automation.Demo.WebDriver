namespace Automation.Demo.WebDriver.Tests.Interaction
{
    public class TestFunctions
    {
        public TestFunctions() { }

        public virtual void Add(int a, int b)
        {
            var num = a + b;
        }

        public virtual int AddWithReturn(int a, int b)
        {
            return a + b;
        }
    }
}
