
namespace Parlor.Game
{
	using System.Runtime.CompilerServices;
	using UnityEngine;
	using Parlor.Runtime;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
	public class Actor : MonoBehaviour
	{
		static public event ReactionDelegate OnReaction;
		static public event ActorDeathDelegate OnDeath;
		static public event ActorActionDelegate OnAction;

		public event PropertyChangeDelegate OnPropertyChanged;

		[Header("Actor")]
		[SerializeField, Unsigned]
		private float m_Damage;
		[SerializeField]
		private Quantity m_Health;
		[SerializeField, Unsigned]
		private Quantity m_Shield;
		[SerializeField, Unsigned]
		private float m_MoveSpeed;
		[SerializeField]
		private ActorFaction m_Faction;
		[SerializeField]
		private AfxReference m_HitAfx;
		[SerializeField]
		private AfxReference m_DeathAfx;
		[SerializeField]
		private TfxReference m_DeathTfx;
		[SerializeField]
		private SfxReference m_DeathSfx;
		[SerializeField]
		private GameObject m_ShieldVisual;
		[SerializeField, NotDefault]
		private Sprite[] m_BodyVisuals;
		private bool m_Active;
		private SpriteRenderer m_Renderer;

		public float Damage
		{
			get => m_Damage;
			set
			{
				SetProperty(ref m_Damage, Mathf.Max(value, 0f));
			}
		}
		public Quantity Health
		{
			get => m_Health;
			set
			{
				SetProperty(ref m_Health, value);
				UpdateBodyVisual();
			}
		}
		public Quantity Shield
		{
			get => m_Shield;
			set
			{
				SetProperty(ref m_Shield, value);
				UpdateShieldVisual();
			}
		}
		public float MoveSpeed
		{
			get => m_MoveSpeed;
			set
			{
				SetProperty(ref m_MoveSpeed, Mathf.Max(value, 0f));
			}
		}
		public bool Active
		{
			get => m_Active;
		}
		protected SpriteRenderer Renderer
		{
			get => m_Renderer;
		}

		protected void Awake()
		{
			m_Renderer = GetComponent<SpriteRenderer>();
		}
		protected void OnEnable()
		{
			m_Active = true;
		}
		protected void OnDisable()
		{
			m_Active = false;
		}
		public bool IsDead()
		{
			return m_Health.Current <= 0f;
		}
		public bool IsHostileAgainst(Actor other)
		{
			if (other == null) return false;
			if ((m_Faction | other.m_Faction) is ActorFaction.FullyChaotic) return true;
			if ((m_Faction & other.m_Faction) is ActorFaction.SemiChaotic) return false;
			return m_Faction != other.m_Faction;
		}
		public virtual void Respawn()
		{
			Health = Quantity.Full(m_Health.Max);
			gameObject.SetActive(true);
		}
		public void TakeDamage(Actor instigator, Vector3 hitPosition)
		{
			if (instigator == null || IsDead()) return;
			var rawDamage = instigator.Damage;
			var takenDamage = Mathf.Max(rawDamage - m_Shield.Current, 0f);
			var absorbedDamage = rawDamage - takenDamage;
			Shield = new(m_Shield.Current - absorbedDamage, m_Shield.Max);
			Health = new(m_Health.Current - takenDamage, m_Health.Max);
			if (Health.Current <= 0f) Die(instigator);
			else PlayHitEffects(hitPosition);
			OnReaction?.Invoke(new(instigator, victim: this, dealtDamage: takenDamage, hitPosition));
		}
		public virtual void Die(Actor instigator)
		{
			gameObject.SetActive(false);
			PlayDeathEffects();
			OnDeath?.Invoke(instigator, victim: this);
		}
		public bool Heal(float amount)
		{
			if (amount <= 0f || m_Health.IsFull) return false;
			amount = Mathf.Min(amount, m_Health.Max - m_Health.Current);
			Health = new(m_Health.Current + amount, m_Health.Max);
			OnAction?.Invoke(subject: this, actionName: "Heal", args: amount);
			return true;
		}
		public bool ShieldUp(float amount)
		{
			if (amount <= 0f || m_Shield.IsFull) return false;
			amount = Mathf.Min(amount, m_Shield.Max - m_Shield.Current);
			Shield = new(m_Shield.Current + amount, m_Shield.Max);
			OnAction?.Invoke(subject: this, actionName: "ShieldUp", args: amount);
			return true;
		}
		protected void SetProperty<T>(ref T property, T value, [CallerMemberName] string name = null)
		{
			property = value;
			OnPropertyChanged?.Invoke(name, value);
		}
		private void UpdateBodyVisual()
		{
			if (m_BodyVisuals.Length != 0)
			{
				var index = (int)((1.0f - m_Health.Ratio) * m_BodyVisuals.Length);
				index = Mathf.Clamp(index, 0, m_BodyVisuals.Length - 1);
				m_Renderer.sprite = m_BodyVisuals[index];
			}
		}
		private void UpdateShieldVisual()
		{
			if (m_ShieldVisual != null)
			{
				m_ShieldVisual.SetActive(m_Shield.Current > 0f);
			}
		}
		private void PlayHitEffects(Vector3 hitPosition)
		{
			m_HitAfx.Play(hitPosition);
		}
		private void PlayDeathEffects()
		{
			m_DeathAfx.Play(transform.position);
			Domain.GetCameraSystem().Shake(m_DeathTfx);
			m_DeathSfx.PlayEffect();
		}
	}
	public enum ActorFaction
	{
		Enemy,
		Player,
		SemiChaotic,
		FullyChaotic
	}
	public readonly ref struct ReactionInfo
	{
		public readonly Actor Instigator;
		public readonly Actor Victim;
		public readonly float DealtDamage;
		public readonly Vector3 HitPosition;

		public ReactionInfo(Actor instigator, Actor victim, float dealtDamage, Vector3 hitPosition)
		{
			Instigator = instigator;
			Victim = victim;
			DealtDamage = dealtDamage;
			HitPosition = hitPosition;
		}
	}
	static public class ActorProvider
	{
		static private readonly ComponentMap<Actor> s_Map;

		static ActorProvider()
		{
			s_Map = new(create: scheme => new(scheme, active: elem => elem.Active));
		}

		static public Actor Provide(Actor scheme)
		{
			return s_Map.Provide(scheme);
		}
		static public void ReturnAll()
		{
			foreach (var coll in s_Map)
			{
				foreach (var elem in coll)
				{
					elem.gameObject.SetActive(false);
				}
			}
		}
	}
	public delegate void ReactionDelegate(in ReactionInfo info);
	public delegate void ActorDeathDelegate(Actor instigator, Actor victim);
	public delegate void ActorActionDelegate(Actor subject, string actionName, object args);
	public delegate void PropertyChangeDelegate(string property, object value);
}