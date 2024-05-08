#nullable enable

namespace Parlor
{
	public class PercentageAttribute : LimitedAttribute
	{
		public PercentageAttribute()
		: base(0.0f, 1.0f)
		{
			/*nop*/
		}
	}
}