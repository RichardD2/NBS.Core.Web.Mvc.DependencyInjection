# NBS.Core.Web.Mvc.DependencyInjection

Dependency injection for ASP.NET MVC 5 applications using the Microsoft DI abstractions library.

## Usage

1. Add a reference to the `Microsoft.Extensions.DependencyInjection` NuGet package.
2. Add a reference to this library.
3. Build your service provider.
4. Use this library to register your service provider as the `IDependencyResolver` for the application.

### Example

```csharp
using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Extensions.DependencyInjection;
using NBS.Core.Web.Mvc.DependencyInjection;
using Scrutor;

static class DependencyConfig
{
    public static void Register()
    {
        IServiceCollection services = new ServiceCollection();
        ConfigureServices(services);
        IServiceProvider provider = services.BuildServiceProvider(true);
        provider.RegisterMvcDependencyInjection();
    }
    
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(RouteTable.Routes);
        services.AddScoped(_ => HttpContext.Current);
        services.AddScoped<HttpContextBase>(sp => new HttpContextWrapper(sp.GetRequiredService<HttpContext>()));
        services.AddScoped(sp => sp.GetRequiredService<HttpContextBase>().Request);
        services.AddScoped(sp => sp.GetRequiredService<RouteCollection>().GetRouteData(sp.GetRequiredService<HttpContextBase>()));
        services.AddScoped(sp => new RequestContext(sp.GetRequiredService<HttpContextBase>(), sp.GetRequiredService<RouteData>()));
        services.AddScoped(sp => new UrlHelper(sp.GetRequiredService<RequestContext>(), sp.GetRequiredService<RouteCollection>()));
        
        // Use Scrutor to register all controllers as scoped services:
        services.Scan(scan => scan.FromAssemblyOf<MvcApplication>()
            .AddClasses(classes => classes.AssignableTo<Controller>())
            .AsSelf().WithScopedLifetime());
    }
}
```
