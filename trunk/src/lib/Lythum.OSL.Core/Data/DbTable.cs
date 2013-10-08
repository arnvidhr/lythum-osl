using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

using Lythum.OSL.Core.Metadata;

namespace Lythum.OSL.Core.Data
{
	public abstract class DbTable : IDbTable
	{
		#region Constants
		const string AliasMask = "a";
		const string CsvColumnDelimiter = "\t";
		const string CsvRecordDelimiter = "\r\n";

		#endregion

		#region Properties

		public string Name { get; set; }
		public List<IDbTableField> Fields { get; protected set; }

		#endregion

		#region Ctor

		public DbTable (string name)
		{
			this.Name = name;
			this.Fields = new List<IDbTableField> ();

		}

		public DbTable (string name, IDbTableField[] fields)
			: this (name)
		{
			this.Fields.AddRange (fields);
			AliasFiels();
		}

		#endregion


		#region Methods

		#region Helpers

		protected virtual void AliasFiels()
		{
			int index = 1;

			foreach (IDbTableField f in Fields)
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

		#region Render
		public virtual string[] RenderCreateSql()
		{
			throw new NotImplementedException();
		}


		public virtual string RenderSelectSql(string[] exceptFields)
		{
			List<string> fields = new List<string>();

#warning todo: implement exceptFields

			foreach (IDbTableField f in this.Fields)
			{
				fields.Add(f.Name);
			}

			return "SELECT " + string.Join(", ", fields.ToArray()) + " FROM " + this.Name;

		}

		#endregion

		#region CSV

		public virtual string ConvertToCsv(DataTable table)
		{
			Errors.Validation.RequireValid(table, "table");

			var result = new StringBuilder();

			// Process rows
			foreach (DataRow r in table.Rows)
			{
				// Process fields
				foreach (IDbTableField f in this.Fields)
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

		public virtual DataTable ConvertFromCsv(string csvData)
		{
			DataTable table = new DataTable(this.Name);

			// creating table columns
			foreach (IDbTableField f in this.Fields)
			{
				table.Columns.Add(f.Name);
			}

			StringReader sr = new StringReader(csvData);

			while (true)
			{
				// Reading lines
				string line = sr.ReadLine();

				if (line == null)
					break;

				DataRow row = table.NewRow();
				string[] nodes = line.Split(CsvColumnDelimiter[0]);

				for (int i = 0; i < this.Fields.Count; i++)
				{
					IDbTableField f = this.Fields[i];

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

		#endregion // CSV

		#endregion // Methods


	}
}
