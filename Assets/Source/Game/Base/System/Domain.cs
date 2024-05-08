#nullable enable

namespace Parlor.Game
{
	using Parlor.Diagnostics;
	using Parlor.Game.UI;

	static public class Domain
	{
		static private InputSystem? s_InputSystem;
		static private CameraSystem? s_CameraSystem;
		static private BackgroundSystem? s_BackgroundSystem;
		static private LootSystem? s_LootSystem;
		static private Player? s_Player;
		static private NotificationSystem? s_NotificationSystem;

		static public InputSystem GetInputSystem()
		{
			return LoadSystem(cache: ref s_InputSystem);
		}
		static public CameraSystem GetCameraSystem()
		{
			return LoadSystem(cache: ref s_CameraSystem);
		}
		static public BackgroundSystem GetBackgroundSystem()
		{
			return LoadSystem(cache: ref s_BackgroundSystem);
		}
		static public LootSystem GetLootSystem()
		{
			return LoadSystem(cache: ref s_LootSystem);
		}
		static public Player GetPlayer()
		{
			return LoadSystem(cache: ref s_Player);
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