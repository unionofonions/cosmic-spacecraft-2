
namespace Parlor.Game
{
	using System;
	using UnityEngine;

	[Serializable]
	public class Formula : IFunction
	{
		static private Formula s_Linear;
		static private Formula s_EaseIn;
		static private Formula s_EaseOut;
		static private Formula s_EaseInOut;

		[SerializeField]
		private Value m_Value;
		private Func<float, float> m_Function;

		public Formula(Func<float, float> function)
		{
			if (function == null) throw new ArgumentNullException(nameof(function));
			m_Function = function;
		}

		static public Formula Linear
		{
			get => s_Linear ??= new(x => x);
		}
		static public Formula EaseIn
		{
			get => s_EaseIn ??= new(x => x * x * x);
		}
		static public Formula EaseOut
		{
			get => s_EaseOut ??= new(x => 1 - Mathf.Pow(1 - x, 2));
		}
		static public Formula EaseInOut
		{
			get
			{
				//return s_EaseInOut ??= new(x => x < 0.5f ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2);
				return s_EaseInOut ??= new(x =>
				{
					x = Mathf.Clamp01(x);
					return x * x * (3f - 2f * x);
				});
			}
		}

		public float Evaluate(float x)
		{
			LazyLoad();
			return m_Function(x);
		}
		private void LazyLoad()
		{
			m_Function ??= (m_Value switch
			{
				Value.Linear => Linear,
				Value.EaseIn => EaseIn,
				Value.EaseOut => EaseOut,
				Value.EaseInOut => EaseInOut,
				_ => Linear
			}).m_Function;
		}

		private enum Value
		{
			Linear,
			EaseIn,
			EaseOut,
			EaseInOut
		}
	}
}