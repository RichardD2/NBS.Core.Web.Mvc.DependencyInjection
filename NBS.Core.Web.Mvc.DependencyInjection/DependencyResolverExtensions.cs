using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JetBrains.Annotations;

namespace NBS.Core.Web.Mvc.DependencyInjection
{
	/// <summary>
	/// Extension methods for the <see cref="IDependencyResolver"/> interface.
	/// </summary>
	[PublicAPI]
	public static class DependencyResolverExtensions
	{
		/// <summary>
		/// Starts a resolution scope.
		/// </summary>
		/// <param name="resolver">
		/// The <see cref="IDependencyResolver"/>.
		/// </param>
		/// <returns>
		/// A new <see cref="IDependencyScope"/>.
		/// </returns>
		[MustUseReturnValue]
		public static IDependencyScope BeginScope(this IDependencyResolver resolver) => resolver switch
		{
			MvcDependencyResolver mdr => mdr.Resolver.BeginScope(),
			InternalDependencyResolver idr => idr.BeginScope(),
			_ => new DummyScope(resolver)
		};

		private sealed class DummyScope : IDependencyScope
		{
			public DummyScope(IDependencyResolver resolver)
			{
				Resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
			}

			private IDependencyResolver Resolver { get; }

			/// <inheritdoc />
			public object? GetService(Type serviceType) => Resolver.GetService(serviceType);

			/// <inheritdoc />
			public IEnumerable<object> GetServices(Type serviceType) => Resolver.GetServices(serviceType);

			/// <inheritdoc />
			public void Dispose()
			{
			}
		}
	}
}