#nullable enable

namespace Parlor.Runtime
{
	using UnityEngine;

	[RequireComponent(typeof(ParticleSystem))]
	public sealed class PfxComponent : MonoBehaviour
	{
		internal PfxId Id;

		internal bool Active
		{
			get => m_ParticleSystem.isEmitting;
		}

		internal void Play()
		{
			m_ParticleSystem.Play();
			++Id.Version;
		}
		internal void Stop(bool clear)
		{
			m_ParticleSystem.Stop(
				withChildren: true,
				clear ? ParticleSystemStopBehavior.StopEmittingAndClear : ParticleSystemStopBehavior.StopEmitting);
		}

		#region Internal
#nullable disable
		private ParticleSystem m_ParticleSystem;

		private void Awake()
		{
			m_ParticleSystem = GetComponent<ParticleSystem>();
			Id = new(this);
		}
#nullable enable
		#endregion // Internal
	}
}