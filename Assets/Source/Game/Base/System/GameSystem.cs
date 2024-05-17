
namespace Parlor.Game
{
	using System;
	using UnityEngine;

	[DisallowMultipleComponent]
	public sealed class GameSystem : MonoBehaviour
	{
		public event Action<GameStatus> OnGameStatusChanged;

		private GameStatus m_GameStatus;

		public void ResumeGame()
		{
			if (m_GameStatus != GameStatus.Playing)
			{
				m_GameStatus = GameStatus.Playing;
				Time.timeScale = 1.0f;
				OnGameStatusChanged?.Invoke(GameStatus.Playing);
			}
		}
		public void PauseGame()
		{
			if (m_GameStatus != GameStatus.Paused)
			{
				m_GameStatus = GameStatus.Paused;
				Time.timeScale = 0.0f;
				OnGameStatusChanged?.Invoke(GameStatus.Paused);
			}
		}
		public void QuitGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
	public enum GameStatus
	{
		Playing,
		Paused
	}
}