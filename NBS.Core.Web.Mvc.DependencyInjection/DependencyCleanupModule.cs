using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using NBS.Core.Web.Mvc.DependencyInjection;

[assembly: PreApplicationStartMethod(typeof(DependencyCleanupModule), nameof(DependencyCleanupModule.Register))]

namespace NBS.Core.Web.Mvc.DependencyInjection
{
	/// <summary>
	/// Module which cleans up the DI scope when a request ends.
	/// </summary>
	public sealed class DependencyCleanupModule : IHttpModule
	{
		private static int _isInitialized;

		/// <summary>
		/// Registers this module.
		/// </summary>
		public static void Register()
		{
			int wasInitialized = Interlocked.Exchange(ref _isInitialized, 1);
			if (wasInitialized == 0) DynamicModuleUtility.RegisterModule(typeof(DependencyCleanupModule));
		}

		/// <inheritdoc />
		public void Init(HttpApplication context)
		{
			context.BeginRequest += OnBeginRequest;
			context.EndRequest += OnEndRequest;
		}

		private static void OnBeginRequest(object sender, EventArgs e)
		{
			if (DependencyResolver.Current is not MvcDependencyResolver mvc) return;

			var httpContext = ((HttpApplication)sender).Context;
			InternalDependencyResolverFactory.EnsureCurrentScope(mvc.Resolver, httpContext.Items);
		}

		private static void OnEndRequest(object sender, EventArgs e)
		{
			var httpContext = ((HttpApplication)sender).Context;
			if (InternalDependencyResolverFactory.GetCurrentScope(httpContext.Items) is IDisposable scope)
			{
				scope.Dispose();
			}
		}

		/// <inheritdoc />
		public void Dispose()
		{
		}
	}
}