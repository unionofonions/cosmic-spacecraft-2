#nullable enable

namespace Parlor
{
	using System;
	using System.Collections.Generic;

	static public class SystemHelper
	{
		static public IEnumerable<Type> Hierarchy(this Type? type)
		{
			for (var elem = type; elem != null; elem = elem.BaseType) yield return elem;
		}
		static public string PartialName(this Type? type)
		{
			if (type == null) return String.Empty;
			return type.IsNested ? $"{type.DeclaringType.PartialName()}+{type.Name}" : type.Name;
		}
	}
}