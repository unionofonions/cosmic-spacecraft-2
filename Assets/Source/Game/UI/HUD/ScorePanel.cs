
namespace Parlor.Game
{
	using UnityEngine;
	using TMPro;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator))]
	public sealed class ScorePanel : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private TextMeshProUGUI m_Label;
		private int m_CurrentScoreChangedHash;
		private Animator m_Animator;

		private void Awake()
		{
			m_Animator = GetComponent<Animator>();
			m_CurrentScoreChangedHash = Animator.StringToHash("current_score_changed");
			ScoreSystem.OnCurrentScoreChanged += OnCurrentScoreChanged;
		}
		private void OnCurrentScoreChanged(int score)
		{
			if (m_Label != null)
			{
				m_Label.text = score.ToString();
				if (score != 0)
				{
					m_Animator.Play(m_CurrentScoreChangedHash);
				}
			}
		}
	}
}