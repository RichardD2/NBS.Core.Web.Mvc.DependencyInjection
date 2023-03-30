using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace NBS.Core.Web.Mvc.DependencyInjection
{
	internal sealed class InternalDependencyResolver : IDependencyResolver, IDisposable
	{
		public InternalDependencyResolver(IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		private IServiceProvider ServiceProvider { get; }

		public void Dispose()
		{
			if (ServiceProvider is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		public object? GetService(Type serviceType) => ServiceProvider.GetService(serviceType);

		public IEnumerable<object> GetServices(Type serviceType) => ServiceProvider.GetServices(serviceType);

		[MustUseReturnValue]
		public InternalDependencyScope BeginScope()
		{
			var scope = ServiceProvider.CreateScope();
			return new InternalDependencyScope(scope);
		}

		[MustUseReturnValue]
		public IDependencyResolver GetOrCreateCurrentScope(HttpContext? context) 
			=> InternalDependencyResolverFactory.GetOrCreateCurrentScope(this, context?.Items);
	}
}