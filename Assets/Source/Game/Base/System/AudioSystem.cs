
namespace Parlor.Game
{
	using System.Collections;
	using UnityEngine;

	[DisallowMultipleComponent]
	public sealed class AudioSystem : MonoBehaviour
	{
		private bool m_Initialized;
		private bool m_InTransition;
		private float m_ReflectedMusicVolume;
		private float m_MusicVolumeScale;
		private AudioSource m_EffectPlayer;
		private AudioSource m_MusicPlayer;
		private AudioSource m_TransitionPlayer;

		public float EffectVolume
		{
			get
			{
				LazyAwake();
				return m_EffectPlayer.volume;
			}
			set
			{
				LazyAwake();
				m_EffectPlayer.volume = value;
			}
		}
		public float MusicVolume
		{
			get
			{
				LazyAwake();
				return m_ReflectedMusicVolume;
			}
			set
			{
				LazyAwake();
				m_ReflectedMusicVolume = Mathf.Clamp01(value);
				if (!m_InTransition)
				{
					m_MusicPlayer.volume = FinalMusicVolume;
				}
			}
		}
		private float FinalMusicVolume
		{
			get => m_ReflectedMusicVolume * m_MusicVolumeScale;
		}

		private void LazyAwake()
		{
			if (!m_Initialized)
			{
				m_Initialized = true;
				m_EffectPlayer = CreateAudioSource("effect_player");
				m_MusicPlayer = CreateAudioSource("music_player");
				m_TransitionPlayer = CreateAudioSource("transition_player");
				m_MusicPlayer.loop = true;
				m_TransitionPlayer.loop = true;
				m_ReflectedMusicVolume = 1.0f;
			}
		}
		public void PlayEffect(SfxReference reference)
		{
			if (reference == null) return;
			var info = reference.Info;
			if (info == null || !info.Valid) return;
			var clip = info.GetClip();
			if (clip == null) return;
			LazyAwake();
			m_EffectPlayer.PlayOneShot(clip, volumeScale: info.VolumeScale);
		}
		public void PlayMusic(SfxReference reference, IFunction function, float time)
		{
			if (reference == null) return;
			var info = reference.Info;
			if (info == null || !info.Valid) return;
			var clip = info.GetClip();
			if (clip == null) return;
			LazyAwake();
			m_MusicVolumeScale = info.VolumeScale;
			m_MusicPlayer.volume = FinalMusicVolume;
			if (m_MusicPlayer.isPlaying && time > 0f)
			{
				function ??= Formula.EaseInOut;
				StopAllCoroutines();
				StartCoroutine(Transition(clip, function, time));
			}
			else
			{
				m_MusicPlayer.clip = clip;
				m_MusicPlayer.Play();
			}
		}
		public void PauseMusic()
		{
			LazyAwake();
			m_MusicPlayer.Pause();
		}
		public void ResumeMusic()
		{
			LazyAwake();
			m_MusicPlayer.UnPause();
		}
		private AudioSource CreateAudioSource(string name)
		{
			var ret = Parlor.Runtime.UnityHelper.CreateComponent<AudioSource>(name);
			ret.transform.SetParent(transform, worldPositionStays: false);
			ret.playOnAwake = false;
			return ret;
		}
		private IEnumerator Transition(AudioClip clip, IFunction function, float time)
		{
			m_InTransition = true;
			m_TransitionPlayer.clip = m_MusicPlayer.clip;
			m_TransitionPlayer.volume = m_MusicPlayer.volume;
			m_TransitionPlayer.Play();
			m_TransitionPlayer.time = m_MusicPlayer.time;
			m_MusicPlayer.Stop();
			m_MusicPlayer.clip = clip;
			m_MusicPlayer.volume = 0f;
			m_MusicPlayer.Play();
			var timer = 0f;
			var tMul = 1f / time;
			var volume = FinalMusicVolume;
			while (timer < 1f)
			{
				timer += Time.unscaledDeltaTime * tMul;
				var y = function.Evaluate(timer);
				m_TransitionPlayer.volume = volume * (1f - y);
				m_MusicPlayer.volume = volume * y;
				yield return null;
			}
			m_TransitionPlayer.Stop();
			m_MusicPlayer.volume = FinalMusicVolume;
			m_InTransition = false;
		}
	}
}