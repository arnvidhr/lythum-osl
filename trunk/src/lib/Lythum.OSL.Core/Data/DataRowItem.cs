using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Lythum.OSL.Core.Errors;

namespace Lythum.OSL.Core.Data
{
	public class DataRowItem
	{
		DataRow _Row;

		public DataRowItem(DataRow row)
		{
			Validation.RequireValid(row, "row");

			_Row = row;
		}

		public object this[string columnName]
		{
			get
			{
				return _Row[columnName];
			}
			set
			{
				_Row[columnName] = value;
			}
				
		}
	}
}
