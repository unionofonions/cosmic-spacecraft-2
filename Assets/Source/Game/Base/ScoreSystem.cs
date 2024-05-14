
namespace Parlor.Game
{
	using System;
	using UnityEngine;

	[CreateAssetMenu(fileName = "ScoreSystem", menuName = "Parlor/Game/ScoreSystem")]
	public sealed class ScoreSystem : ScriptableObject
	{
		static public event Action<int> OnCurrentScoreChanged;
		static public event Action<int> OnHighestScoreChanged;

		static private ScoreSystem s_Instance;
		static private int s_CurrentScore;
		static private bool s_DontNotifyHighestScore;

		[SerializeField, Unsigned]
		private int m_HighestScore;

		static private ScoreSystem Instance
		{
			get
			{
				if (s_Instance == null)
				{
					s_Instance = Resources.Load<ScoreSystem>("Game/ScoreSystem");
				}
				return s_Instance;
			}
		}
		static public int CurrentScore
		{
			get
			{
				return s_CurrentScore;
			}
			private set
			{
				s_CurrentScore = value;
				OnCurrentScoreChanged?.Invoke(value);
			}
		}
		static public int HighestScore
		{
			get
			{
				if (Instance == null) return 0;
				return Instance.m_HighestScore;
			}
			private set
			{
				if (Instance != null)
				{
					Instance.m_HighestScore = value;
					if (!s_DontNotifyHighestScore)
					{
						OnHighestScoreChanged?.Invoke(value);
					}
				}
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		static private void RegisterEvents()
		{
			Domain.GetSpawnSystem().OnBeginSpawn += delegate
			{
				CurrentScore = 0;
				s_DontNotifyHighestScore = HighestScore == 0;
			};
			Actor.OnDeath += (instigator, victim) =>
			{
				if (victim.Score != 0)
				{
					CurrentScore += victim.Score;
					if (CurrentScore > HighestScore)
					{
						HighestScore = CurrentScore;
						s_DontNotifyHighestScore = true;
					}
				}
			};
		}
	}
}