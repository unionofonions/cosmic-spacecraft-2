using UnityEngine;
using UnityEngine.Scripting;
using TMPro;

namespace Parlor.Game
{
	using Parlor.Game.UI;

	[DisallowMultipleComponent]
	public sealed class PlayerUpgradeSystem : MonoBehaviour
	{
		[SerializeField, Unsigned]
		private int m_RerollCount;
		[SerializeField]
		private SfxReference m_RegreshSfx;
		[SerializeField, NotDefault]
		private PlayerUpgradeCollection m_Upgrades;
		[SerializeField, NotDefault]
		private PlayerUpgradeComponent[] m_Components;
		[SerializeField, NotDefault]
		private Button m_RerollButton;
		[SerializeField, NotDefault]
		private TextMeshProUGUI m_RerollLabel;
		private int m_RerollCounter;

		private void Awake()
		{
			Domain.GetSpawnSystem().OnBeginSpawn += OnBeginSpawn;
			Actor.OnDeath += OnActorDeath;
			gameObject.SetActive(false);
		}
		private void OnBeginSpawn()
		{
			SetRerollCounter(m_RerollCount);

		}
		private void OnActorDeath(Actor instigator, Actor victim)
		{
			if (victim is EnemyShip enemyShip && (enemyShip.Flags & EnemyShipFlags.PlayerUpgradeOnDeath) != 0)
			{
				gameObject.SetActive(true);
			}
		}
		public void RefreshOptions(int isReroll)
		{
			if (isReroll != 0)
			{
				if (m_RerollCounter <= 0)
				{
					return;
				}
				SetRerollCounter(m_RerollCounter - 1);
			}
			foreach (var elem in m_Components)
			{
				if (elem != null)
				{
					var upgrade = m_Upgrades.GetUpgrade();
					elem.PlayerUpgrade = upgrade;
				}
			}
			Domain.GetAudioSystem().PlayEffect(m_RegreshSfx);
		}
		[Preserve]
		private void OnEndAnimation()
		{
			if (Domain.GetPlayer().IsDead())
			{
				gameObject.SetActive(false);
			}
			else
			{
				Domain.GetGameSystem().PauseGame();
			}
		}
		private void SetRerollCounter(int value)
		{
			m_RerollCounter = value;
			if (m_RerollButton != null)
			{
				m_RerollButton.Interactable = m_RerollCounter > 0;
			}
			if (m_RerollLabel != null)
			{
				m_RerollLabel.text = $"x{m_RerollCounter}";
			}
		}
	}
}