#nullable enable

namespace Parlor
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	[Serializable, EditorHandled]
	public class Curve : IFunction
	{
		public float Evaluate(float x)
		{
			return m_Function.Evaluate(x);
		}

		#region Internal
#nullable disable
		[SerializeField]
		private AnimationCurve m_Function;
#nullable enable
		#endregion // Internal
	}
}