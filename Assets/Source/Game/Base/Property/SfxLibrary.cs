
namespace Parlor.Game
{
	using System;
	using UnityEngine;
	using Parlor.Diagnostics;

	static public class SfxHandler
	{
		static private GameObject s_Container;
		static private AudioSource s_EffectPlayer;

		static private GameObject Container
		{
			get
			{
				if (s_Container == null)
				{
					s_Container = Parlor.Runtime.UnityHelper.CreateGameObject(name: nameof(SfxHandler), permanent: true);
				}
				return s_Container;
			}
		}
		static private AudioSource EffectPlayer
		{
			get
			{
				if (s_EffectPlayer == null)
				{
					s_EffectPlayer = Parlor.Runtime.UnityHelper.CreateComponent<AudioSource>(name: "effect_player");
					s_EffectPlayer.volume = 1.0f;
					s_EffectPlayer.mute = false;
					s_EffectPlayer.loop = false;
					s_EffectPlayer.playOnAwake = false;
					s_EffectPlayer.transform.SetParent(Container.transform, worldPositionStays: false);
				}
				return s_EffectPlayer;
			}
		}
		static public float EffectVolume
		{
			get => EffectPlayer.volume;
			set => EffectPlayer.volume = value;
		}
		static public bool EffectSilent
		{
			get => EffectPlayer.mute;
			set => EffectPlayer.mute = value;
		}

		static public void PlayEffect(this SfxReference reference)
		{
			if (reference == null) return;
			var info = reference.Info;
			if (info == null || !info.Valid) return;
			var clip = info.GetClip();
			if (clip == null) return;
			EffectPlayer.PlayOneShot(clip, volumeScale: info.VolumeScale);
		}
	}

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