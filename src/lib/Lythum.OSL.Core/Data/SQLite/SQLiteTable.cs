using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lythum.OSL.Core.Data.SQLite
{
	public class SQLiteTable
	{
		#region Constants

		const string AliasMask = "a";

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

		public string Name { get; set; } 
		public List<SQLiteTableField> Fields { get; protected set; }

		#endregion

		#region Ctor
		public SQLiteTable (string name)
		{
			this.Name = name;
			this.Fields = new List<SQLiteTableField> ();

			ApplyFieldsAliasesToFields();
		}

		public SQLiteTable (string name, SQLiteTableField[] fields)
			: this (name)
		{
			this.Fields.AddRange (fields);

		}

		#endregion

		#region Helpers
		void ApplyFieldsAliasesToFields()
		{
			int index = 1;

			foreach (SQLiteTableField f in Fields)
			{
				// apply automatic alias only if alias is not defined
				if (string.IsNullOrEmpty(f.Alias))
				{
					f.Alias = AliasMask + index;
					index++;
				}
			}
		}

		#endregion

		#region Methods

		public List<string> RenderCreate()
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

		/// <summary>
		/// Applies field aliases to DataTable columns names (Obfuscation)
		/// </summary>
		/// <param name="t"></param>
		/// <returns>count of aliased columns</returns>
		public int AliasTableColumns(DataTable table)
		{
			int retVal = 0;

			Errors.Validation.RequireValid(table, "table");

			// check all fields
			foreach (SQLiteTableField f in Fields)
			{
				// if such column exist
				if (table.Columns.Contains(f.Name))
				{
					DataColumn c = table.Columns[f.Name];

					// if this column name not equal field alias name
					if (!c.ColumnName.Equals(f.Alias))
					{
						// apply alias
						c.ColumnName = f.Alias;

						// counting aliased fields
						retVal++;
					} // if
				} // if
			} // foreach

			return retVal;
		}

		public int UnaliasTableColumns (DataTable table)
		{
			int retVal = 0;

			Errors.Validation.RequireValid(table, "table");

			// check all fields
			foreach (SQLiteTableField f in Fields)
			{
				// if such column exist
				if (table.Columns.Contains(f.Alias))
				{
					// rename column name to real name
					table.Columns[f.Alias].ColumnName = f.Name;

					// counting aliased fields
					retVal++;
				} // if
			} // foreach


			return retVal;
		}

		#endregion

	}
}
