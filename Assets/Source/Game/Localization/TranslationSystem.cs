
namespace Parlor.Game.Localization
{
	using System;
	using UnityEngine;

	[CreateAssetMenu(fileName = "TranslationSystem", menuName = "Parlor/Game/Localization/TranslationSystem")]
	public sealed class TranslationSystem : ScriptableObject
	{
		static public event Action<Language> OnLanguageChanged;

		static private TranslationSystem s_Instance;

		[SerializeField]
		private Language m_CurrentLanguage;
		[SerializeField]
		private EnumMap<Language, Locale> m_LocaleMap;

		static private TranslationSystem Instance
		{
			get
			{
				if (s_Instance == null)
				{
					s_Instance = Resources.Load<TranslationSystem>("Game/Localization/TranslationSystem");
				}
				return s_Instance;
			}
		}
		static public Language CurrentLanguage
		{
			get
			{
				if (Instance == null) return Language.English;
				return Instance.m_CurrentLanguage;
			}
			set
			{
				if (Instance != null && (uint)value < (uint)Instance.m_LocaleMap.Count)
				{
					Instance.m_CurrentLanguage = value;
					OnLanguageChanged?.Invoke(value);
				}
			}
		}

		static public string KeywordToText(string keyword)
		{
			if (Instance == null)
			{
				return MissingAsset();
			}
			var locale = Instance.m_LocaleMap[(int)Instance.m_CurrentLanguage];
			if (locale == null)
			{
				return MissingLocale();
			}
			var ret = locale.KeywordToText(keyword);
			if (ret == null)
			{
				return MissingKeyword(keyword);
			}
			return ret;
		}
		static public string KeywordToTextFormat(string keyword, params object[] args)
		{
			var format = KeywordToText(keyword);
			var ret = StringHelper.FormatSafe(format, args);
			if (ret == null)
			{
				return InvalidFormat(format);
			}
			return ret;
		}
		static public void SetCurrentLanguageByIndex(int index)
		{
			if (Instance != null && (uint)index < (uint)Instance.m_LocaleMap.Count)
			{
				CurrentLanguage = (Language)index;
			}
		}
		static private string MissingAsset()
		{
#if UNITY_EDITOR
			return "$missing_asset$";
#else
			return String.Empty;
#endif
		}
		static private string MissingLocale()
		{
#if UNITY_EDITOR
			return $"$missing_locale$";
#else
			return String.Empty;
#endif
		}
		static private string MissingKeyword(string keyword)
		{
#if UNITY_EDITOR
			return $"${keyword}$";
#else
			return String.Empty;
#endif
		}
		static private string InvalidFormat(string format)
		{
#if UNITY_EDITOR
			return $"${format}$";
#else
			return String.Empty;
#endif
		}
	}
}