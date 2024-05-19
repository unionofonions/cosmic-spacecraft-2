
namespace Parlor.Game.Localization
{
	using System;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Parlor/Game/Localization/Locale")]
	public class Locale : ScriptableObject
	{
		[SerializeField]
		private KeywordTextPair[] m_KeywordTextPairs;
		[NonSerialized]
		private KeywordTextPair[] m_OrderedKeywordTextPairs;

		public string KeywordToText(string keyword)
		{
			var hash = StringToHash(keyword);
			var coll = GetOrderedKeywordTextPairs();
			var low = 0;
			var high = coll.Length - 1;
			while (low <= high)
			{
				var mid = low + (high - low) / 2;
				var _hash = coll[mid].Hash;
				if (hash > _hash) low = mid + 1;
				else if (hash < _hash) high = mid - 1;
				else return coll[mid].Text;
			}
			return null;
		}
		private int StringToHash(string str)
		{
			return Animator.StringToHash(str);
		}
		private KeywordTextPair[] GetOrderedKeywordTextPairs()
		{
			if (m_OrderedKeywordTextPairs == null)
			{
				m_OrderedKeywordTextPairs = new KeywordTextPair[m_KeywordTextPairs.Length];
				for (var i = 0; i < m_KeywordTextPairs.Length; ++i)
				{
					m_OrderedKeywordTextPairs[i] = m_KeywordTextPairs[i];
					m_OrderedKeywordTextPairs[i].Hash = StringToHash(m_KeywordTextPairs[i].Keyword);
				}
				Array.Sort(m_OrderedKeywordTextPairs, (e1, e2) => e1.Hash.CompareTo(e2.Hash));
			}
			return m_OrderedKeywordTextPairs;
		}

		[Serializable]
		private sealed class KeywordTextPair
		{
			[NotDefault]
			public string Keyword;
			[NotDefault]
			public string Text;
			[NonSerialized]
			public int Hash;
		}
	}
}