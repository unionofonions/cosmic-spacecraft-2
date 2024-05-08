
namespace Parlor.Game.UI
{
	using UnityEngine;
	using UnityEngine.UI;

	[DisallowMultipleComponent]
	public sealed class PlayerHealthBar : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private Animator m_Heart;
		[SerializeField, NotDefault]
		private Image m_HealthFill;
		[SerializeField, NotDefault]
		private Image m_ShieldFill;
		private int m_HealHash;
		private int m_HitHash;

		private void Awake()
		{
			m_HealHash = Animator.StringToHash("heal");
			m_HitHash = Animator.StringToHash("hit");
			Actor.OnReaction += OnReaction;
			Actor.OnAction += OnAction;
			Domain.GetPlayer().OnPropertyChanged += OnPropertyChanged;
		}
		private void OnReaction(in ReactionInfo info)
		{
			if (info.Victim is Player && m_Heart != null)
			{
				m_Heart.Play(m_HitHash);
			}
		}
		private void OnAction(Actor subject, string actionName, object args)
		{
			if (subject is Player && actionName is "Heal" or "ShieldUp" && m_Heart != null)
			{
				m_Heart.Play(m_HealHash);
			}
		}
		private void OnPropertyChanged(string property, object value)
		{
			switch (property)
			{
				case nameof(Player.Health):
					if (m_HealthFill != null) m_HealthFill.fillAmount = ((Quantity)value).Ratio;
					return;
				case nameof(Player.Shield):
					if (m_ShieldFill != null) m_ShieldFill.fillAmount = ((Quantity)value).Ratio;
					return;
			}
		}
	}
}