#nullable enable

namespace Parlor.Diagnostics
{
	using System.Diagnostics;
	using UnityEngine;

	static public class Log
	{
		[HideInCallstack]
		[Conditional("UNITY_EDITOR")]
		static public void Info(string? message)
		{
			UnityEngine.Debug.Log(message);
		}
		[HideInCallstack]
		[Conditional("UNITY_EDITOR")]
		static public void Warning(string? message)
		{
			UnityEngine.Debug.LogWarning(message);
		}
		[HideInCallstack]
		[Conditional("UNITY_EDITOR")]
		static public void Error(string? message)
		{
			UnityEngine.Debug.LogError(message);
		}
	}
}