namespace Yoke.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MyMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> nextFunc;

        public MyMiddleware(Func<IDictionary<string, object>, Task> nextFunc)
        {
            this.nextFunc = nextFunc;
        }

        public Task Invoke(IDictionary<string, object> environment, ISomethingFancy somethingFancy)
        {
            somethingFancy.DoSomethingFancy();
            return nextFunc(environment);
        }
    }
}
