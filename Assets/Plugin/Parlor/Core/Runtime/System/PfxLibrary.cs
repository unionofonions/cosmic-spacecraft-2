#nullable enable

namespace Parlor.Runtime
{
	using System;
	using System.Diagnostics.CodeAnalysis;
	using UnityEngine;
	using Parlor.Diagnostics;

	static public class PfxHandler
	{
		static public PfxId Play(this RecordReference<PfxInfo>? reference, Vector3 position, Quaternion rotation)
		{
			Contract.Assume(Application.isPlaying);
			if (reference == null) return default;
			var info = reference.Info;
			if (info == null || !info.Valid) return default;
			Contract.Assert(info.Scheme != null);
			var component = Map.Provide(info.Scheme);
			component.transform.SetPositionAndRotation(position, rotation);
			component.Play();
			return component.Id;
		}
		static public PfxId Play(this RecordReference<PfxInfo>? reference, Vector3 position)
		{
			return Play(reference, position, Quaternion.identity);
		}
		static public bool Active(this PfxId id)
		{
			return id.Reference(out _);
		}
		static public bool Stop(this PfxId id, bool clear)
		{
			if (id.Reference(out var reference))
			{
				reference.Stop(clear);
				return true;
			}
			else return false;
		}
		static public void StopAll()
		{
			Contract.Assume(Application.isPlaying);
			foreach (var coll in Map)
				foreach (var elem in coll)
					elem.Stop(clear: true);
		}

		#region Internal
		static private ComponentMap<PfxComponent>? s_Map;

		static private ComponentMap<PfxComponent> Map
		{
			get
			{
				return s_Map ??= new(scheme => new(scheme, elem => elem.Active));
			}
		}
		#endregion // Internal
	}
	public struct PfxId
	{
		internal PfxId(PfxComponent? reference)
		{
			m_Reference = reference;
			Version = 0;
		}

		internal int Version;

		internal readonly bool Reference([NotNullWhen(true)] out PfxComponent? reference)
		{
			if (m_Reference != null && m_Reference.Active && m_Reference.Id.Version == Version)
			{
				reference = m_Reference;
				return true;
			}
			else
			{
				reference = null;
				return false;
			}
		}

		#region Internal
		private readonly PfxComponent? m_Reference;
		#endregion // Internal
	}
	[Serializable, EditorHandled]
	public sealed class PfxInfo : RecordInfo
	{
		internal PfxComponent? Scheme
		{
			get => m_Scheme;
		}

		#region Internal
#nullable disable
		[SerializeField, NotDefault]
		private PfxComponent m_Scheme;
#nullable enable
		#endregion // Internal
	}
	[Serializable]
	public sealed class PfxReference : RecordReference<PfxInfo> { }
	[CreateAssetMenu(menuName = "Parlor/Runtime/PfxLibrary")]
	public sealed class PfxLibrary : RecordLibrary<PfxInfo> { }
}