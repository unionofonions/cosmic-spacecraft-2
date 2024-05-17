
namespace Parlor.Game
{
	using UnityEngine;

	[DisallowMultipleComponent]
	public sealed class MusicSystem : MonoBehaviour
	{
		[SerializeField]
		private SfxReference m_MainMenuClip;
		[SerializeField]
		private SfxReference m_EndGameClip;
		[SerializeField]
		private Bank<SfxReference> m_LevelClips;
		[SerializeField]
		private Formula m_TransitionFunction;
		[SerializeField, Unsigned]
		private float m_MainMenuTransitionTime;
		[SerializeField, Unsigned]
		private float m_EndGameTransitionTime;
		[SerializeField, Unsigned]
		private float m_LevelTransitionTime;

		private void Start()
		{
			Domain.GetSpawnSystem().OnBeginLevel += level =>
			{
				PlayLevelMusic();
			};
		}
		public void PlayMainMenuMusic()
		{
			Domain.GetAudioSystem().PlayMusic(
				m_MainMenuClip,
				m_TransitionFunction,
				m_MainMenuTransitionTime);
		}
		public void PlayEndGameMusic()
		{
			Domain.GetAudioSystem().PlayMusic(
				m_EndGameClip,
				m_TransitionFunction,
				m_EndGameTransitionTime);
		}
		public void PlayLevelMusic()
		{
			var clip = m_LevelClips.Provide();
			Domain.GetAudioSystem().PlayMusic(
				clip,
				m_TransitionFunction,
				m_LevelTransitionTime);
		}
	}
}