#nullable enable

namespace Parlor
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	[EditorHandled]
	public class LimitedAttribute : PropertyAttribute
	{
		public LimitedAttribute(float min, float max)
		{
			Min = min;
			Max = max;
		}
		public LimitedAttribute(float min)
		: this(min, Single.PositiveInfinity)
		{
			/*nop*/
		}

		public readonly float Min;
		public readonly float Max;
	}
}