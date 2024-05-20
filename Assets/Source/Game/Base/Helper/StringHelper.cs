#nullable enable

namespace Parlor.Game
{
	using System;
	using UnityEngine;

	static public class StringHelper
	{
		static public string? FormatSafe(string? format, params object?[]? args)
		{
			if (format == null) return null;
			try
			{
				return String.Format(format, args);
			}
			catch (FormatException)
			{
				return null;
			}
		}
		static public string? Colorize(string? str, Color color, bool close = true)
		{
			if (str == null) return null;
			var hex = ColorUtility.ToHtmlStringRGB(color);
			return close ? $"<#{hex}>{str}</color>" : $"<#{hex}>{str}";
		}
	}
}