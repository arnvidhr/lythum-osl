using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Lythum.OSL.Core.Data;

namespace Lythum.OSL.Core.Data.SQLite
{
	public class SQLiteTable
	{
		#region Constants

		const string AliasMask = "a";
		const string CsvColumnDelimiter = "\t";
		const string CsvRecordDelimiter = "\r\n";

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

		}

		public SQLiteTable (string name, SQLiteTableField[] fields)
			: this (name)
		{
			this.Fields.AddRange (fields);
			ApplyFieldsAliasesToFields();
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

		public string RenderSelect()
		{
			List<string> fields = new List<string>();

			foreach (SQLiteTableField f in this.Fields)
			{
				fields.Add(f.Name);
			}

			return "SELECT " + string.Join(", ", fields.ToArray()) + " FROM " + this.Name;
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

			table.AcceptChanges();

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


		public string ConvertToCsv(DataTable table)
		{
			Errors.Validation.RequireValid(table, "table");

			var result = new StringBuilder();

			// Process rows
			foreach (DataRow r in table.Rows)
			{
				// Process fields
				foreach (SQLiteTableField f in this.Fields)
				{
					if (r[f.Name] != null || !DBNull.Value.Equals(r[f.Name]))
					{
						result.Append(
							r[f.Name].ToString().Replace(CsvColumnDelimiter, " ").
								Replace(CsvRecordDelimiter, " "));
					}

					result.Append(CsvColumnDelimiter);
				} // fields

				result.Append(CsvRecordDelimiter);

			} // rows

			return result.ToString();
		}

		public DataTable ConvertToDataTable(string csvData)
		{
			DataTable table = new DataTable(this.Name);

			// creating table columns
			foreach (SQLiteTableField f in this.Fields)
			{
				table.Columns.Add(f.Name);
			}

			StringReader sr = new StringReader(csvData);

			while(true)
			{
				// Reading lines
				string line = sr.ReadLine();

				if (line == null)
					break;

				DataRow row = table.NewRow();
				string[] nodes = line.Split(CsvColumnDelimiter[0]);

				for (int i = 0; i < this.Fields.Count; i++)
				{
					SQLiteTableField f = this.Fields[i];

					if (string.IsNullOrEmpty(nodes[i]))
					{
						row[f.Name] = DBNull.Value;
					}
					else
					{
						row[f.Name] = nodes[i];
					}
				}

				table.Rows.Add(row);
			}

			table.AcceptChanges();

			return table;
		}

		#endregion

	}
}
