
namespace Parlor.Game
{
	using System;
	using UnityEngine;

	[Serializable]
	public sealed class PlayerUpgrade
	{
		[SerializeField, NotDefault]
		private Sprite m_Icon;
		[SerializeField]
		private Affix[] m_Affixes;

		public Sprite Icon
		{
			get => m_Icon;
		}
		public Affix[] Affixes
		{
			get => m_Affixes;
		}
	}
}