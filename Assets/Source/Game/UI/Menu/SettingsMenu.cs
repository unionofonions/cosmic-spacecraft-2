
namespace Parlor.Game
{
	using UnityEngine;
	using UnityEngine.UI;

	[DisallowMultipleComponent]
	public sealed class SettingsMenu : MonoBehaviour
	{
		[SerializeField, NotDefault]
		private Slider m_EffectVolumeSlider;
		[SerializeField, NotDefault]
		private Slider m_MusicVolumeSlider;
		[SerializeField, NotDefault]
		private Toggle m_CameraShakeToggle;

		private void Start()
		{
			if (m_EffectVolumeSlider != null) m_EffectVolumeSlider.value = Settings.EffectVolume;
			if (m_MusicVolumeSlider != null) m_MusicVolumeSlider.value = Settings.MusicVolume;
			if (m_CameraShakeToggle != null) m_CameraShakeToggle.isOn = Settings.EnableCameraShake;
		}
	}
}