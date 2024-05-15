
namespace Parlor.Game
{
	using System;
	using UnityEngine;
	using Parlor.Game.Localization;

	static public class Broadcaster
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		static private void RegisterEvents()
		{
			Domain.GetSpawnSystem().OnBeginSpawn += delegate
			{
				Domain.GetNotificationSystem().HideText();
				BroadcastKeyword("spawn_started", animationName: "spawn_started");
			};
			Domain.GetSpawnSystem().OnBeginLevel += level =>
			{
				if (!String.IsNullOrEmpty(level.LevelName))
				{
					var levelStarted = $"<size=60%>{TranslationSystem.KeywordToText("level_started")}";
					var levelName = $"<size=115%>{TranslationSystem.KeywordToText(level.LevelName)}";
					BroadcastText($"{levelStarted}\n{levelName}", animationName: "level_started");
				}
			};
			ScoreSystem.OnHighestScoreChanged += score =>
			{
				BroadcastKeyword("new_record", animationName: null);
			};
		}
		static private void BroadcastText(string text, string animationName)
		{
			Domain.GetNotificationSystem().ShowText(text, animationName);
		}
		static private void BroadcastKeyword(string keyword, string animationName)
		{
			var text = TranslationSystem.KeywordToText(keyword);
			BroadcastText(text, animationName);
		}
	}
}