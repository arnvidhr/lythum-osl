using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lythum.OSL.Core.Errors
{
	public class Fix
	{
		const string DefaultNullValue = "NULL";

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

		public static string FixObjectToDb(object o)
		{
			if (IsObjectNullOrDbNull(o))
			{
				return DefaultNullValue;
			}
			else if (o is float || o is decimal || o is double)
			{
				return FixString(o.ToString().Replace(",", "."));
			}
			else
			{
				return "'" + FixString(o.ToString()) + "'";
			}

		}


		public static string[] PrepareSqlArray(object[] array, bool parseNulls)
		{
			List<string> retVal = new List<string>();

			if (array != null)
			{
				foreach (object o in array)
				{
					if ((parseNulls && DBNull.Value.Equals(o)) ||
						(parseNulls && o == null)
						)
					{
						retVal.Add("NULL");
					}
					else
					{
						retVal.Add("'" + FixString(o.ToString()) + "'");
					}

				}
			}

			return retVal.ToArray();
		}


		public static bool IsObjectNullOrDbNull(object o)
		{
			return o == null || DBNull.Value.Equals(o);
		}
	}
}
