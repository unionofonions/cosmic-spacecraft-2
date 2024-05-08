#nullable enable

namespace Parlor
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	[EditorHandled]
	public class ReadOnlyAttribute : PropertyAttribute
	{
		public ReadOnlyAttribute()
		{
			order = Int32.MinValue;
		}
	}
}