#nullable enable

namespace Parlor.Runtime
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	[Serializable, EditorHandled]
	public sealed class TfxInfo : RecordInfo
	{
		internal Curve3 Position
		{
			get => m_Position;
		}
		internal Curve3 Rotation
		{
			get => m_Rotation;
		}

		#region Internal
#nullable disable
		[SerializeField]
		private Curve3 m_Position;
		[SerializeField]
		private Curve3 m_Rotation;
#nullable enable
		#endregion // Internal
	}
	[Serializable]
	public sealed class TfxReference : RecordReference<TfxInfo>
	{
		public float Amplitude
		{
			get => m_Amplitude;
		}
		public float Duration
		{
			get => m_Duration;
		}

		#region Internal
		[SerializeField, Unsigned]
		private float m_Amplitude;
		[SerializeField, Unsigned]
		private float m_Duration;
		#endregion // Internal
	}
	[CreateAssetMenu(menuName = "Parlor/Runtime/TfxLibrary")]
	public sealed class TfxLibrary : RecordLibrary<TfxInfo> { }
}