
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
		[SerializeField, NotDefault]
		private Image m_Image;

		private void Awake()
		{
			if (Time.frameCount == 0)
			{
				gameObject.SetActive(false);
			}
		}
		public void FadeIn(float duration)
		{
			if (m_Image == null) return;
			m_Image.color = Color.black;
			gameObject.SetActive(true);
			StopAllCoroutines();
			StartCoroutine(FadeInAsync());
			IEnumerator FadeInAsync()
			{
				var timer = 0f;
				var tMul = 1f / duration;
				while (timer < duration)
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