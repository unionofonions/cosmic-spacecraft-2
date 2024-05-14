
namespace Parlor.Game.UI
{
	using System;
	using UnityEngine;
	using UnityEngine.Events;
	using Parlor.Diagnostics;

	[EditorHandled]
	public class CallbackListener : MonoBehaviour
	{
		[SerializeField]
		private CallbackType m_Callbacks;
		[SerializeField]
		private UnityEvent m_OnAwake;
		[SerializeField]
		private UnityEvent m_OnStart;
		[SerializeField]
		private UnityEvent m_OnEnable;
		[SerializeField]
		private UnityEvent m_OnDisable;

		protected void Awake()
		{
			m_OnAwake.Invoke();
		}
		protected void Start()
		{
			m_OnStart.Invoke();
		}
		protected void OnEnable()
		{
			m_OnEnable.Invoke();
		}
		protected void OnDisable()
		{
			m_OnDisable.Invoke();
		}
	}
	[Flags]
	public enum CallbackType
	{
		Awake = 0x1,
		Start = 0x2,
		OnEnable = 0x4,
		OnDisable = 0x8,
	}
}