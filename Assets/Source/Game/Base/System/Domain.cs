#nullable enable

namespace Parlor.Game
{
	using Parlor.Diagnostics;
	using Parlor.Game.UI;

	static public class Domain
	{
		static private GameSystem? s_GameSystem;
		static private AudioSystem? s_AudioSystem;
		static private InputSystem? s_InputSystem;
		static private CameraSystem? s_CameraSystem;
		static private BoundarySystem? s_BoundarySystem;
		static private BackgroundSystem? s_BackgroundSystem;
		static private LootSystem? s_LootSystem;
		static private SpawnSystem? s_SpawnSystem;
		static private Player? s_Player;
		static private ScreenEffectSystem? s_ScreenEffectSystem;
		static private NotificationSystem? s_NotificationSystem;

		static public GameSystem GetGameSystem()
		{
			return LoadSystem(cache: ref s_GameSystem);
		}
		static public AudioSystem GetAudioSystem()
		{
			return LoadSystem(cache: ref s_AudioSystem);
		}
		static public InputSystem GetInputSystem()
		{
			return LoadSystem(cache: ref s_InputSystem);
		}
		static public CameraSystem GetCameraSystem()
		{
			return LoadSystem(cache: ref s_CameraSystem);
		}
		static public BoundarySystem GetBoundarySystem()
		{
			return LoadSystem(cache: ref s_BoundarySystem);
		}
		static public BackgroundSystem GetBackgroundSystem()
		{
			return LoadSystem(cache: ref s_BackgroundSystem);
		}
		static public LootSystem GetLootSystem()
		{
			return LoadSystem(cache: ref s_LootSystem);
		}
		static public SpawnSystem GetSpawnSystem()
		{
			return LoadSystem(cache: ref s_SpawnSystem);
		}
		static public Player GetPlayer()
		{
			return LoadSystem(cache: ref s_Player);
		}
		static public ScreenEffectSystem GetScreenEffectSystem()
		{
			return LoadSystem(cache: ref s_ScreenEffectSystem);
		}
		static public NotificationSystem GetNotificationSystem()
		{
			return LoadSystem(cache: ref s_NotificationSystem);
		}
		static private T LoadSystem<T>(ref T? cache) where T : UnityEngine.Object
		{
			if (cache == null)
			{
				cache = UnityEngine.Object.FindObjectOfType<T>(includeInactive: true);
				if (cache == null)
				{
					Log.Error($"Missing system '{typeof(T).FullName}' at scene.");
				}
			}
			return cache!;
		}
	}
}