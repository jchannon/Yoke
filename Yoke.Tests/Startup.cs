namespace Yoke.Tests
{
    using Owin;

    using TinyIoC;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = new TinyIoCContainer();
            container.Register<ISomethingFancy,SomethingFancy>();

            app.Properties.Add("container", container);
            app.UseMiddleware<MyMiddleware>();
        }
    }
}
