/*
 * Created by SharpDevelop.
 * User: Arvydas Grigonis, (C) www.lythum.lt
 * Date: 2013.07.30
 * Time: 18:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
#define DISPLAY_DEBUG_INFO

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
	public class Sql : ErrorInfo, ISimpleDataAccess, ISimpleDataTransaction
	{
		#region Attributes
		DbConnection _Connection;
		DbTransaction _Transaction;
		
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
		
		/// <summary>
		/// Simple SQL Query, like Select * From table
		/// </summary>
		/// <param name="sql">Sql query</param>
		/// <returns>DataTable result</returns>
		public DataTable Query (string sql)
		{
			Error ();
			LastSql = sql;
			DateTime start = DateTime.Now;

			DataTable retVal = null;
			IDbCommand cmd = null;
			
			try
			{
				Validation.RequireValidString (sql, "sql");

				cmd = CreateCommand ();

				if (cmd != null)
				{
					cmd.CommandText = sql;

					IDataReader reader = cmd.ExecuteReader ();

					retVal = new DataTable ("R");	// R - result, just one letter to make datatable's XML serialization light
					retVal.Load (reader);

#if DISPLAY_DEBUG_INFO
					Debug.WriteLine(DateTime.Now.Subtract(start) + ">SQL_Q: " + sql);
#endif
				}
			}
			catch (Exception ex)
			{
				Error (ex);

				// throw exception if it enabled
				if (ThrowException)
					throw new LythumException (
						"Sql.Query: [" + LastSql + " ]", ex);
			}
			finally
			{
				if(cmd != null)
				{
					cmd.Dispose ();
				}

				if (CloseConnection && !IsTransactionInProgress)
				{
					Close ();
				}
			}

			
			return retVal;
		}
		
		/// <summary>
		/// Executable SQL queries, like Insert, Update, Delete
		/// </summary>
		/// <param name="sql"></param>
		public void Execute(string sql)
		{

			Error ();
			LastSql = sql;
			DateTime start = DateTime.Now;

			IDbCommand cmd = null;

			try
			{
				Validation.RequireValidString (sql, "sql");

				cmd = CreateCommand ();

				if (cmd != null)
				{
					cmd.CommandText = sql;
					cmd.ExecuteNonQuery ();
				}

#if DISPLAY_DEBUG_INFO
					Debug.WriteLine(DateTime.Now.Subtract(start) + ">SQL_E: " + sql);
#endif
			}
			catch (Exception ex)
			{
				Error (ex);

				// throw exception if it enabled
				if (ThrowException)
					throw new LythumException ("Sql.Execute [ " + LastSql + " ]", ex);
			}
			finally
			{
				if (cmd != null)
				{
					cmd.Dispose ();
				}

				// close if set to close and if transaction not in progress
				if (CloseConnection && !IsTransactionInProgress)
				{
					Close ();
				}
			}
		}

		/// <summary>
		/// Returns first DataTable's row
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		public DataRow QueryRow(string sql)
		{
			DataTable table = Query(sql);

			if (table != null)
			{
				if (table.Rows.Count > 0 && table.Columns.Count > 0)
				{
					return table.Rows[0];
				}
			}

			return null;
		}
		
		/// <summary>
		/// Method will return first column's and record's field value as string
		/// 
		/// In some cases programmer needs first column of first row value in select query.
		/// For example:
		/// 
		/// SELECT COUNT(*) FROM TABLE
		/// SELECT CASE FIELD WHEN '1' THEN 'true' ELSE 'false' END 
		/// 
		/// This method inherited ExecuteScalar name, 
		/// just regular ASP.NET's ExecuteScalar method was so limited and almost useless
		/// then required some improvement. 
		/// Not in all cases developer needs int type return value and we can't use ExecuteScalar.
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		public string QueryScalar(string sql)
		{
			DataRow row = QueryRow(sql);

			if (row != null)
			{
				return row[0].ToString();
			}

			return string.Empty;
		}
		
		#endregion
		
		#region Helpers
		
		/// <summary>
		/// Connects to database if it's not connected
		/// </summary>
		/// <returns>true if success</returns>
		public bool Connect ()
		{
			if(_Connection.State == ConnectionState.Closed){
				_Connection.Open();
			}
			
			return _Connection.State != ConnectionState.Closed;
		}
		
		
		/// <summary>
		/// Closes connection to DB if it's open and rollbacks any transaction
		/// </summary>
		void Close ()
		{
			Close (false);
		}

		public void Close (bool commit)
		{
			if (IsTransactionInProgress)
				EndTransaction (commit);

			if (_Connection.State != ConnectionState.Closed)
			{
				_Connection.Close ();
			}
		}
		
		/// <summary>
		/// Creates command for database access
		/// </summary>
		/// <returns></returns>
		public DbCommand CreateCommand ()
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


		#region ISimpleDataTransaction Members

		public bool IsTransactionInProgress
		{
			get { return _Transaction != null; }
		}

		public bool BeginTransaction ()
		{
			if (!IsTransactionInProgress)
			{
				_Transaction = _Connection.BeginTransaction ();
			}

			return IsTransactionInProgress;
		}

		public void EndTransaction (bool commit)
		{
#if DEBUG
			if (!IsTransactionInProgress && commit)
				Debug.WriteLine ("WARNING! Transaction commit without transaction!");
#endif

			if (IsTransactionInProgress)
			{
				if (commit)
				{
					_Transaction.Commit ();
				}
				else
				{
					_Transaction.Rollback ();
				}

				_Transaction.Dispose ();
				_Transaction = null;
			}
		}

		#endregion
	}
}
