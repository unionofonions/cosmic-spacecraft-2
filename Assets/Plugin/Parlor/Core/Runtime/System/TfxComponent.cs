#nullable enable

using UnityEngine;

namespace Parlor.Runtime
{
	using Parlor.Diagnostics;

	public sealed class TfxComponent : MonoBehaviour
	{
		public void Play(TfxReference? reference, float amplitude)
		{
			Contract.Assume(Application.isPlaying);
			if (!m_Valid || reference == null) return;
			var info = reference.Info;
			if (info == null || !info.Valid || reference.Duration <= 0f) return;
			amplitude *= reference.Amplitude;
			if (amplitude <= 0f) return;
			var processor = m_Pool.Provide();
			processor.Restart(info, amplitude, reference.Duration);
			if (++m_Pool.ActiveCount == 1) enabled = true;
		}
		public void Play(TfxReference? reference)
		{
			Play(reference, amplitude: 1f);
		}
		public bool Active()
		{
			return m_Valid && m_Pool.ActiveCount > 0;
		}
		public void StopAll()
		{
			if (!m_Valid) return;
			foreach (var elem in m_Pool) elem.Active = false;
			m_Pool.ActiveCount = 0;
			enabled = false;
			transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		}

		#region Internal
#nullable disable
		private Pool m_Pool;
		private bool m_Valid;

		private void Awake()
		{
			m_Pool = new(transform);
			enabled = false;
			m_Valid = true;
		}
		private void Update()
		{
			foreach (var elem in m_Pool)
			{
				if (elem.Active && !elem.Update() && --m_Pool.ActiveCount == 0)
				{
					enabled = false;
					transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
				}
			}
		}

		private sealed class Processor
		{
			public Processor(Transform transform)
			{
				m_Transform = transform;
			}

			private readonly Transform m_Transform;
			private Curve3 m_Position;
			private Curve3 m_Rotation;
			private float m_Timer;
			private float m_TMul;
			private Vector3 m_PosMul;
			private Vector3 m_RotMul;
			private Vector3 m_PrevPos;
			private Vector3 m_PrevRot;
			public bool Active;

			public void Restart(TfxInfo info, float amplitude, float duration)
			{
				m_Position = info.Position;
				m_Rotation = info.Rotation;
				m_Timer = 0f;
				m_TMul = 1f / duration;
				m_PosMul = Sign() * amplitude;
				m_RotMul = Sign() * amplitude;
				m_PrevPos = Vector3.zero;
				m_PrevRot = Vector3.zero;
				Active = true;
				static Vector3 Sign() => new(Random.Sign(), Random.Sign(), Random.Sign());
			}
			public bool Update()
			{
				bool ret;
				if ((m_Timer += Time.deltaTime * m_TMul) >= 1f)
				{
					m_Timer = 1f;
					ret = false;
					Active = false;
				}
				else ret = true;
				if (m_Position.Valid)
				{
					var reg = m_Position.Evaluate(m_Timer);
					var curr = new Vector3(reg.x * m_PosMul.x, reg.y * m_PosMul.y, reg.z * m_PosMul.z);
					m_Transform.localPosition += curr - m_PrevPos;
					m_PrevPos = curr;
				}
				if (m_Rotation.Valid)
				{
					var reg = m_Rotation.Evaluate(m_Timer);
					var curr = new Vector3(reg.x * m_RotMul.x, reg.y * m_RotMul.y, reg.z * m_RotMul.z);
					m_Transform.localEulerAngles += curr - m_PrevRot;
					m_PrevRot = curr;
				}
				return ret;
			}
		}
		private sealed class Pool : Pool<Processor>
		{
			public Pool(Transform transform)
			: base(active: elem => elem.Active)
			{
				m_Transform = transform;
				ActiveCount = 0;
			}

			private readonly Transform m_Transform;
			public int ActiveCount;

			protected override Processor Create()
			{
				return new(m_Transform);
			}
		}
#nullable enable
		#endregion // Internal
	}
}