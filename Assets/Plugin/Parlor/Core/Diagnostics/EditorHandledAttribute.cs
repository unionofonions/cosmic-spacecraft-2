#nullable enable

namespace Parlor.Diagnostics
{
	using System;
	using System.Diagnostics;

	[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false)]
	[Conditional("UNITY_EDITOR")]
	public class EditorHandledAttribute : Attribute { }
}