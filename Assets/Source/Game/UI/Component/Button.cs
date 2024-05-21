
namespace Parlor.Game.UI
{
	using System;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;
	using TMPro;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Image))]
	public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
	{
		public event UnityAction OnClick
		{
			add
			{
				m_OnClick.AddListener(value);
			}
			remove
			{
				m_OnClick.RemoveListener(value);
			}
		}

		[SerializeField]
		private UnityEvent m_OnClick;
		[SerializeField]
		private ButtonInfo m_ButtonInfo;
		[SerializeField]
		private TextMeshProUGUI m_Label;
		private bool m_Interactable;
		private Color m_ImageIdleColor;
		private Color m_LabelIdleColor;
		private Image m_Image;

		public bool Interactable
		{
			get => m_Interactable;
			set
			{
				m_Interactable = value;
				if (!m_Interactable)
				{
					ResetProperties();
					m_Image.color = m_ButtonInfo.ImageDisabledColor;
					if (m_Label != null) m_Label.color = m_ButtonInfo.LabelDisabledColor;
				}
			}
		}

		protected void Awake()
		{
			m_Image = GetComponent<Image>();
			m_ImageIdleColor = m_Image.color;
			if (m_Label != null) m_LabelIdleColor = m_Label.color;
			m_Interactable = true;
		}
		protected void OnEnable()
		{
			ResetProperties();
			Interactable = m_Interactable;
		}
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!m_Interactable) return;
			m_Image.color = m_ButtonInfo.ImageHoverColor;
			if (m_Label != null) m_Label.color = m_ButtonInfo.LabelHoverColor;
			transform.localScale = Vector3.one * m_ButtonInfo.ImageHoverScale;
			Domain.GetAudioSystem().PlayEffect(m_ButtonInfo.HoverSfx);
		}
		public void OnPointerExit(PointerEventData eventData)
		{
			if (m_Interactable)
			{
				ResetProperties();
			}
		}
		public void OnPointerDown(PointerEventData eventData)
		{
			if (m_Interactable && eventData.button is PointerEventData.InputButton.Left)
			{
				transform.localEulerAngles = Vector3.forward * m_ButtonInfo.ImageDownRotation;
			}
		}
		public void OnPointerUp(PointerEventData eventData)
		{
			if (m_Interactable && eventData.button is PointerEventData.InputButton.Left)
			{
				transform.localEulerAngles = Vector3.zero;
			}
		}
		public void OnPointerClick(PointerEventData eventData)
		{
			if (m_Interactable && eventData.button is PointerEventData.InputButton.Left)
			{
				m_OnClick.Invoke();
				Domain.GetAudioSystem().PlayEffect(m_ButtonInfo.ClickSfx);
			}
		}
		private void ResetProperties()
		{
			m_Image.color = m_ImageIdleColor;
			if (m_Label != null) m_Label.color = m_LabelIdleColor;
			transform.localScale = Vector3.one;
			transform.localEulerAngles = Vector3.zero;
		}

		[Serializable]
		private sealed class ButtonInfo
		{
			public Color ImageHoverColor = Color.black;
			public Color LabelHoverColor = Color.black;
			public Color ImageDisabledColor = Color.black;
			public Color LabelDisabledColor = Color.black;
			public float ImageHoverScale = 1f;
			public float ImageDownRotation = 0f;
			public SfxReference HoverSfx;
			public SfxReference ClickSfx;
		}
	}
}