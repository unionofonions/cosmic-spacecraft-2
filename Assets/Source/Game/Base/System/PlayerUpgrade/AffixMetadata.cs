
namespace Parlor.Game
{
	using System;
	using UnityEngine;
	using Parlor.Game.Localization;

	[CreateAssetMenu(fileName = "AffixMetadata", menuName = "Parlor/Game/AffixMetadata")]
	public sealed class AffixMetadata : ScriptableObject
	{
		static private AffixMetadata s_Instance;

		[SerializeField]
		private Color m_PositiveColor;
		[SerializeField]
		private Color m_NegativeColor;
		[SerializeField]
		private EnumMap<AffixType, AffixInfo> m_InfoMap;

		static private AffixMetadata Instance
		{
			get
			{
				if (s_Instance == null)
				{
					s_Instance = Resources.Load<AffixMetadata>("Game/AffixMetadata");
				}
				return s_Instance;
			}
		}

		static public string GetDescription(Affix affix)
		{
			if (Instance == null || affix == null)
			{
				return String.Empty;
			}
			var type = affix.AffixType;
			if ((uint)type >= (uint)Instance.m_InfoMap.Count)
			{
				return String.Empty;
			}
			var info = Instance.m_InfoMap[(int)type];
			var quality = affix.Quality;
			if ((uint)quality >= (uint)info.FortifyAmountMap.Count)
			{
				return String.Empty;
			}
			var keyword = info.DescriptionKeywordMap[(int)quality];
			//var text = TranslationSystem.KeywordToText(keyword);
			var text = keyword;
			var isNegative = IsNegative(affix);
			var color = isNegative ? Instance.m_NegativeColor : Instance.m_PositiveColor;
			return StringHelper.Colorize(text, color, close: false);
		}
		static public float GetFortifyAmount(Affix affix)
		{
			if (Instance == null || affix == null)
			{
				return 0f;
			}
			var type = affix.AffixType;
			if ((uint)type >= (uint)Instance.m_InfoMap.Count)
			{
				return 0f;
			}
			var info = Instance.m_InfoMap[(int)type];
			var quality = affix.Quality;
			if ((uint)quality >= (uint)info.FortifyAmountMap.Count)
			{
				return 0f;
			}
			return info.FortifyAmountMap[(int)quality];
		}
		static private bool IsNegative(Affix affix)
		{
			return affix.Quality is AffixQuality.MinorNegative or AffixQuality.MajorNegative;
		}

		[Serializable]
		private sealed class AffixInfo
		{
			public EnumMap<AffixQuality, string> DescriptionKeywordMap;
			public EnumMap<AffixQuality, float> FortifyAmountMap;
		}
	}
}