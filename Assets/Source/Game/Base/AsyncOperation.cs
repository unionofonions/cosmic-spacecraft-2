#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Parlor.Game
{
	using Parlor.Diagnostics;

	public readonly struct AsyncOperation
	{
		private readonly Processor? m_Reference;
		private readonly int m_Version;

		private AsyncOperation(Processor reference)
		{
			m_Reference = reference;
			m_Version = reference.Version;
		}

		static public AsyncOperation InvokeDelayed(Action? action, AsyncTime delay)
		{
			if (action == null)
			{
				return default;
			}
			if (delay.Seconds <= 0f)
			{
				action.Invoke();
				return default;
			}
			var processor = Pool.Provide();
			processor.Restart(OpCode.Delayed);
			processor.Action = action;
			processor.Timer = delay.Seconds;
			processor.Scaled = delay.IsScaled;
			Messenger.Instance.UpdateStatus();
			return new(reference: processor);
		}
		static public AsyncOperation StartCoroutine(IEnumerator<AsyncTime>? coroutine)
		{
			if (coroutine == null) return default;
			var processor = Pool.Provide();
			processor.Restart(OpCode.Coroutine);
			processor.Coroutine = coroutine;
			(processor.Timer, processor.Scaled) = coroutine.Current;
			Messenger.Instance.UpdateStatus();
			return new(reference: processor);
		}
		static public AsyncOperation ReadFunction(Action<float>? actionf, IFunction? function, AsyncTime time)
		{
			if (actionf == null || function == null)
			{
				return default;
			}
			var processor = Pool.Provide();
			processor.Restart(OpCode.ReadFunction);
			processor.Actionf = actionf;
			processor.Function = function;
			processor.Timer = 0f;
			processor.Scaled = time.IsScaled;
			processor.TMul = 1f / time.Seconds;
			Messenger.Instance.UpdateStatus();
			return new(reference: processor);
		}

		public AsyncOperation OnEnd(Action<AsyncOperationEndType>? action)
		{
			if (TryGetReference(out var reference))
			{
				reference.OnEnd = action;
			}
			return this;
		}
		public bool IsActive()
		{
			return TryGetReference(out _);
		}
		public bool Abort()
		{
			if (TryGetReference(out var reference))
			{
				Pool.SustendAt(reference.Offset);
				Messenger.Instance.UpdateStatus();
				reference.OnEnd?.Invoke(AsyncOperationEndType.Abort);
				return true;
			}
			return false;
		}
		private bool TryGetReference([NotNullWhen(true)] out Processor? reference)
		{
			if (m_Reference == null || m_Reference.Version != m_Version || !m_Reference.Active)
			{
				reference = null;
				return false;
			}
			reference = m_Reference;
			return true;
		}

		[DefaultExecutionOrder(Int32.MaxValue)]
		private sealed class Messenger : MonoBehaviour
		{
			static private Messenger? s_Instance;

			static public Messenger Instance
			{
				get
				{
					if (s_Instance == null)
					{
						s_Instance = Parlor.Runtime.UnityHelper.CreateComponent<Messenger>(permanent: true);
					}
					return s_Instance;
				}
			}

			private void Awake()
			{
				enabled = false;
			}
			private void Update()
			{
				for (var i = Pool.Count; --i >= 0;)
				{
					if (!Pool.Collection[i].Update())
					{
						Pool.SustendAt(i);
						UpdateStatus();
						var elem = Pool.Collection[i];
						elem.OnEnd?.Invoke(AsyncOperationEndType.Complete);
					}
				}
			}
			public void UpdateStatus()
			{
				switch (Pool.Count)
				{
					case 0:
						enabled = false;
						return;
					case 1:
						enabled = true;
						return;
				}
			}
		}
		private sealed class Processor
		{
			public int Offset;
			public int Version;
			public Func<bool> Update;
			public Action Action;
			public Action<float> Actionf;
			public IFunction Function;
			public IEnumerator<AsyncTime> Coroutine;
			public float Timer;
			public float TMul;
			public bool Scaled;
			public Action<AsyncOperationEndType>? OnEnd;
			private readonly Empty m_Empty;

			public Processor()
			{
				Version = 1;
				m_Empty = new();
				Update = m_Empty.Update;
				Action = m_Empty.Action;
				Actionf = m_Empty.Actionf;
				Function = m_Empty;
				Coroutine = m_Empty.Coroutine();
			}

			public bool Active
			{
				get
				{
					return Offset < Pool.Count;
				}
			}

			public void Restart(OpCode opCode)
			{
				Update = opCode switch
				{
					OpCode.Delayed => UpdateDelayed,
					OpCode.Coroutine => UpdateCoroutine,
					OpCode.ReadFunction => UpdateReadFunction,
					_ => m_Empty.Update
				};
				OnEnd = null;
				++Version;
			}
			private bool UpdateDelayed()
			{
				Timer -= GetDeltaTime();
				if (Timer <= 0f)
				{
					Action?.Invoke();
					return false;
				}
				return true;
			}
			private bool UpdateCoroutine()
			{
				Timer -= GetDeltaTime();
				if (Timer <= 0f)
				{
					if (Coroutine.MoveNext())
					{
						(Timer, Scaled) = Coroutine.Current;
						return true;
					}
					return false;
				}
				return true;
			}
			private bool UpdateReadFunction()
			{
				Timer += GetDeltaTime() * TMul;
				float y;
				if (Timer >= 1f)
				{
					y = Function.Evaluate(1f);
					Actionf?.Invoke(y);
					return false;
				}
				y = Function.Evaluate(Timer);
				Actionf?.Invoke(y);
				return true;
			}
			private float GetDeltaTime()
			{
				return Scaled ? Time.deltaTime : Time.unscaledDeltaTime;
			}

#pragma warning disable IDE0060
			private sealed class Empty : IFunction
			{
				public bool Update()
				{
					Log.Error("AsyncOperation.Processor.Empty.Update should not be called.");
					return false;
				}
				public void Action()
				{
					Log.Error("AsyncOperation.Processor.Empty.Action should not be called.");
				}
				public void Actionf(float y)
				{
					Log.Error("AsyncOperation.Processor.Empty.Actionf should not be called.");
				}
				public IEnumerator<AsyncTime> Coroutine()
				{
					Log.Error("AsyncOperation.Processor.Empty.Coroutine should not be called.");
					yield break;
				}
				float IFunction.Evaluate(float x)
				{
					Log.Error("AsyncOperation.Processor.Empty.Evaluate should not be called.");
					return 0f;
				}
			}
#pragma warning restore IDE0060
		}
		private enum OpCode
		{
			Delayed,
			Coroutine,
			ReadFunction
		}
		static private class Pool
		{
			static public Processor[] Collection;
			static public int Count;

			static Pool()
			{
				Collection = new Processor[1];
				InitializeCollection(low: 0);
				Count = 0;
			}

			static public Processor Provide()
			{
				EnsureEnoughSpace();
				return Collection[Count++];
			}
			static public void SustendAt(int index)
			{
				Contract.Assert((uint)index < (uint)Count);
				var suspending = Collection[index];
				var swapping = Collection[--Count];
				Collection[index] = swapping;
				Collection[Count] = suspending;
				suspending.Offset = Count;
				swapping.Offset = index;
			}
			static private void InitializeCollection(int low)
			{
				for (var i = low; i < Collection.Length; ++i)
				{
					Collection[i] = new() { Offset = i };
				}
			}
			static private void EnsureEnoughSpace()
			{
				if (Count >= Collection.Length)
				{
					var newSize = Collection.Length * 2;
					Array.Resize(ref Collection, newSize);
					InitializeCollection(low: Count);
				}
			}
		}
	}
	public readonly struct AsyncTime
	{
		internal readonly float Seconds;
		internal readonly bool IsScaled;

		private AsyncTime(float seconds, bool isScaled)
		{
			Seconds = seconds;
			IsScaled = isScaled;
		}

		static public AsyncTime Scaled(float seconds)
		{
			return new(seconds, isScaled: true);
		}
		static public AsyncTime Unscaled(float seconds)
		{
			return new(seconds, isScaled: false);
		}

		internal void Deconstruct(out float seconds, out bool isScaled)
		{
			seconds = Seconds;
			isScaled = IsScaled;
		}
	}
	public enum AsyncOperationEndType
	{
		Abort,
		Complete
	}
}