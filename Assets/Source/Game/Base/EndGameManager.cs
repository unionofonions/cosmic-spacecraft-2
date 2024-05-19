
namespace Parlor.Game
{
	using System.Collections;
	using UnityEngine;
	using UnityEngine.Scripting;
	using TMPro;

	[DisallowMultipleComponent]
	public sealed class EndGameManager : MonoBehaviour
	{
		[SerializeField]
		private Curve m_TextAnimationFunction;
		[SerializeField, Unsigned]
		private float m_TextAnimationTime;
		[SerializeField, NotDefault]
		private TextMeshProUGUI m_ScoreLabel;

		private void Awake()
		{
			if (m_ScoreLabel != null)
			{
				Actor.OnDeath += OnActorDeath;
			}
			gameObject.SetActive(false);
		}
		private void OnActorDeath(Actor instigator, Actor victim)
		{
			if (victim is Player)
			{
				gameObject.SetActive(true);
				Domain.GetMusicSystem().PlayEndGameMusic();
			}
		}
		[Preserve]
		private void PauseGame()
		{
			Domain.GetGameSystem().PauseGame();
		}
		[Preserve]
		private void ShowScore()
		{
			if (m_ScoreLabel != null)
			{
				var score = ScoreSystem.CurrentScore;
				if (m_TextAnimationTime > 0f && score > 0)
				{
					StartCoroutine(PlayTextAnimation(score));
				}
				else
				{
					m_ScoreLabel.text = score.ToString();
				}
			}
		}
		private IEnumerator PlayTextAnimation(int score)
		{
			var timer = 0f;
			var tMul = 1f / m_TextAnimationTime;
			var start = 0;
			var end = score;
			while (timer < 1f)
			{
				timer += Time.unscaledDeltaTime * tMul;
				var y = m_TextAnimationFunction.Evaluate(timer);
				var value = Mathf.Lerp(start, end, y);
				m_ScoreLabel.text = ((int)value).ToString();
				yield return null;
			}
			m_ScoreLabel.text = score.ToString();
		}
	}
}