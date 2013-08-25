using System;
using System.Data.Common;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lythum.OSL.Core.Data.SQLite
{
	public class SQLiteTableField
	{
		public string Name { get; set; }
		public string Type { get; set; }

		public bool PrimaryKey { get; set; }
		public bool AutoIncrement { get; set; }
		public bool Index { get; set; }

		public SQLiteTableField (string name, string type, bool primaryKeyAutoIncrement)
		{
			this.Name = name;
			this.Type = type;
			this.PrimaryKey = primaryKeyAutoIncrement;
			this.AutoIncrement = primaryKeyAutoIncrement;
		}

		public SQLiteTableField (string name, string type)
			: this (name, type, false)
		{
		}

	}
}
