using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace NBS.Core.Web.Mvc.DependencyInjection
{
	internal sealed class InternalDependencyScope : IDependencyScope
	{
		public InternalDependencyScope(IServiceScope scope)
		{
			Scope = scope ?? throw new ArgumentNullException(nameof(scope));
		}

		private IServiceScope Scope { get; }

		public void Dispose() => Scope.Dispose();
		public object? GetService(Type serviceType) => Scope.ServiceProvider.GetService(serviceType);
		public IEnumerable<object> GetServices(Type serviceType) => Scope.ServiceProvider.GetServices(serviceType);
	}
}