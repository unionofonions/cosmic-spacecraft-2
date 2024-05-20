
namespace Parlor.Game
{
	using System;
	using UnityEngine;

	[Serializable]
	public sealed class Affix
	{
		[SerializeField]
		private AffixType m_AffixType;
		[SerializeField]
		private AffixQuality m_Quality;

		public AffixType AffixType
		{
			get => m_AffixType;
		}
		public AffixQuality Quality
		{
			get => m_Quality;
		}
		public string Description
		{
			get => AffixMetadata.GetDescription(affix: this);
		}

		public void Apply(Actor target)
		{
			if (target != null)
			{
				var amount = AffixMetadata.GetFortifyAmount(affix: this);
				var ship = target as Ship;
				var player = target as Player;
				switch (m_AffixType)
				{
					case AffixType.FortifyDamage:
						target.Damage += amount;
						return;
					case AffixType.FortifyCritChance:
						target.CritChance += amount;
						return;
					case AffixType.FortifyCritDamage:
						target.CritDamage += amount;
						return;
					case AffixType.FortifyArmorPenetration:
						target.ArmorPenetration += amount;
						return;
					case AffixType.FortifyHealth:
						var health = target.Health;
						target.Health = new(health.Current + amount, health.Max + amount);
						return;
					case AffixType.FortifyArmor:
						target.Armor += amount;
						return;
					case AffixType.FortifyShield:
						var shield = target.Shield;
						target.Shield = new(shield.Current + amount, shield.Max + amount);
						return;
					case AffixType.FortifyMoveSpeed:
						target.MoveSpeed += amount;
						return;
					case AffixType.FortifyFireRate:
						if (ship != null) ship.FireInterval -= amount;
						return;
					case AffixType.FortifyBulletSpeed:
						if (ship != null) ship.BulletSpeed += amount;
						return;
					case AffixType.FortifyGunCount:
						if (player != null) player.GunCount += (int)amount;
						return;
					default:
						return;
				}
			}
		}
	}
	public enum AffixType
	{
		FortifyDamage,
		FortifyCritChance,
		FortifyCritDamage,
		FortifyArmorPenetration,
		FortifyHealth,
		FortifyArmor,
		FortifyShield,
		FortifyMoveSpeed,
		FortifyFireRate,
		FortifyBulletSpeed,
		FortifyGunCount
	}
	public enum AffixQuality
	{
		MinorPositive,
		MajorPositive,
		MinorNegative,
		MajorNegative
	}
}