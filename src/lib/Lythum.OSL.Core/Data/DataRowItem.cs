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
		protected DataRow Row;

		protected DataRowItem()
		{
			this.Row = null;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="row">
		/// Can be DataRow or DataRowView. 
		/// Will be validated.
		/// </param>
		public DataRowItem(object row)
			: this()
		{
			Validation.RequireValid(row, "row");

			if (row is DataRowView)
			{
				DataRowView drv = (DataRowView)row;

				Validation.RequireValid(drv.Row, "DataRowView.Row");

				this.Row = drv.Row;
			}
			else if (row is DataRow)
			{
				this.Row = (DataRow)row;
			}
			else
			{
				throw new LythumException("Unknown DataRow type!");
			}
		}

		public object this[string columnName]
		{
			get
			{
				ValidateRow();

				return this.Row[columnName];
			}
			set
			{
				ValidateRow();

				this.Row[columnName] = value;
			}
				
		}

		void ValidateRow()
		{
			if (this.Row == null)
			{
				throw new LythumException(this.GetType().ToString() + " Row is null!");
			}
		}
	}
}
