
namespace Parlor.Game
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	[Serializable, EditorHandled]
	public sealed class SfxInfo : RecordInfo
	{
		[SerializeField]
		private Bank<AudioClip> m_Bank;
		[SerializeField, Percentage]
		private float m_VolumeScale = 1.0f;

		public float VolumeScale
		{
			get => m_VolumeScale;
		}

		public AudioClip GetClip()
		{
			return m_Bank.Provide();
		}
	}
	[Serializable]
	public sealed class SfxReference : RecordReference<SfxInfo> { }
	[CreateAssetMenu(menuName = "Parlor/Runtime/SfxLibrary")]
	public sealed class SfxLibrary : RecordLibrary<SfxInfo> { }
}