/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.07.30
 * Time: 18:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

using Lythum.OSL.Core.Metadata;
using Lythum.OSL.Core.Errors;

namespace Lythum.OSL.Core.Data
{
	/// <summary>
	/// Class which works with ADO.NET drivers
	/// Designed to simplify database access
	/// </summary>
	public class Sql : ErrorInfo
	{
		#region Attributes
		DbConnection _Connection;
		
		#endregion
		
		#region Properties
		/// <summary>
		/// Close connection after query
		/// </summary>
		public bool CloseConnection { get; set; }
		public bool ThrowException { get; set; }
		public string LastSql { get; protected set; }
		
		#endregion
		
		#region ctor
		
		private Sql()
			: base ()
		{
			ThrowException = true;
			CloseConnection = true;
			LastSql = string.Empty;
		}
		
		public Sql (DbConnection connection)
			: this ()
		{
			Validation.RequireValid(connection, "connection");
			
			_Connection = connection;
		}
		
		#endregion
		
		#region ISimpleDataAccess
		
		public DataTable Query (string sql)
		{
			Error ();
			DataTable retVal = null;
			IDbCommand cmd = null;
			
			try
			{
				LastSql = sql;
				Debug.Print ("SQL::Query: " + sql);
				Validation.RequireValidString (sql, "sql");

				cmd = CreateCommand (true);

				if (cmd != null)
				{
					cmd.CommandText = sql;

					IDataReader reader = cmd.ExecuteReader ();

					retVal = new DataTable ("R");	// R - result, just one letter to make datatable's XML serialization light
					retVal.Load (reader);
				}
			}
			catch (Exception ex)
			{
				Error (ex);

				// throw exception if it enabled
				if (ThrowException)
					throw new LythumException (
						"Lythum.OSL.Core.Data.Sql.Query [ " + LastSql + " ]", ex);
			}
			finally
			{
				if(cmd != null)
				{
					cmd.Dispose ();
				}

				if (CloseConnection)
				{
					Close ();
				}
			}

			
			return retVal;
		}
		
		public void Execute(string sql)
		{
			Debug.Print("SQL::Execute: " + sql);
		}
		
		public string QueryScalar(string sql)
		{
			DataTable table = Query (sql);

			if (table != null)
			{
				if (table.Rows.Count > 0 && table.Columns.Count > 0)
				{
					return table.Rows[0].ItemArray[0].ToString ();
				}
			}

			return string.Empty;
		}
		
		#endregion
		
		#region Helpers
		bool Connect ()
		{
			if(_Connection.State == ConnectionState.Closed){
				_Connection.Open();
			}
			
			return _Connection.State != ConnectionState.Closed;
		}
		
		void Close ()
		{

			if(_Connection.State != ConnectionState.Closed){
				_Connection.Close();
			}
		}
		
		/// <summary>
		/// This function will create command for database access
		/// </summary>
		/// <returns></returns>
		public DbCommand CreateCommand (bool connect)
		{
			DbCommand cmd = null;

			try
			{
				if (Connect ())
				{
					cmd = _Connection.CreateCommand ();
					cmd.CommandType = CommandType.Text;
				}
			}
			catch (Exception ex)
			{
				Error (ex);

				// throw exception if it enabled
				if (ThrowException)
					throw new LythumException (ex);
			}

			return cmd;
		}

		
		
		#endregion
		
	}
}
