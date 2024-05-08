#nullable enable

namespace Parlor.Diagnostics
{
	using System;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.Runtime.InteropServices;

	static public class Contract
	{
		[Conditional("UNITY_EDITOR")]
		static public void Assert([DoesNotReturnIf(false)] bool condition, [Optional] string? message)
		{
			if (!condition) throw new AssertionException(message);
		}
		[Conditional("UNITY_EDITOR")]
		static public void Assume([DoesNotReturnIf(false)] bool condition, [Optional] string? message)
		{
			if (!condition) throw new AssumptionException(message);
		}

		#region Internal
		private sealed class AssertionException : Exception
		{
			private const string c_Message = "Assertion failed. Message: {0}.";

			public AssertionException(string? message)
			: base(String.Format(c_Message, message))
			{
				/*nop*/
			}
		}
		private sealed class AssumptionException : Exception
		{
			private const string c_Message = "Assumption failed. Message: {0}.";

			public AssumptionException(string? message)
			: base(String.Format(c_Message, message))
			{
				/*nop*/
			}
		}
		#endregion // Internal
	}
}