using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lythum.OSL.Core.Errors
{
	public class Fix
	{
		/// <summary>
		/// Prepare string data for db
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string FixString (string text)
		{
			if (string.IsNullOrEmpty (text))
			{
				return text;
			}
			else
			{
				return text.Replace ("'", "''") // double quotes to write single quote in SQL
					.TrimEnd (' '); // trim string's end, to not write any trash
			}
		}

	}
}
