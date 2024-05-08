#nullable enable

namespace Parlor.Game
{
	using System;

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
	}
}