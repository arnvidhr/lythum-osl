﻿using System;
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
		public const string SqlCreateTable = "CREATE TABLE IF NOT EXISTS {0} ( {1} )";
		public const string SqlPrimaryKey = "PRIMARY KEY";
		public const string SqlAutoIncrement = "AUTOINCREMENT";
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

				// auto increment or default value
				if (f.AutoIncrement)
				{
					fieldSql += " " + SqlAutoIncrement;
				}
				else if (!string.IsNullOrEmpty(f.DefaultValue))
				{
					fieldSql += " DEFAULT " + f.DefaultValue;

				}

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
