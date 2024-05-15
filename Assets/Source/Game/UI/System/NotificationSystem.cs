
namespace Parlor.Game.UI
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.InteropServices;
	using UnityEngine;
	using UnityEngine.Scripting;
	using TMPro;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator))]
	public sealed class NotificationSystem : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private TextMeshProUGUI m_Label;
		private Queue<TextInfo> m_TextQueue;
		private bool m_Valid;
		private Animator m_Animator;

		private void Awake()
		{
			if (m_Label == null)
			{
				return;
			}
			m_Animator = GetComponent<Animator>();
			gameObject.SetActive(false);
			m_TextQueue = new();
			m_Valid = true;
		}
		public void ShowText(string text, [Optional] string animationName)
		{
			if (m_Valid && !String.IsNullOrEmpty(text))
			{
				animationName ??= "default";
				if (!gameObject.activeSelf)
				{
					ShowTextInternal(text, animationName);
				}
				else
				{
					m_TextQueue.Enqueue(new(text, animationName));
				}
			}
		}
		public void HideText()
		{
			m_TextQueue.Clear();
			m_Label.text = String.Empty;
			gameObject.SetActive(false);
		}
		private void ShowTextInternal(string text, string animationName)
		{
			m_Label.text = text;
			gameObject.SetActive(true);
			m_Animator.Play(animationName);
		}
		[Preserve]
		private void OnAnimationEnd()
		{
			if (m_TextQueue.TryDequeue(out var textInfo))
			{
				ShowTextInternal(textInfo.Text, textInfo.AnimationName);
			}
			else
			{
				HideText();
			}
		}

		private readonly struct TextInfo
		{
			public readonly string Text;
			public readonly string AnimationName;

			public TextInfo(string text, string animationName)
			{
				Text = text;
				AnimationName = animationName;
			}
		}
	}
}