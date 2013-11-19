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

		List<string> FieldNames { get; }

		#region Render
		/// <summary>
		/// Renders sql select query for specific table
		/// If include and except fields are null, will be rendered query with all table's fields
		/// </summary>
		/// <param name="includeFields">
		/// Include only specified fields (priority value), can be null
		/// </param>
		/// <param name="exceptFields">
		/// Specified fields which need to skip, can be null.
		/// </param>
		/// <returns>
		/// Sql query
		/// </returns>
		string RenderSelectSql(string[] includeFields, string[] exceptFields);

		/// <summary>
		/// Renders sql insert query for specific table
		/// If include and except fields are null, will be rendered query with all table's fields
		/// </summary>
		/// <param name="includeFields">
		/// Include only specified fields (priority value), can be null
		/// </param>
		/// <param name="exceptFields">
		/// Specified fields which need to skip, can be null.
		/// </param>
		/// <returns>
		/// Sql query
		/// </returns>
		string RenderInsertSql(string[] includeFields, string[] exceptFields);
		
		string[] RenderCreateSql();

		/// <summary>
		/// Process include and except fields logic and returns specific fields which are needed
		/// </summary>
		/// <param name="includeFields"></param>
		/// <param name="exceptFields"></param>
		/// <returns></returns>
		string[] WorkoutFields(string[] includeFields, string[] exceptFields);


		#endregion

		#region Methods

		#region CSV
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

		#endregion
	}
}
