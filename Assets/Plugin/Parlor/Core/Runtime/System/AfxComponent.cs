#nullable enable

namespace Parlor.Runtime
{
	using UnityEngine;

	public sealed class AfxComponent : MonoBehaviour
	{
		internal AfxId Id;

		internal bool Active
		{
			get => m_Active;
		}

		internal void Play()
		{
			m_Active = true;
			gameObject.SetActive(true);
		}
		internal void Stop()
		{
			gameObject.SetActive(false);
			m_Active = false;
		}

		#region Internal
		private bool m_Active;

		private void Awake()
		{
			Id = new(this);
			m_Active = false;
		}
		#endregion // Internal
	}
}