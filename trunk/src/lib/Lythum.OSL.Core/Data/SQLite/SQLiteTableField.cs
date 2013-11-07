using System;
using System.Data.Common;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Lythum.OSL.Core.Data;
using Lythum.OSL.Core.Metadata;

namespace Lythum.OSL.Core.Data.SQLite
{
	public class SQLiteTableField : DbTableField
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

		public override bool AutoIncrement
		{
			get
			{
				return base.AutoIncrement;
			}
			set
			{
				base.AutoIncrement = value;

				if (base.AutoIncrement && !this.Type.Equals(FieldTypeMap[SQLiteDataType.INTEGER]))
				{
					throw new Exception(
						"Field: " + Name + 
						", SQLite doesn't support " + SQLiteTable.SqlAutoIncrement + 
						" for other than INTEGER type field!");
				}
			}
		}

		/// <summary>
		/// Not used 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="primaryKeyAutoIncrement"></param>
		private SQLiteTableField(string name, string type, bool primaryKeyAutoIncrement)
			: base()
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
