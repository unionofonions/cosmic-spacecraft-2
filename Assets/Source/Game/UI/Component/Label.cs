
namespace Parlor.Game.UI
{
	using UnityEngine;
	using TMPro;
	using Parlor.Game.Localization;

	[RequireComponent(typeof(TextMeshProUGUI))]
	public class Label : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private string m_Keyword;
		private bool m_Dirty;
		private TextMeshProUGUI m_Renderer;

		protected void Awake()
		{
			m_Renderer = GetComponent<TextMeshProUGUI>();
			m_Dirty = true;
			TranslationSystem.OnLanguageChanged += OnLanguageChanged;
		}
		private void OnEnable()
		{
			if (m_Dirty)
			{
				UpdateText();
				m_Dirty = false;
			}
		}
		private void UpdateText()
		{
			m_Renderer.text = TranslationSystem.KeywordToText(m_Keyword);
		}
		private void OnLanguageChanged(Language language)
		{
			if (gameObject.activeSelf)
			{
				UpdateText();
			}
			else m_Dirty = true;
		}
	}
}