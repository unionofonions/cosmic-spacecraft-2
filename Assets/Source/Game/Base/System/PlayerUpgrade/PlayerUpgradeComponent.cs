
namespace Parlor.Game
{
	using System;
	using UnityEngine;
	using TMPro;

	[DisallowMultipleComponent]
	public sealed class PlayerUpgradeComponent : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private TextMeshProUGUI[] m_Labels;
		private PlayerUpgrade m_PlayerUpgrade;

		public PlayerUpgrade PlayerUpgrade
		{
			get => m_PlayerUpgrade;
			set
			{
				m_PlayerUpgrade = value;
				UpdateLabels();
			}
		}

		public void ApplyUpgrade()
		{
			if (m_PlayerUpgrade != null)
			{
				var player = Domain.GetPlayer();
				foreach (var elem in m_PlayerUpgrade.Affixes)
				{
					elem.Apply(player);
				}
			}
		}
		private void UpdateLabels()
		{
			foreach (var elem in m_Labels)
			{
				elem.text = String.Empty;
			}
			if (m_PlayerUpgrade != null)
			{
				var length = Math.Min(m_PlayerUpgrade.Affixes.Length, m_Labels.Length);
				for (var i = 0; i < length; ++i)
				{
					m_Labels[i].text = m_PlayerUpgrade.Affixes[i].Description;
				}
			}
		}
	}
}