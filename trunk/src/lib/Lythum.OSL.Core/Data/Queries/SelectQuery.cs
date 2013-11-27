using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using Lythum.OSL.Core.Metadata;
using Lythum.OSL.Core.Errors;

namespace Lythum.OSL.Core.Data.Queries
{
	public class SelectQuery : Query
	{
		#region Constants
		const string DefaultSqlSelect = "SELECT";
		const string DefaultSqlFrom = "FROM";
		const string DefaultSqlJoin = "JOIN";
		const string DefaultSqlJoinOn = "ON";
		const string DefaultSqlWhere = "WHERE";
		const string DefaultSqlAnd = " AND ";	// with spaces

		const string DefaultInnerJoin = "INNER";
		const string DefaultCrossJoin = "CROSS";
		const string DefaultLeftJoin = "LEFT";
		const string DefaultRightJoin = "RIGHT";

		#endregion

		#region Properties

		public List<string> SelectFields { get; protected set; }
		public List<QueryJoin> QueryJoins { get; protected set; }
		public List<string> Conditions { get; protected set; }

		public string TableName;

		#endregion

		#region Ctor

		public SelectQuery()
		{
			this.SelectFields = new List<string>();
			this.QueryJoins = new List<QueryJoin>();
			this.Conditions = new List<string>();
		}

		public SelectQuery(
			string tableName)
			: this()
		{
			this.TableName = tableName;
		}

		public SelectQuery(
			string tableName,
			string[] selectFields)
			: this(tableName)
		{
			this.SelectFields.AddRange(selectFields);
		}

		public SelectQuery(
			string tableName,
			string[] selectFields,
			string[] conditions)
			: this(tableName, selectFields)
		{
			this.Conditions.AddRange(conditions);
		}

		public SelectQuery(
			string tableName,
			string[] selectFields,
			string[] conditions,
			QueryJoin[] queryJoins)
			: this(tableName, selectFields, conditions)
		{
			this.QueryJoins.AddRange(queryJoins);
		}

		/// <summary>
		/// Simplified constructor for small queries
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="selectField"></param>
		/// <param name="condition"></param>
		public SelectQuery(
			string tableName,
			string selectField,
			string condition)
			: this()
		{
			this.TableName = tableName;

			if (!string.IsNullOrEmpty(selectField))
				this.SelectFields.Add(selectField);

			if (!string.IsNullOrEmpty(condition))
				this.Conditions.Add(condition);
		}

		#endregion

		#region Methods

		public override string RenderString()
		{
			var result = new StringBuilder();

			result.Append(DefaultSqlSelect + " ");

			// fields
			if (SelectFields.Count > 0)
			{
				result.Append(string.Join(", ", SelectFields.ToArray()));
			}
			else
			{
				result.Append("*");
			}

			// table
			result.Append(" " + DefaultSqlFrom + " " + TableName);

			// joins
			if (QueryJoins.Count > 0)
			{
				foreach (QueryJoin qj in QueryJoins)
				{
					switch (qj.JoinType)
					{
						default:
						case QueryJoinType.Inner:
							result.Append(DefaultInnerJoin);
							break;

						case QueryJoinType.Cross:
							result.Append(DefaultCrossJoin);
							break;

						case QueryJoinType.OutterLeft:
							result.Append(DefaultLeftJoin);
							break;

						case QueryJoinType.OutterRight:
							result.Append(DefaultRightJoin);
							break;
					}

					result.Append(" " + DefaultSqlJoin + " " + qj.TableName);

					if (qj.JoinConditions.Count > 0)
					{
						result.Append(DefaultSqlJoinOn + " ");
						result.Append(string.Join(DefaultSqlAnd, qj.JoinConditions.ToArray()));
					}
				}
			}

			// conditions
			if (Conditions.Count > 0)
			{
				result.Append(" " + DefaultSqlWhere + " ");
				result.Append(string.Join(DefaultSqlAnd, Conditions.ToArray()));
				
			}

			return result.ToString();
		}

		#endregion

	}
}
