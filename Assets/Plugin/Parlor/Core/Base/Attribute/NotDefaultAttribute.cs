#nullable enable

namespace Parlor
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	[EditorHandled]
	public class NotDefaultAttribute : PropertyAttribute
	{
		public NotDefaultAttribute()
		{
			order = Int32.MinValue;
		}
	}
}