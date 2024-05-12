#nullable enable

namespace Parlor
{
	using System.Collections.Generic;
	using UnityEngine;

	static public class Random
	{
		static public float Single(float min, float max)
		{
			return UnityEngine.Random.Range(min, max);
		}
		static public int Int32(int low, int high)
		{
			return UnityEngine.Random.Range(low, high);
		}
		static public bool Boolean(float trueChance)
		{
			return Single(MathHelper.Epsilon, 1.0f) <= trueChance;
		}
		static public int Sign(bool includeZero = false)
		{
			return includeZero
				? Int32(-1, 2)
				: Int32(0, 2) == 0 ? -1 : 1;
		}
		static public Vector2 Circle(float outerRadius, float innerRadius = 0f, float minAngle = 0f, float maxAngle = 360f)
		{
			var angleOffset = Single(minAngle, maxAngle);
			var radius = Single(innerRadius, outerRadius);
			return MathHelper.Deg2Dir(angleOffset) * radius;
		}
		static public T? Element<T>(IEnumerable<T?>? enumerable)
		{
			if (enumerable is null)
			{
				return default;
			}
			else if (enumerable is IReadOnlyList<T?> list)
			{
				return list.Count switch
				{
					0 => default,
					1 => list[0],
					_ => list[Int32(0, list.Count)],
				};
			}
			else if (enumerable is IReadOnlyCollection<T?> collection)
			{
				var count = collection.Count;
				if (count == 0) return default;
				var index = Int32(0, count);
				foreach (var elem in collection)
					if (index-- == 0) return elem;
				return default;
			}
			else
			{
				var ret = default(T);
				var count = 0;
				foreach (var elem in enumerable)
					if (Int32(0, count++) == 0) ret = elem;
				return ret;
			}
		}
	}
}