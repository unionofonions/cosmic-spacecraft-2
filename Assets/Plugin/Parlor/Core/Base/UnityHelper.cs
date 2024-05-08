#nullable enable

namespace Parlor
{
	static public class UnityHelper
	{
		static public bool IsNull(object? obj)
		{
			return obj is UnityEngine.Object uobj ? uobj == null : obj == null;
		}
	}
}