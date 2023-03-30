using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using JetBrains.Annotations;

namespace NBS.Core.Web.Mvc.DependencyInjection
{
	/// <summary>
	/// MVC dependency resolver using Microsoft DI abstractions.
	/// </summary>
	[PublicAPI]
	public sealed class MvcDependencyResolver : IDependencyResolver, IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MvcDependencyResolver"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// The service provider.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </exception>
		public MvcDependencyResolver(IServiceProvider serviceProvider)
		{
			if (serviceProvider is null) throw new ArgumentNullException(nameof(serviceProvider));
			Resolver = new InternalDependencyResolver(serviceProvider);
		}

		internal InternalDependencyResolver Resolver { get; }

		/// <inheritdoc />
		public object? GetService(Type serviceType) => Resolver.GetOrCreateCurrentScope(HttpContext.Current).GetService(serviceType);

		/// <inheritdoc />
		public IEnumerable<object> GetServices(Type serviceType) => Resolver.GetOrCreateCurrentScope(HttpContext.Current).GetServices(serviceType);

		/// <inheritdoc />
		public void Dispose() => Resolver.Dispose();
	}
}