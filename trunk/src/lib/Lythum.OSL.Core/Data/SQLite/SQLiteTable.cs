using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Lythum.OSL.Core.Data;
using Lythum.OSL.Core.Metadata;

namespace Lythum.OSL.Core.Data.SQLite
{
	public class SQLiteTable : DbTable
	{
		#region Constants

		/// <summary>
		/// {0} table name, {1} all fields creation info
		/// </summary>
		const string SqlCreateTable = "CREATE TABLE IF NOT EXISTS {0} ( {1} )";
		const string SqlPrimaryKey = "PRIMARY KEY";
		const string SqlAutoIncrement = "AUTOINCREMENT";
		/// <summary>
		/// {0} table, {1} column
		/// </summary>
		const string SqlCreateIndex = "CREATE INDEX {0}_{1} ON {0} ({1})";

		#endregion

		#region Attributes

		#endregion

		#region Ctor
		public SQLiteTable(string name)
			: base(name)
		{
		}

		public SQLiteTable(string name, IDbTableField[] fields)
			: base(name, fields)
		{
		}

		#endregion

		#region Helpers

		#endregion

		#region Methods

		public override string[] RenderCreateSql()
		{
			List<string> retVal = new List<string> ();
			List<string> createFields = new List<string>();

			// create table
			foreach (SQLiteTableField f in this.Fields)
			{
				string fieldSql = f.Name + " " + f.Type;

				if (f.PrimaryKey)
				{
					fieldSql += " " + SqlPrimaryKey;
				}
				/*
				if (f.AutoIncrement)
				{
					fieldSql += " " + SqlAutoIncrement;
				}
				*/
				createFields.Add (fieldSql);
			}

			retVal.Add (string.Format(
				SqlCreateTable,
				this.Name,
				string.Join (", ", createFields.ToArray ())
				));

			// indexes
			foreach (SQLiteTableField f in this.Fields)
			{
				if (f.Index)
				{
					retVal.Add (string.Format (
						SqlCreateIndex,
						this.Name,
						f.Name
						));
				}
			}

			return retVal.ToArray();
		}

		#endregion

	}
}
