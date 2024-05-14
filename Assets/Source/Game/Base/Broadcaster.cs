
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
			};
			Domain.GetSpawnSystem().OnBeginSpawn += delegate
			{
				BroadcastKeyword("game_started", delay: 0.6f);
			};
			Domain.GetSpawnSystem().OnBeginLevel += level =>
			{
				if (!String.IsNullOrEmpty(level.LevelName))
				{
					BroadcastText(
						$"{TranslationSystem.KeywordToText("level_started")}\n{TranslationSystem.KeywordToText(level.LevelName)}",
						delay: 0f);
				}
			};
			ScoreSystem.OnHighestScoreChanged += score =>
			{
				BroadcastKeyword("new_record", delay: 0f);
			};
		}
		static private void BroadcastText(string text, float delay)
		{
			Domain.GetNotificationSystem().ShowText(text, delay: delay);
		}
		static private void BroadcastKeyword(string keyword, float delay)
		{
			var text = TranslationSystem.KeywordToText(keyword);
			BroadcastText(text, delay);
		}
	}
}