using System;
using System.Web.Mvc;
using JetBrains.Annotations;

namespace NBS.Core.Web.Mvc.DependencyInjection
{
	/// <summary>
	/// Represents a disposable scope for dependencies.
	/// </summary>
	[PublicAPI]
	public interface IDependencyScope : IDependencyResolver, IDisposable
	{
	}
}