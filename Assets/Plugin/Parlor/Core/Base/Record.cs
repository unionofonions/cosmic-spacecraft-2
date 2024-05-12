#nullable enable

namespace Parlor
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Diagnostics;
	using UnityEngine;
	using Parlor.Diagnostics;

	[EditorHandled]
	public class RecordInfo
	{
		public string Name
		{
			get => m_Name;
		}
		internal int Id
		{
			get => m_Id;
		}
		public bool Valid
		{
			get => m_Valid;
		}

		#region Internal
#nullable disable
		[SerializeField, NotDefault]
		private string m_Name;
		[SerializeField, ReadOnly]
		private int m_Id;
		[SerializeField, ReadOnly]
		private bool m_Valid;
#nullable enable
		#endregion // Internal
	}
	[Serializable, EditorHandled]
	public class RecordReference<T> where T : RecordInfo
	{
		public T? Info
		{
			get
			{
#if UNITY_EDITOR
				return m_Library != null ? m_Library.InfoById(m_Id) : null;
#else
				return m_Info ??= (m_Library != null ? m_Library.InfoById(m_Id) : null);
#endif
			}
		}

		#region Internal
#nullable disable
		[SerializeField, HideInInspector]
		private RecordLibrary<T> m_Library;
		[SerializeField, HideInInspector]
		private int m_Id;
		[NonSerialized]
		private T m_Info;
#nullable enable
#endregion // Internal
	}
	[EditorHandled]
	public class RecordLibrary<T> : ScriptableObject, IEnumerable<T> where T : RecordInfo
	{
		internal T? InfoById(int id)
		{
			if (id == m_Default.Id) return m_Default;
			if (m_OrderedHeap == null)
			{
				m_OrderedHeap = new T[m_Heap.Length];
				Array.Copy(m_Heap, m_OrderedHeap, m_Heap.Length);
				Array.Sort(m_OrderedHeap, (op1, op2) => op1.Id - op2.Id);
			}
			var low = 0;
			var high = m_OrderedHeap.Length - 1;
			while (low <= high)
			{
				var mid = (low + high) / 2;
				var _id = m_OrderedHeap[mid].Id;
				if (id > _id) low = mid + 1;
				else if (id < _id) high = mid - 1;
				else return m_OrderedHeap[mid];
			}
			return null;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return new Enumerator(this);
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
		[Conditional("UNITY_EDITOR")]
		protected void OnValidate()
		{
			m_OrderedHeap = null;
		}

		#region Internal
#nullable disable
		[SerializeField]
		private T m_Default;
		[SerializeField]
		private T[] m_Heap;
		[SerializeField, ReadOnly]
		private int m_NextId;
		[NonSerialized]
		private T[] m_OrderedHeap;

		private struct Enumerator : IEnumerator<T>
		{
			public Enumerator(RecordLibrary<T> reference)
			{
				m_Reference = reference;
				m_Current = default;
				m_Index = -1;
			}

			private readonly RecordLibrary<T> m_Reference;
			private T m_Current;
			private int m_Index;

			public readonly T Current
			{
				get => m_Current;
			}
			readonly object IEnumerator.Current
			{
				get => Current;
			}

			public bool MoveNext()
			{
				if (++m_Index < m_Reference.m_Heap.Length + 1)
				{
					m_Current = m_Index == 0 ? m_Reference.m_Default : m_Reference.m_Heap[m_Index - 1];
					return true;
				}
				else
				{
					m_Current = default;
					m_Index = m_Reference.m_Heap.Length + 1;
					return false;
				}
			}
			public void Reset()
			{
				m_Current = default;
				m_Index = -1;
			}
			public readonly void Dispose()
			{
				/*nop*/
			}
		}
#nullable enable
		#endregion // Internal
	}
	static public class RecordDatabase
	{
		static public object[]? LibrariesByInfoType(Type infoType)
		{
			if (infoType == null)
				throw new ArgumentNullException(nameof(infoType));
			if (!infoType.IsSubclassOf(typeof(RecordInfo)))
				throw new ArgumentException("Argument infoType must derive from RecordInfo.", nameof(infoType));
			if (!s_Map.TryGetValue(infoType, out var ret))
				s_Map.Add(infoType, ret = Load());
			return ret;
			object[]? Load()
			{
				var libType = LibraryType();
				return libType != null
					? Resources.LoadAll(path: String.Empty, libType)
					: null;
			}
			Type? LibraryType()
			{
				var baseType = typeof(RecordLibrary<>).MakeGenericType(infoType);
				return infoType
					.Assembly
					.GetTypes()
					.FirstOrDefault(type => type.IsSubclassOf(baseType));
			}
		}
		static public void ClearAll()
		{
			s_Map.Clear();
		}

		#region Internal
		static RecordDatabase()
		{
			s_Map = new();
		}

		static private readonly Dictionary<Type, object[]?> s_Map;
		#endregion // Internal
	}
}