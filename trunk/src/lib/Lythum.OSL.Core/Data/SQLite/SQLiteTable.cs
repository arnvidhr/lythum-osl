using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lythum.OSL.Core.Data.SQLite
{
	public class SQLiteTable
	{
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

		public string Name { get; set; } 
		public List<SQLiteTableField> Fields { get; protected set; }

		public SQLiteTable (string name)
		{
			this.Name = name;
			this.Fields = new List<SQLiteTableField> ();
		}

		public SQLiteTable (string name, SQLiteTableField[] fields)
			: this (name)
		{
			this.Fields.AddRange (fields);
		}

		public List<string> RenderCreate ()
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

			return retVal;
		}
	}
}
