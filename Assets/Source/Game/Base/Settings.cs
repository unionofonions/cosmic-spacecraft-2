
namespace Parlor.Game
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "Settings", menuName = "Parlor/Game/Settings")]
	public sealed class Settings : ScriptableObject
	{
		static private Settings s_Instance;

		[SerializeField, Percentage]
		private float m_EffectVolume;
		[SerializeField, Percentage]
		private float m_MusicVolume;
		[SerializeField]
		private bool m_EnableCameraShake;

		static private Settings Instance
		{
			get
			{
				if (s_Instance == null)
				{
					s_Instance = Resources.Load<Settings>("Game/Settings");
				}
				return s_Instance;
			}
		}
		static public float EffectVolume
		{
			get
			{
				if (Instance == null) return 1.0f;
				return Instance.m_EffectVolume;
			}
			set
			{
				if (Instance != null)
				{
					Instance.m_EffectVolume = Mathf.Clamp01(value);
				}
			}
		}
		static public float MusicVolume
		{
			get
			{
				if (Instance == null) return 1.0f;
				return Instance.m_MusicVolume;
			}
			set
			{
				if (Instance != null)
				{
					Instance.m_MusicVolume = Mathf.Clamp01(value);
				}
			}
		}
		static public bool EnableCameraShake
		{
			get
			{
				if (Instance == null) return true;
				return Instance.m_EnableCameraShake;
			}
			set
			{
				if (Instance != null)
				{
					Instance.m_EnableCameraShake = value;
				}
			}
		}
	}
}