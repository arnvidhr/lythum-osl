using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lythum.OSL.Core
{
	public class Helpers
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="format">Used default format if empty</param>
		/// <returns></returns>
		public static string GetCurrentDate(string format)
		{
			return DateTime.Now.ToString(string.IsNullOrEmpty(format) ? Defaults.DateFormat : format);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="format">Used default format if empty</param>
		/// <returns></returns>
		public static string GetCurrentTime(string format)
		{
			return DateTime.Now.ToString(string.IsNullOrEmpty(format) ? Defaults.TimeFormat : format);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="format">Used default format if empty</param>
		/// <returns></returns>
		public static string GetCurrentDateTime(string format)
		{
			return DateTime.Now.ToString(string.IsNullOrEmpty(format) ? Defaults.DateTimeFormat : format);
		}
	}
}
