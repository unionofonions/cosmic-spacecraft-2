#nullable enable

namespace Parlor
{
	using UnityEngine;

	static public class MathHelper
	{
		public const float Epsilon = 1e-6f;
		public const float Tau = 6.28318530f;

		static public float Dir2Deg(Vector2 dir)
		{
			return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		}
		static public Vector2 Deg2Dir(float deg)
		{
			var rad = deg * Mathf.Deg2Rad;
			return new(Mathf.Cos(rad), Mathf.Sin(rad));
		}
	}
}