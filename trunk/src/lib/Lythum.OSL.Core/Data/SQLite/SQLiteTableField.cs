using System;
using System.Data.Common;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Lythum.OSL.Core.Data.SQLite
{
	public class SQLiteTableField
	{
        static Dictionary<SQLiteDataType, string> FieldTypeMap =
            new Dictionary<SQLiteDataType, string>()
            {
				{ SQLiteDataType.INT, "INT" },
				{ SQLiteDataType.INTEGER, "INTEGER" },
				{ SQLiteDataType.TINYINT, "TINYINT" },
				{ SQLiteDataType.SMALLINT, "SMALLINT" },
				{ SQLiteDataType.MEDIUMINT, "MEDIUMINT" },
				{ SQLiteDataType.BIGINT, "BIGINT" },
				{ SQLiteDataType.UNSIGNED_BIG_INT, "UNSIGNED BIG INT" },
				{ SQLiteDataType.INT2, "INT2" },
				{ SQLiteDataType.INT8, "INT8" },

				{ SQLiteDataType.TEXT, "TEXT" },

				{ SQLiteDataType.BLOB, "BLOB" },

				{ SQLiteDataType.REAL, "REAL" },
				{ SQLiteDataType.DOUBLE, "DOUBLE" },
				{ SQLiteDataType.DOUBLE_PRECISION, "DOUBLE PRECISION" },
				{ SQLiteDataType.FLOAT, "FLOAT" },

				{ SQLiteDataType.NUMERIC, "NUMERIC" },
				{ SQLiteDataType.DECIMAL_10_5, "DECIMAL(10,5)" },
				{ SQLiteDataType.DECIMAL_10_4, "DECIMAL(10,4)" },   // Accounting
				{ SQLiteDataType.DECIMAL_10_3, "DECIMAL(10,3)" },   // Half precision
				{ SQLiteDataType.DECIMAL_10_2, "DECIMAL(10,2)" },   // Money
				{ SQLiteDataType.BOOLEAN, "BOOLEAN" },
				{ SQLiteDataType.DATE, "DATE" },
				{ SQLiteDataType.DATETIME, "DATETIME" },
            };

		/// <summary>
		/// Field name
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Field type
		/// </summary>
		public string Type { get; set; }
		/// <summary>
		/// Field alias, used for XML serialization to shortcut XML tags.
		/// Normally this field is automatically initialized on SQLiteTable creation 
		/// and don't require to manual initialization.
		/// </summary>
		public string Alias { get; set; }

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

		public SQLiteTableField(string name, SQLiteDataType type, bool primaryKeyAutoIncrement)
            : this(name, ConvertToStringType(type), primaryKeyAutoIncrement)
        {
        }

		public SQLiteTableField(string name, SQLiteDataType type)
			: this(name, type, false)
		{
		}

		/// <summary>
		/// Converts SQLite data type enum to string
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
        static string ConvertToStringType(SQLiteDataType type)
        {
			// Try to find
			if (FieldTypeMap.ContainsKey(type))
			{
				return FieldTypeMap[type];
			}
			// if not found return TEXT type
			else
			{
				return FieldTypeMap[SQLiteDataType.TEXT];
			}
        }

	}
}
