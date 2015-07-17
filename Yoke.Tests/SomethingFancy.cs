namespace Yoke.Tests
{
    using System.Diagnostics;

    public class SomethingFancy : ISomethingFancy
    {
        public void DoSomethingFancy()
        {
            Debug.WriteLine("Would you like a cucumber sandwich?");
        }
    }
}