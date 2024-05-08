#nullable enable

namespace Parlor
{
	public class MinEpsilonAttribute : LimitedAttribute
	{
		public MinEpsilonAttribute()
		: base(MathHelper.Epsilon)
		{
			/*nop*/
		}
	}
}