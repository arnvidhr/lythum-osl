/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.11.19
 * Time: 19:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Lythum.OSL.Core.Errors;
using Lythum.OSL.Core.Metadata;

namespace Lythum.OSL.Core.Data
{
	/// <summary>
	/// Purpose of this class is to segmentate very big database tables objects, where db table has a lot of fields. 
	/// In one PDA project was found performance issues regarding of getting to much db record's fields at once.
	/// So was decided to split these all fields in few data groups, for what was developed this class. 
	/// 
	/// First at design time to split specific database table record data into few DataGroups 
	/// and to retrieve it only when it needed, normally at different phases of data processing, 
	/// regarding to some project's business logics.
	/// 
	/// Basically it designed to get initialized with IDbTable where it can get db table's structure, some sql queries render
	/// and when requested to return specified for DataGroup fields values. 
	/// Maybe was possible to use for this Nullable infrastructure, but some database record values can be null
	/// </summary>
	public class DataGroup
	{
		#region Attributes
		#endregion

		#region Properties

		public bool IsDataLoaded { get; protected set; }

		/// <summary>
		/// Can be null if nothing found.
		/// </summary>
		public DataRow Row
		{
			get
			{
				// If data is not loaded
				if (!IsDataLoaded)
				{
					// Building sql query
					string sql = Table.RenderSelectSql(
						IncludeFields,
						null);

					// Adding primary key condition
					sql += " " + WhereCause;

					// Querying
					DataGroupRow = DataAccess.QueryRow(sql);

					IsDataLoaded = true;
				}

				return DataGroupRow;
			}
		}

		protected IDbTable Table;
		protected string[] IncludeFields;
		protected string WhereCause;
		protected ISimpleDataAccess DataAccess;
		protected DataRow DataGroupRow;


		#endregion

		#region Ctor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="table">
		/// IdbTable with defined structure
		/// </param>
		/// <param name="includeFields">
		/// Which fields are included in data group
		/// </param>
		/// <param name="whereCause">
		/// Where cause to select corresponding record
		/// </param>
		public DataGroup(
			IDbTable table, 
			ISimpleDataAccess dataAccess,
			string[] includeFields, 
			string whereCause)
		{
			Validation.RequireValid(table, "table");
			Validation.RequireValid(dataAccess, "dataAccess");
			Validation.RequireValid(includeFields, "includeFields");
			Validation.RequireValidString(whereCause, "whereCause");

			if (includeFields.Length < 1)
				throw new LythumException("DataGroup: Must be at least one data group field!");

			this.Table = table;
			this.DataAccess = dataAccess;
			this.IncludeFields = includeFields;
			this.WhereCause = whereCause;

			this.IsDataLoaded = false;
		}

		public DataGroup(DataRow row)
		{
			Validation.RequireValid(row, "row");

			this.DataGroupRow = row;
			this.IsDataLoaded = true;
		}

		#endregion
	}
}
