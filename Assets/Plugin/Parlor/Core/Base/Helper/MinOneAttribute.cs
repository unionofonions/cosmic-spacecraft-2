#nullable enable

namespace Parlor
{
	public class MinOneAttribute : LimitedAttribute
	{
		public MinOneAttribute()
		: base(1f)
		{
			/*nop*/
		}
	}
}