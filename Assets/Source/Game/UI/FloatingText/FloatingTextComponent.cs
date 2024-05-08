
namespace Parlor.Game.UI
{
	using System;
	using UnityEngine;
	using TMPro;

	[DefaultExecutionOrder(Int32.MaxValue)]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Canvas), typeof(Animator))]
	public sealed class FloatingTextComponent : MonoBehaviour
	{
		static private readonly int s_BasicDamageHash;
		static private readonly int s_HealHash;
		static private readonly int s_ShieldUpHash;

		[SerializeField, NotDefault]
		private TextMeshProUGUI m_Label;
		private bool m_Active;
		private Animator m_Animator;

		static FloatingTextComponent()
		{
			s_BasicDamageHash = Animator.StringToHash("basic_damage");
			s_HealHash = Animator.StringToHash("heal");
			s_ShieldUpHash = Animator.StringToHash("shield_up");
		}

		public bool Active
		{
			get => m_Active;
		}

		private void Awake()
		{
			m_Animator = GetComponent<Animator>();
			var canvas = GetComponent<Canvas>();
			canvas.worldCamera = Domain.GetCameraSystem().Camera;
		}
		private void OnEnable()
		{
			m_Active = true;
		}
		private void OnDisable()
		{
			m_Active = false;
		}
		public void PlayReactionAnimation(Vector3 position, in ReactionInfo info)
		{
			var text = ((int)info.DealtDamage).ToString();
			PlayAnimation(position, text, s_BasicDamageHash);
		}
		public void PlayActionAnimation(Vector3 position, string actionName, object args)
		{
			string text;
			int hash;
			switch (actionName)
			{
				case "Heal":
					text = $"+{(int)(float)args}";
					hash = s_HealHash;
					break;
				case "ShieldUp":
					text = $"+{(int)(float)args}";
					hash = s_ShieldUpHash;
					break;
				default:
					return;
			}
			PlayAnimation(position, text, hash);
		}
		public void Stop()
		{
			gameObject.SetActive(false);
		}
		private void PlayAnimation(Vector3 position, string text, int animHash)
		{
			if (m_Label == null) return;
			transform.position = position;
			m_Label.text = text;
			gameObject.SetActive(true);
			m_Animator.Play(animHash);
		}
	}
}