
namespace Parlor.Game
{
	using System;
	using UnityEngine;
	using Parlor.Runtime;

	[DefaultExecutionOrder(Int32.MinValue)]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Camera))]
	public sealed class CameraSystem : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private TfxComponent m_TfxComponent;
		private Camera m_Camera;

		public Camera Camera
		{
			get
			{
				LazyAwake();
				return m_Camera;
			}
		}
		public float FieldOfView
		{
			get
			{
				LazyAwake();
				return m_Camera.fieldOfView;
			}
			set
			{
				LazyAwake();
				m_Camera.fieldOfView = value;
			}
		}

		private void LazyAwake()
		{
			if (m_Camera == null)
			{
				m_Camera = GetComponent<Camera>();
			}
		}
		public void Shake(TfxReference reference)
		{
			if (m_TfxComponent != null)
			{
				m_TfxComponent.Play(reference);
			}
		}
		public void StopShake()
		{
			if (m_TfxComponent != null)
			{
				m_TfxComponent.StopAll();
			}
		}
	}
}