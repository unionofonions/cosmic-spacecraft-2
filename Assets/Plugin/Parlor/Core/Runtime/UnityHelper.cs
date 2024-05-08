#nullable enable

namespace Parlor.Runtime
{
	using System;
	using System.Runtime.InteropServices;
	using UnityEngine;

	static public class UnityHelper
	{
		static public GameObject CreateGameObject([Optional] string? name, [Optional] bool permanent)
		{
			var ret = new GameObject(name ?? String.Empty);
			if (permanent) UnityEngine.Object.DontDestroyOnLoad(ret);
			return ret;
		}
		static public T CreateComponent<T>([Optional] string? name, [Optional] bool permanent) where T : Component
		{
			return CreateGameObject(name: name ?? typeof(T).PartialName(), permanent: permanent)
				.AddComponent<T>();
		}
		static public T InstantiateScheme<T>(T scheme, [Optional] string? name, [Optional] bool permanent) where T : Component
		{
			if (scheme == null) throw new ArgumentNullException(nameof(scheme));
			var ret = UnityEngine.Object.Instantiate(scheme);
			ret.name = name ?? scheme.name;
			if (permanent) UnityEngine.Object.DontDestroyOnLoad(ret);
			return ret;
		}
	}
}