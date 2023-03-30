using System;
using System.Collections;
using JetBrains.Annotations;
using System.Web.Mvc;

namespace NBS.Core.Web.Mvc.DependencyInjection
{
	internal static class InternalDependencyResolverFactory
	{
		private static readonly object ContextKey = new();

		[MustUseReturnValue]
		internal static IDependencyScope? GetCurrentScope(IDictionary contextItems) => contextItems[ContextKey] as IDependencyScope;

		internal static IDependencyResolver EnsureCurrentScope(IDependencyResolver root, IDictionary contextItems)
		{
			if (contextItems[ContextKey] is not IDependencyScope result)
			{
				result = root.BeginScope();
				contextItems[ContextKey] = result;
			}

			return result;
		}

		[MustUseReturnValue]
		internal static IDependencyResolver GetOrCreateCurrentScope(IDependencyResolver root, IDictionary? contextItems) 
			=> contextItems is null ? root : EnsureCurrentScope(root, contextItems);
	}
}