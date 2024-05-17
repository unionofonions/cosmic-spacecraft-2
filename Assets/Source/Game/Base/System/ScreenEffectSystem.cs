
namespace Parlor.Game
{
	using System.Collections;
	using UnityEngine;
	using UnityEngine.UI;

	[DisallowMultipleComponent]
	public sealed class ScreenEffectSystem : MonoBehaviour
	{
		[SerializeField]
		private Curve m_FadeInCurve;
		[SerializeField, Unsigned]
		private float m_IntroFadeInTime;
		[SerializeField, NotDefault]
		private Image m_Image;

		private void Awake()
		{
			Domain.GetSpawnSystem().OnBeginSpawn += delegate
			{
				FadeIn(m_IntroFadeInTime);
			};
			gameObject.SetActive(false);
		}
		public void FadeIn(float duration)
		{
			if (duration <= 0f || m_Image == null)
			{
				return;
			}
			m_Image.color = Color.black;
			gameObject.SetActive(true);
			StopAllCoroutines();
			StartCoroutine(FadeInAsync());
			IEnumerator FadeInAsync()
			{
				var timer = 0f;
				var tMul = 1f / duration;
				while (timer < 1f)
				{
					timer += Time.unscaledDeltaTime * tMul;
					var y = m_FadeInCurve.Evaluate(timer);
					m_Image.color = Color.black * y;
					yield return null;
				}
				gameObject.SetActive(false);
			}
		}
		public void StopAll()
		{
			StopAllCoroutines();
			gameObject.SetActive(false);
		}
	}
}