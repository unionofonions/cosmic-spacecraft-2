
namespace Parlor.Game
{
	using UnityEngine;

	[CreateAssetMenu(menuName = "Parlor/Game/LootInfo")]
	public sealed class LootInfo : ScriptableObject
	{
		[SerializeField, Percentage]
		private float m_HealDropChance;
		[SerializeField, Percentage]
		private float m_ShieldDropChance;
		[SerializeField]
		private Range m_HealAmount;
		[SerializeField]
		private Range m_ShieldAmount;

		public float HealDropChance
		{
			get => m_HealDropChance;
		}
		public float ShieldDropChance
		{
			get => m_ShieldDropChance;
		}
		public Range HealAmount
		{
			get => m_HealAmount;
		}
		public Range ShieldAmount
		{
			get => m_ShieldAmount;
		}
	}
}