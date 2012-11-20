using System;

namespace Tweddle.Commons.Vies
{
	/// <summary>
	/// StringUtil not re-used from Tweddle.Commons to avoid dependency
	/// </summary>
	/// <remarks>
	///		<list>
	///			<item>Authored by <a href="mailto:bstrubbe@tweddle.com">Bart STRUBBE</a></item>
	///		</list>	 
	///	</remarks>
	public class StringUtil
	{
		private StringUtil()
		{
			
		}

		public static bool IsNullOrEmpty(string s)
		{
			return (s == null || s.Length == 0);
		}
	}
}
