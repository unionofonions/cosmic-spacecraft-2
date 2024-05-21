
namespace Parlor.Game
{
	using UnityEngine;

	[DisallowMultipleComponent]
	public sealed class MusicSystem : MonoBehaviour
	{
		[SerializeField]
		private SfxReference m_MainMenuClips;
		[SerializeField]
		private SfxReference m_EndGameClips;
		[SerializeField]
		private SfxReference m_LevelClips;
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
				m_MainMenuClips,
				m_TransitionFunction,
				m_MainMenuTransitionTime);
		}
		public void PlayEndGameMusic()
		{
			Domain.GetAudioSystem().PlayMusic(
				m_EndGameClips,
				m_TransitionFunction,
				m_EndGameTransitionTime);
		}
		public void PlayLevelMusic()
		{
			Domain.GetAudioSystem().PlayMusic(
				m_LevelClips,
				m_TransitionFunction,
				m_LevelTransitionTime);
		}
	}
}