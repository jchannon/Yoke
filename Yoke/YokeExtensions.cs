namespace Yoke
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    using Owin;

    using TinyIoC;
    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>; 

    public static class YokeExtensions
    {
        public delegate Task RequestDelegate(IDictionary<string, object> environment);

        public static IAppBuilder UseMiddleware<T>(this IAppBuilder builder, params object[] args)
        {
            return builder.UseMiddleware(typeof(T), args);
        }


        public static IAppBuilder UseMiddleware(this IAppBuilder builder, Type middleware, params object[] args)
        {
            return builder.Use(new Func<AppFunc,AppFunc>(next => (env =>
            {
                var container = (TinyIoCContainer)builder.Properties["container"];

                //var typeActivator = applicationServices.GetRequiredService<ITypeActivator>();
                //var instance = typeActivator.CreateInstance(builder.ApplicationServices, middleware, new[] { next }.Concat(args).ToArray());

                var instance = Activator.CreateInstance(middleware, new Func<IDictionary<string, object>, Task>[] { next });

                var methodinfo = middleware.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public);
                var parameters = methodinfo.GetParameters();
                //if (parameters[0].ParameterType != typeof(HttpContext))
                //{
                //    throw new Exception("TODO: Middleware Invoke method must take first argument of HttpContext");
                //}
                if (parameters.Length == 1)
                {
                    return (RequestDelegate)methodinfo.CreateDelegate(typeof(RequestDelegate), instance);
                }

                //return context =>
                {
                    //var serviceProvider = context.RequestServices ?? context.ApplicationServices ?? applicationServices;
                    //if (serviceProvider == null)
                    //{
                    //    throw new Exception("TODO: IServiceProvider is not available");
                    //}
                    
                    var arguments = new object[parameters.Length];
                    //arguments[0] = context;
                    for (var index = 0; index != parameters.Length; ++index)
                    {
                        var serviceType = parameters[index].ParameterType;
                        arguments[index] = container.Resolve(serviceType); //serviceProvider.GetService(serviceType);
                        if (arguments[index] == null)
                        {
                            throw new Exception(string.Format("TODO: No service for type '{0}' has been registered.", serviceType));
                        }
                    }

                    

                    return (Task)methodinfo.Invoke(instance, arguments);
                };

                
                
            })));

            
        }

    }
}
