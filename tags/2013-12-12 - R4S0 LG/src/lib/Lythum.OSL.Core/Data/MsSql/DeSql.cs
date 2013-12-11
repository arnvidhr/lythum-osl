using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lythum.OSL.Core.Data.MsSql
{
	public class DeSql
	{
		/// <summary>
		/// Method replaces MSSQL variables, eg. @date, @value & etc to specific values, specified as parameters values
		/// </summary>
		/// <param name="sqlQuery">Original MSSQL T-SQL query</param>
		/// <param name="parameters">Parametters key, eg. @date, value, eg. '2012-05-05' </param>
		/// <returns></returns>
		public static string DeSqlQuery(
			string sqlQuery, 
			Dictionary<string, string> parameters)
		{
			string retVal = sqlQuery;

			foreach (KeyValuePair<string, string> pair in parameters)
			{
				if (pair.Key[0].Equals('@'))
				{
					retVal = retVal.Replace(pair.Key, pair.Value);
				}
				else
				{
					retVal = retVal.Replace("@" + pair.Key, pair.Value);
				}
			}

			return retVal;
		}
	}
}
