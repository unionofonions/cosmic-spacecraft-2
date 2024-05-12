using System;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

namespace Parlor.Game.UI
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator))]
	public sealed class NotificationSystem : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private TextMeshProUGUI m_Label;
		private AsyncOperation m_DelayOp;
		private Animator m_Animator;

		public bool Active
		{
			get => gameObject.activeSelf;
		}

		private void Awake()
		{
			if (Time.frameCount == 0)
			{
				gameObject.SetActive(false);
			}
		}
		private void LazyAwake()
		{
			if (m_Animator == null)
			{
				m_Animator = GetComponent<Animator>();
			}
		}
		public void ShowText(string text, [Optional] string animationName, [Optional] float delay)
		{
			if (String.IsNullOrEmpty(text) || m_Label == null)
			{
				return;
			}
			animationName ??= "default";
			if (delay <= 0f)
			{
				ShowTextInternal(text, animationName);
			}
			else
			{
				m_DelayOp = AsyncOperation.InvokeDelayed(
					() => ShowTextInternal(text, animationName),
					AsyncTime.Scaled(delay));
			}
		}
		public void HideText(int abortDelayed = 1)
		{
			if (abortDelayed != 0) m_DelayOp.Abort();
			if (m_Label != null) m_Label.text = String.Empty;
			gameObject.SetActive(false);
		}
		private void ShowTextInternal(string text, string animationName)
		{
			LazyAwake();
			m_Label.text = text;
			gameObject.SetActive(true);
			m_Animator.Play(animationName);
		}
	}
}