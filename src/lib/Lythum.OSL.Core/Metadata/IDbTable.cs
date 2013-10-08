using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lythum.OSL.Core.Metadata
{
	public interface IDbTable
	{
		/// <summary>
		/// Table name
		/// </summary>
		string Name { get; set; }
		/// <summary>
		/// Table fields
		/// </summary>
		List<IDbTableField> Fields { get; }

		#region Render
		/// <summary>
		/// Renders sql select for specific table
		/// </summary>
		/// <param name="exceptFields">
		/// Specified fields which need to skip, can be null.
		/// </param>
		/// <returns>
		/// Sql select string
		/// </returns>
		string RenderSelectSql(string[] exceptFields);
		string[] RenderCreateSql();


		#endregion

		#region Methods
		/// <summary>
		/// Converts DataTable to csv string
		/// </summary>
		/// <param name="table"></param>
		/// <returns></returns>
		string ConvertToCsv(DataTable table);

		/// <summary>
		/// Converts CSV string to DataTable
		/// </summary>
		/// <param name="csvText"></param>
		/// <returns></returns>
		DataTable ConvertFromCsv(string csvText);

		#endregion
	}
}
