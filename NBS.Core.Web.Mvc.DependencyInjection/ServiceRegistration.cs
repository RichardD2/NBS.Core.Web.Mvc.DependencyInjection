using System;
using System.Web.Mvc;
using JetBrains.Annotations;

namespace NBS.Core.Web.Mvc.DependencyInjection
{
	/// <summary>
	/// Utilities to register the Microsoft dependency injection.
	/// </summary>
	[PublicAPI]
	public static class ServiceRegistration
	{
		/// <summary>
		/// Registers the Microsoft dependency injection using the specified service provider.
		/// </summary>
		/// <param name="rootServiceProvider">
		/// The root <see cref="IServiceProvider"/> to use.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="rootServiceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void RegisterMvcDependencyInjection(this IServiceProvider rootServiceProvider)
		{
			if (rootServiceProvider is null) throw new ArgumentNullException(nameof(rootServiceProvider));

			MvcDependencyResolver resolver = new(rootServiceProvider);
			DependencyResolver.SetResolver(resolver);
		}
	}
}